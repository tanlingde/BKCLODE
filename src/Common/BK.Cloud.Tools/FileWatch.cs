using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace BK.Cloud.Tools
{
    public class FileWatch
    {
        public bool IsText = true;
        private MyFileSystemWather watcher;

        public FileWatch(string path, string filter)
        {
            WatchStart(path, filter);
        }

        public List<string> ModifyedMessage { get; set; }

        /// <summary>
        /// 文件被修改时触发
        /// </summary>
        public event Action<string> FileContentChanged;

        private void WatchStart(string path, string filter)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fullPath = Path.Combine(path, filter);
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath);
            }
            //FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //ModifyedMessage = new List<string>();
            //while (!sr.EndOfStream)
            //{
            //    ModifyedMessage.Add(sr.ReadLine());
            //}
            //sr.Close();
            //fs.Close();
            watcher = new MyFileSystemWather(path, filter);

            watcher.OnChanged += OnChanged;
            watcher.OnCreated += OnCreated;
            watcher.OnRenamed += OnRenamed;
            watcher.OnDeleted += OnDeleted;
        }

        public void Start()
        {
            if (watcher != null)
                watcher.Start();
        }

        public void Stop()
        {
            try
            {
                if (watcher != null)
                    watcher.Stop();
            }
            catch (Exception)
            {
            }
        }

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
            }

            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                OnChanged(source, e);
            }

            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                OnDeleted(source, e);
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string text = string.Empty;
            if (IsText)
            {
                var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var sr = new StreamReader(fs, Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    text = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            // ModifyedMessage.Add(text);
            if (FileContentChanged != null)
            {
                if (IsText)
                {
                    FileContentChanged(text);
                }
                else
                {
                    FileContentChanged(e.FullPath);
                }
            }
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
        }
    }

    public delegate void Completed(string key);


    public class MyFileSystemWather
    {
        private readonly FileSystemWatcher fsWather;


        private readonly Hashtable hstbWather;

        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="path">要监控的路径</param> 
        public MyFileSystemWather(string path, string filter)
        {
            if (!Directory.Exists(path))
            {
                throw new Exception("找不到路径：" + path);
            }


            hstbWather = new Hashtable();


            fsWather = new FileSystemWatcher(path);
            //fsWather.NotifyFilter = NotifyFilters.LastWrite;
            // 是否监控子目录

            fsWather.IncludeSubdirectories = false;

            fsWather.Filter = filter;

            fsWather.Renamed += fsWather_Renamed;

            fsWather.Changed += fsWather_Changed;

            fsWather.Created += fsWather_Created;

            fsWather.Deleted += fsWather_Deleted;
        }


        public event RenamedEventHandler OnRenamed;

        public event FileSystemEventHandler OnChanged;

        public event FileSystemEventHandler OnCreated;

        public event FileSystemEventHandler OnDeleted;


        /// <summary> 
        /// 开始监控 
        /// </summary> 
        public void Start()
        {
            fsWather.EnableRaisingEvents = true;
        }


        /// <summary> 
        /// 停止监控 
        /// </summary> 
        public void Stop()
        {
            fsWather.EnableRaisingEvents = false;
        }


        /// <summary> 
        /// filesystemWatcher 本身的事件通知处理过程 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        private void fsWather_Renamed(object sender, RenamedEventArgs e)
        {
            lock (hstbWather)
            {
                hstbWather.Add(e.FullPath, e);
            }


            var watcherProcess = new WatcherProcess(sender, e);

            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;

            watcherProcess.OnRenamed += WatcherProcess_OnRenamed;

            var thread = new Thread(watcherProcess.Process);

            thread.Start();
        }


        private void WatcherProcess_OnRenamed(object sender, RenamedEventArgs e)
        {
            OnRenamed(sender, e);
        }


        private void fsWather_Created(object sender, FileSystemEventArgs e)
        {
            lock (hstbWather)
            {
                hstbWather.Add(e.FullPath, e);
            }

            var watcherProcess = new WatcherProcess(sender, e);

            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;

            watcherProcess.OnCreated += WatcherProcess_OnCreated;

            var threadDeal = new Thread(watcherProcess.Process);

            threadDeal.Start();
        }


        private void WatcherProcess_OnCreated(object sender, FileSystemEventArgs e)
        {
            OnCreated(sender, e);
        }


        private void fsWather_Deleted(object sender, FileSystemEventArgs e)
        {
            lock (hstbWather)
            {
                hstbWather.Add(e.FullPath, e);
            }

            var watcherProcess = new WatcherProcess(sender, e);

            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;

            watcherProcess.OnDeleted += WatcherProcess_OnDeleted;

            var tdDeal = new Thread(watcherProcess.Process);

            tdDeal.Start();
        }


        private void WatcherProcess_OnDeleted(object sender, FileSystemEventArgs e)
        {
            OnDeleted(sender, e);
        }


        private void fsWather_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (hstbWather.ContainsKey(e.FullPath))
                {
                    WatcherChangeTypes oldType = ((FileSystemEventArgs) hstbWather[e.FullPath]).ChangeType;

                    if (oldType == WatcherChangeTypes.Created || oldType == WatcherChangeTypes.Changed)
                    {
                        return;
                    }
                }
            }


            lock (hstbWather)
            {
                hstbWather.Add(e.FullPath, e);
            }

            var watcherProcess = new WatcherProcess(sender, e);

            watcherProcess.OnCompleted += WatcherProcess_OnCompleted;

            watcherProcess.OnChanged += WatcherProcess_OnChanged;

            var thread = new Thread(watcherProcess.Process);

            thread.Start();
        }


        private void WatcherProcess_OnChanged(object sender, FileSystemEventArgs e)
        {
            OnChanged(sender, e);
        }


        public void WatcherProcess_OnCompleted(string key)
        {
            lock (hstbWather)
            {
                hstbWather.Remove(key);
            }
        }
    }

    public class WatcherProcess
    {
        private readonly object eParam;
        private readonly object sender;

        public WatcherProcess(object sender, object eParam)
        {
            this.sender = sender;

            this.eParam = eParam;
        }


        public event RenamedEventHandler OnRenamed;

        public event FileSystemEventHandler OnChanged;

        public event FileSystemEventHandler OnCreated;

        public event FileSystemEventHandler OnDeleted;

        public event Completed OnCompleted;


        public void Process()
        {
            if (eParam.GetType() == typeof (RenamedEventArgs))
            {
                OnRenamed(sender, (RenamedEventArgs) eParam);

                OnCompleted(((RenamedEventArgs) eParam).FullPath);
            }

            else
            {
                var e = (FileSystemEventArgs) eParam;

                if (e.ChangeType == WatcherChangeTypes.Created)
                {
                    OnCreated(sender, e);

                    OnCompleted(e.FullPath);
                }

                else if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    OnChanged(sender, e);

                    OnCompleted(e.FullPath);
                }

                else if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    OnDeleted(sender, e);

                    OnCompleted(e.FullPath);
                }

                else
                {
                    OnCompleted(e.FullPath);
                }
            }
        }
    }
}
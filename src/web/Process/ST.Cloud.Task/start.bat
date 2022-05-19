@ECHO OFF
echo ====Install Services====
sc delete BK.Cloud.Task
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %~dp0BK.Cloud.Task.exe
net start BK.Cloud.Task
 
@ECHO OFF
echo ====Install Services====
sc delete ST.Cloud.Task
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %~dp0ST.Cloud.Task.exe
net start ST.Cloud.Task
 
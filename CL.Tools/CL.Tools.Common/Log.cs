using System;
using System.IO;
using System.Text;

namespace CL.Tools.Common
{
    /// <summary>
    /// Log 的摘要说明
    /// </summary>
    public class Log
    {
        private string pathName;
        private int logFileMaxSize_MB = 10;

        public Log(string pathname)
        {
            if (String.IsNullOrEmpty(pathname))
            {
                throw new Exception("没有初始化 Log 类的 PathName 变量");
            }

            pathName = AppDomain.CurrentDomain.BaseDirectory + "App_Log\\" + pathname;

            if (!Directory.Exists(pathName))
            {
                try
                {
                    Directory.CreateDirectory(pathName);
                }
                catch {
                    throw;
                }
            }

            if (!Directory.Exists(pathName))
            {
                pathName = AppDomain.CurrentDomain.BaseDirectory + "App_Log";

                if (!Directory.Exists(pathName))
                {
                    try
                    {
                        Directory.CreateDirectory(pathName);
                    }
                    catch {
                        throw;
                    }
                }

                if (!Directory.Exists(pathName))
                {
                    pathName = AppDomain.CurrentDomain.BaseDirectory;
                }
            }

        }

        private String FileName
        {
            get
            {
                int index = 1;
                DateTime dateNow = DateTime.Now;
                string logFileFormat = "{0}/{1:yyyy-MM-dd}_{2}.log";
                string logFileName = string.Format(logFileFormat, pathName, dateNow, index);

                FileInfo fileInfo = null;
                while (true)
                {
                    fileInfo = new FileInfo(logFileName);
                    if (fileInfo.Exists)
                    {
                        if (fileInfo.Length > logFileMaxSize_MB * 1024 * 1024)
                        {
                            logFileName = string.Format(logFileFormat, pathName, dateNow, ++index);
                        }
                        else { break; }
                    }
                    else { break; }
                }
                return logFileName;
            }
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Message"></param>
        public void Write(string Message, bool IsError = false)
        {
            if (String.IsNullOrEmpty(FileName)) return;

            lock ("FileLog_{758FC277-9B7E-4733-AD4A-140104F77F84}_" + FileName)
            {
                try
                {
                    if (IsError)
                        using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Write))
                        {
                            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GBK"));
                            sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            sw.Write(":");
                            sw.Write(DateTime.Now.Millisecond);
                            sw.Write("\t\t");
                            sw.Write(Message);
                            sw.Write("\r\n");
                            sw.Close();
                        }
                    else
                    {
#if Debug
                        using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Write))
                        {
                            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GBK"));
                            sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            sw.Write(":");
                            sw.Write(DateTime.Now.Millisecond);
                            sw.Write("\t\t");
                            sw.Write(Message);
                            sw.Write("\r\n");
                            sw.Close();
                        }
#endif
                    }
                }
                catch (Exception ex)
                {
                    var log_dir = AppDomain.CurrentDomain.BaseDirectory + "App_Log/";
                    if (!Directory.Exists(log_dir))
                    {
                        Directory.CreateDirectory(log_dir);
                    }
                    File.AppendAllText(log_dir + DateTime.Now.ToString("yyyy-MM-dd") + ".log",
                        String.Format("{0:yyyy-MM-dd HH:mm:ss} 日志写入异常 {1} {2} {3}", DateTime.Now, pathName, Message, ex));
                }
            }
        }

    }
}

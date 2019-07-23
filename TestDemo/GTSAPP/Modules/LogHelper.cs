using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace TestReader.Modules
{

    public enum TLogType
    {
        LogInfo,
        LogErr,
        LogWarn
    }

    public delegate void WriteLogEvent(TLogType LogType, string msg);

    /// <summary>
    /// 日志记录类
    /// </summary>
    public class LogHelper
    {
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        private static readonly log4net.ILog logRun = log4net.LogManager.GetLogger("logRun");

        public static WriteLogEvent LogEvent;
        

        public static void WriteLog(string info)
        {
#if DEBUG

            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
#endif
        }

        public static void WriteRunLog(string info)
        {
            if (logRun.IsInfoEnabled)
            {
                info.Replace("\r\n", "  ");

                logRun.Info(info);

                if (LogEvent != null)
                {
                    LogEvent(TLogType.LogInfo, info);
                }

            }
        }

        public static void WriteWarnLog(string info)
        {
            if (logRun.IsInfoEnabled)
            {
                info.Replace("\r\n", "  ");
                logRun.Warn(info);

                if (LogEvent != null)
                {
                    LogEvent( TLogType.LogWarn, info);
                }
            }
        }


        public static void WriteLog(string info, Exception se)
        {
            if (logRun.IsErrorEnabled)
            {
                info.Replace("\r\n", "  ");
                logRun.Error(info, se);

                if (LogEvent != null)
                {
                    LogEvent( TLogType.LogErr, info);
                }
            }
        }

        public static void WriteLogErr(string info)
        {
            if (logRun.IsErrorEnabled)
            {
                info.Replace("\r\n", "  ");
                logRun.Error(info);

                if (LogEvent != null)
                {
                    LogEvent(TLogType.LogErr, info);
                }
            }
        }

    }
}

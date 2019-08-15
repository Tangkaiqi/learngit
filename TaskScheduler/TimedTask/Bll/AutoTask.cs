using System;
using System.Collections;
using System.Collections.Generic;
using TimedTask.Model;

namespace TimedTask.Bll
{
    /// <summary>
    /// 定时任务类
    /// </summary>
    public class AutoTask : MSL.Tool.Data.DBAccessBase<Model.AutoTask>
    {
        public AutoTask()
            : base("AutoTask", "Id")
        {

        }
        public AutoTask(string connString)
            : base(connString, "AutoTask", "Id")
        {

        }
    }
}

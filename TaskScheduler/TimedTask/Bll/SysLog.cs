using System;
using System.Collections.Generic;
using System.Text;

namespace TimedTask.Bll
{
    /// <summary>
    /// 任务日志逻辑类
    /// </summary>
    public class SysLog : MSL.Tool.Data.DBAccessBase<Model.SysLog>
    {
        public SysLog()
            : base("SysLog", "Id")
        {

        }
        public SysLog(string connString)
            : base(connString, "SysLog", "Id")
        {

        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int Add(Model.SysLog model)
        {
            if (!Model.PM.SaveLog)
                return 0;

            return base.Add(model);
        }

        /// <summary>
        /// 删除历史日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void DeleteHistory()
        {
            base.Delete(" RunType IN('2','5') AND CreateDate<datetime('" + DateTime.Now.AddDays(-3) + "') ");//日 一次 3天前
            base.Delete(" RunType IN('1') AND CreateDate<datetime('" + DateTime.Now.AddMonths(-3) + "') ");//月 3月前
            base.Delete(" RunType IN('3','4') AND CreateDate<datetime('" + DateTime.Now.AddDays(-3) + "') ");//小时、分钟 2天前
        }
    }
}

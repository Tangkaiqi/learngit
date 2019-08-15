using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimedTask.Bll
{
    /// <summary>
    /// 系统类别 逻辑类
    /// </summary>
    public class TypeList : MSL.Tool.Data.DBAccessBase<Model.TypeList>
    {
        public TypeList()
            : base("TypeList", "Id")
        {

        }
        public TypeList(string connString)
            : base(connString, "TypeList", "Id")
        {

        }
    }
}

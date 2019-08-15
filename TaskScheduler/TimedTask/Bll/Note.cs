using System;
using System.Collections.Generic;
using System.Text;

namespace TimedTask.Bll
{
    /// <summary>
    /// 记事
    /// </summary>
    public class Note : MSL.Tool.Data.DBAccessBase<Model.Note>
    {
        public Note()
            : base("Note", "Id")
        {

        }
        public Note(string connString)
            : base(connString, "Note", "Id")
        {

        }
    }
}

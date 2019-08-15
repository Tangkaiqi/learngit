using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimedTask.Model
{
    public class Area
    {
        public int ID { get; set; }
        public int ZoneID { get; set; }
        public string AreaCode { get; set; }
        public string Name { get; set; }
    }
    public class Zone : ModelBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                this._id = value;
                this.RaisePropertyChanged("ID");
            }
        }
        public string Name { get; set; }
    }
}

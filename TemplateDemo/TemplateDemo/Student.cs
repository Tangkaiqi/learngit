using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateDemo
{
    public class Student : INotifyPropertyChanged
    {
        public string ID { get { return Guid.NewGuid().ToString(); } }

        public string Name { get; set; }

        public int Age { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}

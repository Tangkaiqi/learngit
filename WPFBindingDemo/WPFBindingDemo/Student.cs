using System.ComponentModel;

namespace WPFBindingDemo
{
    public class Student:INotifyPropertyChanged
    {
        private int m_ID;
        private string m_StudentName;
        private double m_Score;
       
        public int ID
        {
            get { return m_ID; }
            set 
            {
                if (value != m_ID)
                {
                    m_ID = value;
                    Notify("ID");
                }
            }
        }

        public string StudentName
        {
            get { return m_StudentName; }
            set
            {
                if (value != m_StudentName)
                {
                    m_StudentName = value;
                    Notify("StudentName");
                }
            }
        }

        public double Score
        {
            get { return m_Score; }
            set 
            {
                if (value != m_Score)
                {
                    m_Score = value;
                    Notify("Score");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

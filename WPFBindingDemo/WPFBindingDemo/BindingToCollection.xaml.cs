using System.Windows;

namespace WPFBindingDemo
{
    /// <summary>
    /// BindingToCollection.xaml 的交互逻辑
    /// </summary>
    public partial class BindingToCollection : Window
    {
        private Student m_student;
        public BindingToCollection()
        {
            InitializeComponent();
            m_student = new Student() { ID = 1, StudentName = "LearningHard", Score = 60 };
            // 设置Window对象的DataContext属性
            this.DataContext = m_student ;
        }

        private void ChangeScore_Click(object sender, RoutedEventArgs e)
        {
            m_student.Score = 90;
        }

        private void changeName_Click_1(object sender, RoutedEventArgs e)
        {
            m_student.StudentName = "Learning";
        }
    }
}

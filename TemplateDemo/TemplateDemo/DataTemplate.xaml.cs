using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TemplateDemo
{
    /// <summary>
    /// DataTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class DataTemplate : Window
    {
        ObservableCollection<Student> persons = new ObservableCollection<Student>() 
        { 
            new Student() { Name ="LearningHard", Age=25},
            new Student() { Name ="HelloWorld", Age=22}
        };
        public DataTemplate()
        {
            InitializeComponent();

            lstPerson.ItemsSource = persons;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Com.First
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CompoentConfig.AppContext.PermissionInfo.Equals("user",StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this.Name);
            }
            else
            {
                MessageBox.Show("对不起，" + CompoentConfig.AppContext.SessionUserInfo + "您的权限角色是" + CompoentConfig.AppContext.PermissionInfo + "，而该按钮只有user权限才能访问！");
            }
        }
    }
}

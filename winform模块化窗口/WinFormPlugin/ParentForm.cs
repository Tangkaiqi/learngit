using PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormPlugin
{
    public partial class ParentForm : Form
    {
        private bool startMove = false;

        public ParentForm()
        {
            InitializeComponent();

            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseUp += panel1_MouseUp;
            panel1.MouseMove += panel1_MouseMove;

            panel1.Top = menuStrip1.Height;
            panel1.Left = treeView1.Left + treeView1.Width;
            panel1.Height = panel1.Parent.Height;

        }

        void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (startMove)
            {
                panel1.Left += e.X;
            }
        }

        void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (startMove)
            {
                panel1.Left += e.X;
                startMove = false;
                this.treeView1.Width = panel1.Left;
            }
        }

        void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            startMove = true;
        }

        private void ParentForm_Load(object sender, EventArgs e)
        {

            AppContext.Current = new AppContext("文俊插件示例程序", "V16.3.26.1", "admin", "administrator", this);
            AppContext.Current.AppCache["loginDatetime"] = DateTime.Now;
            AppContext.Current.AppCache["baseDir"] = AppDomain.CurrentDomain.BaseDirectory;
            AppContext.Current.AppFormTypes = new Dictionary<string, Type>();
            LoadComponents();
            LoadMenuNodes();
        }

        private void LoadComponents()
        {
            string path = AppContext.Current.AppCache["baseDir"] + "com\\";
            Type targetFormType = typeof(Form);
            foreach (string filePath in Directory.GetFiles(path, "*.dll"))
            {
                var asy = Assembly.LoadFile(filePath);
                var configType = asy.GetTypes().FirstOrDefault(t => t.GetInterface("ICompoentConfig") != null);
                if (configType != null)
                {
                    ICompoent compoent=null;
                    var config = (ICompoentConfig)Activator.CreateInstance(configType);
                    config.CompoentRegister(AppContext.Current,out compoent);
                    if (compoent != null)
                    {
                        foreach (Type formType in compoent.FormTypes)
                        {
                            if (targetFormType.IsAssignableFrom(formType))
                            {
                                AppContext.Current.AppFormTypes.Add(formType.FullName, formType);
                            }
                        }
                    }
                }
            }
        }


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count <= 0)//当非父节点（即：实际的功能节点）
            {
                //ShowChildForm<ChildForm>();
                ShowChildForm(e.Node.Tag as Type);
            }
        }

        private void ParentForm_Resize(object sender, EventArgs e)
        {
            panel1.Height = panel1.Parent.Height;
        }

        private void LoadMenuNodes() //实现情况应该是从数据库及用户权限来进行动态创建菜单项
        {
            this.treeView1.Nodes.Clear();
            var root = this.treeView1.Nodes.Add("Root");
            //for (int i = 1; i <= 10; i++)
            //{
            //    var section = root.Nodes.Add("Section-" + i);
            //    int maxNodes = new Random(i).Next(1, 10);
            //    for (int n = 1; n <= maxNodes; n++)
            //    {
            //        section.Nodes.Add(string.Format("Level-{0}-{1}", i, n));
            //    }
            //}

            foreach (var formType in AppContext.Current.AppFormTypes)
            {
                var node = new TreeNode(formType.Key) { Tag = formType.Value };
                root.Nodes.Add(node);
            }
        }



        private void ShowChildForm<TForm>() where TForm : Form, new()
        {
            Form childForm = new TForm();
            childForm.MdiParent = this;
            childForm.Name = "ChildForm - " + DateTime.Now.Millisecond.ToString();
            childForm.Text = childForm.Name;
            childForm.Show();
        }

        private void ShowChildForm(Type formType)
        {
            var childForm= Application.OpenForms.Cast<Form>().SingleOrDefault(f=>f.GetType()==formType);
            if (childForm == null)
            {
                childForm = AppContext.Current.CreatePlugInForm(formType);  //(Form)Activator.CreateInstance(formType);

                childForm.MdiParent = this;
                childForm.Name = "ChildForm - " + DateTime.Now.Millisecond.ToString();
                childForm.Text = childForm.Name;
                childForm.Show();
            }
            else
            {
                childForm.BringToFront();
                childForm.Activate();
            }

        }
    }
}

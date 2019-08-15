namespace DXApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            this.tileNavPane1 = new DevExpress.XtraBars.Navigation.TileNavPane();
            this.navButton2 = new DevExpress.XtraBars.Navigation.NavButton();
            this.navButton5 = new DevExpress.XtraBars.Navigation.NavButton();
            this.navButton6 = new DevExpress.XtraBars.Navigation.NavButton();
            this.navButton7 = new DevExpress.XtraBars.Navigation.NavButton();
            this.navButton8 = new DevExpress.XtraBars.Navigation.NavButton();
            this.navButton3 = new DevExpress.XtraBars.Navigation.NavButton();
            this.navButton4 = new DevExpress.XtraBars.Navigation.NavButton();
            this.xitongwh = new DevExpress.XtraBars.Navigation.TileNavItem();
            this.tileNavItem1 = new DevExpress.XtraBars.Navigation.TileNavItem();
            this.tileNavItem2 = new DevExpress.XtraBars.Navigation.TileNavItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPane1 = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            ((System.ComponentModel.ISupportInitialize)(this.tileNavPane1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).BeginInit();
            this.tabPane1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tileNavPane1
            // 
            this.tileNavPane1.Buttons.Add(this.navButton2);
            this.tileNavPane1.Buttons.Add(this.navButton5);
            this.tileNavPane1.Buttons.Add(this.navButton6);
            this.tileNavPane1.Buttons.Add(this.navButton7);
            this.tileNavPane1.Buttons.Add(this.navButton8);
            this.tileNavPane1.Buttons.Add(this.navButton3);
            this.tileNavPane1.Buttons.Add(this.navButton4);
            // 
            // tileNavCategory1
            // 
            this.tileNavPane1.DefaultCategory.Items.AddRange(new DevExpress.XtraBars.Navigation.TileNavItem[] {
            this.xitongwh,
            this.tileNavItem1,
            this.tileNavItem2});
            this.tileNavPane1.DefaultCategory.Name = "tileNavCategory1";
            this.tileNavPane1.DefaultCategory.OwnerCollection = null;
            // 
            // 
            // 
            this.tileNavPane1.DefaultCategory.Tile.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.tileNavPane1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tileNavPane1.Location = new System.Drawing.Point(0, 0);
            this.tileNavPane1.Name = "tileNavPane1";
            this.tileNavPane1.Size = new System.Drawing.Size(791, 48);
            this.tileNavPane1.TabIndex = 1;
            this.tileNavPane1.Text = "tileNavPane1";
            // 
            // navButton2
            // 
            this.navButton2.Caption = "设置";
            this.navButton2.IsMain = true;
            this.navButton2.Name = "navButton2";
            this.navButton2.Tag = "1";
            this.navButton2.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.navButton2_ElementClick);
            // 
            // navButton5
            // 
            this.navButton5.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.navButton5.Caption = "任务提交";
            this.navButton5.Name = "navButton5";
            // 
            // navButton6
            // 
            this.navButton6.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.navButton6.Caption = "设备自检";
            this.navButton6.Name = "navButton6";
            // 
            // navButton7
            // 
            this.navButton7.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.navButton7.Caption = "重置针盒";
            this.navButton7.Name = "navButton7";
            // 
            // navButton8
            // 
            this.navButton8.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.navButton8.Caption = "项目标曲";
            this.navButton8.Name = "navButton8";
            // 
            // navButton3
            // 
            this.navButton3.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.navButton3.Caption = "底物探测";
            this.navButton3.Name = "navButton3";
            // 
            // navButton4
            // 
            this.navButton4.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.navButton4.Caption = "退出系统";
            this.navButton4.Name = "navButton4";
            this.navButton4.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.navButton4_ElementClick);
            // 
            // xitongwh
            // 
            this.xitongwh.Caption = "系统维护";
            this.xitongwh.Name = "xitongwh";
            this.xitongwh.OwnerCollection = this.tileNavPane1.DefaultCategory.Items;
            this.xitongwh.Padding = new System.Windows.Forms.Padding(10);
            // 
            // 
            // 
            this.xitongwh.Tile.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.Text = "系统维护";
            this.xitongwh.Tile.Elements.Add(tileItemElement1);
            this.xitongwh.Tile.Name = "系统维护";
            // 
            // tileNavItem1
            // 
            this.tileNavItem1.Caption = "用户设置";
            this.tileNavItem1.Name = "tileNavItem1";
            this.tileNavItem1.OwnerCollection = this.tileNavPane1.DefaultCategory.Items;
            // 
            // 
            // 
            this.tileNavItem1.Tile.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.Text = "用户设置";
            this.tileNavItem1.Tile.Elements.Add(tileItemElement2);
            this.tileNavItem1.Tile.Name = "tileBarItem1";
            this.tileNavItem1.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.tileNavItem1_ElementClick);
            // 
            // tileNavItem2
            // 
            this.tileNavItem2.Caption = "系统设置";
            this.tileNavItem2.Name = "tileNavItem2";
            this.tileNavItem2.OwnerCollection = this.tileNavPane1.DefaultCategory.Items;
            // 
            // 
            // 
            this.tileNavItem2.Tile.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement3.Text = "系统设置";
            this.tileNavItem2.Tile.Elements.Add(tileItemElement3);
            this.tileNavItem2.Tile.Name = "tileBarItem2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabPane1);
            this.panel1.Location = new System.Drawing.Point(0, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(791, 504);
            this.panel1.TabIndex = 2;
            // 
            // tabPane1
            // 
            this.tabPane1.AllowCollapse = DevExpress.Utils.DefaultBoolean.Default;
            this.tabPane1.Controls.Add(this.tabNavigationPage1);
            this.tabPane1.Controls.Add(this.tabNavigationPage2);
            this.tabPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPane1.Location = new System.Drawing.Point(0, 0);
            this.tabPane1.Name = "tabPane1";
            this.tabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1,
            this.tabNavigationPage2});
            this.tabPane1.RegularSize = new System.Drawing.Size(791, 504);
            this.tabPane1.SelectedPage = this.tabNavigationPage2;
            this.tabPane1.Size = new System.Drawing.Size(791, 504);
            this.tabPane1.TabIndex = 0;
            this.tabPane1.Text = "tabPane1";
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.Caption = "tabNavigationPage1";
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Size = new System.Drawing.Size(300, 122);
            // 
            // tabNavigationPage2
            // 
            this.tabNavigationPage2.Caption = "tabNavigationPage2";
            this.tabNavigationPage2.Name = "tabNavigationPage2";
            this.tabNavigationPage2.Size = new System.Drawing.Size(791, 476);
            // 
            // Form1
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 850);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tileNavPane1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tileNavPane1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).EndInit();
            this.tabPane1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Navigation.TileNavPane tileNavPane1;
        private DevExpress.XtraBars.Navigation.NavButton navButton2;
        private DevExpress.XtraBars.Navigation.NavButton navButton3;
        private DevExpress.XtraBars.Navigation.NavButton navButton4;
        private DevExpress.XtraBars.Navigation.TileNavItem tileNavItem1;
        private DevExpress.XtraBars.Navigation.TileNavItem tileNavItem2;
        private DevExpress.XtraBars.Navigation.NavButton navButton5;
        private DevExpress.XtraBars.Navigation.NavButton navButton6;
        private DevExpress.XtraBars.Navigation.NavButton navButton7;
        private DevExpress.XtraBars.Navigation.NavButton navButton8;
        private DevExpress.XtraBars.Navigation.TileNavItem xitongwh;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraBars.Navigation.TabPane tabPane1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
    }
}


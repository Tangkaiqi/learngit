namespace TestDemo
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabExperimentTask = new System.Windows.Forms.TabPage();
            this.btnNewTask = new System.Windows.Forms.Button();
            this.tabExperimentHistory = new System.Windows.Forms.TabPage();
            this.tabSetUp = new System.Windows.Forms.TabPage();
            this.tabHelp = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabExperimentTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabExperimentTask);
            this.tabControl1.Controls.Add(this.tabExperimentHistory);
            this.tabControl1.Controls.Add(this.tabSetUp);
            this.tabControl1.Controls.Add(this.tabHelp);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold);
            this.tabControl1.Location = new System.Drawing.Point(0, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1168, 617);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            this.tabControl1.Tag = "0";
            // 
            // tabExperimentTask
            // 
            this.tabExperimentTask.BackColor = System.Drawing.Color.Transparent;
            this.tabExperimentTask.Controls.Add(this.btnNewTask);
            this.tabExperimentTask.Font = new System.Drawing.Font("宋体", 10F);
            this.tabExperimentTask.Location = new System.Drawing.Point(4, 28);
            this.tabExperimentTask.Name = "tabExperimentTask";
            this.tabExperimentTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabExperimentTask.Size = new System.Drawing.Size(1160, 585);
            this.tabExperimentTask.TabIndex = 0;
            this.tabExperimentTask.Tag = "0";
            this.tabExperimentTask.Text = "实验任务";
            // 
            // btnNewTask
            // 
            this.btnNewTask.FlatAppearance.BorderSize = 0;
            this.btnNewTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnNewTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnNewTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewTask.Location = new System.Drawing.Point(6, 6);
            this.btnNewTask.Name = "btnNewTask";
            this.btnNewTask.Size = new System.Drawing.Size(136, 37);
            this.btnNewTask.TabIndex = 1;
            this.btnNewTask.Text = "新建任务";
            this.btnNewTask.UseVisualStyleBackColor = true;
            this.btnNewTask.Click += new System.EventHandler(this.btnNewTask_Click);
            // 
            // tabExperimentHistory
            // 
            this.tabExperimentHistory.BackColor = System.Drawing.Color.Transparent;
            this.tabExperimentHistory.Font = new System.Drawing.Font("宋体", 10F);
            this.tabExperimentHistory.Location = new System.Drawing.Point(4, 28);
            this.tabExperimentHistory.Name = "tabExperimentHistory";
            this.tabExperimentHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabExperimentHistory.Size = new System.Drawing.Size(1160, 585);
            this.tabExperimentHistory.TabIndex = 1;
            this.tabExperimentHistory.Tag = "1";
            this.tabExperimentHistory.Text = "实验历史";
            // 
            // tabSetUp
            // 
            this.tabSetUp.BackColor = System.Drawing.Color.Transparent;
            this.tabSetUp.Font = new System.Drawing.Font("宋体", 10F);
            this.tabSetUp.Location = new System.Drawing.Point(4, 28);
            this.tabSetUp.Name = "tabSetUp";
            this.tabSetUp.Size = new System.Drawing.Size(1160, 585);
            this.tabSetUp.TabIndex = 2;
            this.tabSetUp.Tag = "2";
            this.tabSetUp.Text = "系统设置";
            // 
            // tabHelp
            // 
            this.tabHelp.BackColor = System.Drawing.Color.Transparent;
            this.tabHelp.Font = new System.Drawing.Font("宋体", 10F);
            this.tabHelp.Location = new System.Drawing.Point(4, 28);
            this.tabHelp.Name = "tabHelp";
            this.tabHelp.Size = new System.Drawing.Size(1160, 585);
            this.tabHelp.TabIndex = 3;
            this.tabHelp.Tag = "3";
            this.tabHelp.Text = "系统帮助";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 699);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "测试系统";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabExperimentTask.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabExperimentTask;
        private System.Windows.Forms.TabPage tabExperimentHistory;
        private System.Windows.Forms.TabPage tabSetUp;
        private System.Windows.Forms.TabPage tabHelp;
        private System.Windows.Forms.Button btnNewTask;
    }
}
namespace TestDemo
{
    partial class Gridform
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
            this.btnYes = new System.Windows.Forms.Button();
            this.gridCtrl1 = new TestDemo.UserContrl.GridCtrl();
            this.btnEsc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(679, 12);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(96, 34);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "确定";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // gridCtrl1
            // 
            this.gridCtrl1.ClientRect = new System.Drawing.Rectangle(708, 344, 0, 0);
            this.gridCtrl1.ColCount = 9;
            this.gridCtrl1.GridSpacing = 8;
            this.gridCtrl1.Location = new System.Drawing.Point(6, 122);
            this.gridCtrl1.Name = "gridCtrl1";
            this.gridCtrl1.Rowcount = 13;
            this.gridCtrl1.Size = new System.Drawing.Size(774, 391);
            this.gridCtrl1.TabIndex = 2;
            this.gridCtrl1.Type = 0;
            // 
            // btnEsc
            // 
            this.btnEsc.Location = new System.Drawing.Point(679, 62);
            this.btnEsc.Name = "btnEsc";
            this.btnEsc.Size = new System.Drawing.Size(95, 34);
            this.btnEsc.TabIndex = 3;
            this.btnEsc.Text = "取消";
            this.btnEsc.UseVisualStyleBackColor = true;
            this.btnEsc.Click += new System.EventHandler(this.btnEsc_Click);
            // 
            // Gridform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 517);
            this.Controls.Add(this.btnEsc);
            this.Controls.Add(this.gridCtrl1);
            this.Controls.Add(this.btnYes);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Gridform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实验任务";
            this.Load += new System.EventHandler(this.Gridform_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnYes;
        private UserContrl.GridCtrl gridCtrl1;
        private System.Windows.Forms.Button btnEsc;
    }
}
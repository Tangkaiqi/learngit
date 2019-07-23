namespace TestDemo
{
    partial class GridForm2
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
            this.gridCtrl1 = new TestDemo.UserContrl.GridCtrl();
            this.SuspendLayout();
            // 
            // gridCtrl1
            // 
            this.gridCtrl1.Location = new System.Drawing.Point(46, 63);
            this.gridCtrl1.Name = "gridCtrl1";
            this.gridCtrl1.Size = new System.Drawing.Size(90, 57);
            this.gridCtrl1.TabIndex = 0;
            // 
            // GridForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 606);
            this.Controls.Add(this.gridCtrl1);
            this.Name = "GridForm2";
            this.Text = "GridForm2";
            this.ResumeLayout(false);

        }

        #endregion

        private UserContrl.GridCtrl gridCtrl1;
    }
}
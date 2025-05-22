namespace VSPremake.Resources
{
    partial class ExecutableLocationPicker
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Args = new System.Windows.Forms.Label();
            this.commandArgs = new System.Windows.Forms.TextBox();
            this.exec_label = new System.Windows.Forms.Label();
            this.fileLocation = new System.Windows.Forms.TextBox();
            this.pickerOpen = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Title = "Executable Picker";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.Args, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.commandArgs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.exec_label, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.fileLocation, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pickerOpen, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(675, 108);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Args
            // 
            this.Args.AutoSize = true;
            this.Args.Dock = System.Windows.Forms.DockStyle.Left;
            this.Args.Location = new System.Drawing.Point(3, 0);
            this.Args.Name = "Args";
            this.Args.Size = new System.Drawing.Size(87, 20);
            this.Args.TabIndex = 0;
            this.Args.Text = "Arguments";
            // 
            // commandArgs
            // 
            this.commandArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandArgs.Location = new System.Drawing.Point(3, 23);
            this.commandArgs.Name = "commandArgs";
            this.commandArgs.Size = new System.Drawing.Size(584, 26);
            this.commandArgs.TabIndex = 1;
            // 
            // exec_label
            // 
            this.exec_label.AutoSize = true;
            this.exec_label.Dock = System.Windows.Forms.DockStyle.Left;
            this.exec_label.Location = new System.Drawing.Point(3, 52);
            this.exec_label.Name = "exec_label";
            this.exec_label.Size = new System.Drawing.Size(153, 20);
            this.exec_label.TabIndex = 2;
            this.exec_label.Text = "Executable Location";
            // 
            // fileLocation
            // 
            this.fileLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileLocation.Location = new System.Drawing.Point(3, 75);
            this.fileLocation.Name = "fileLocation";
            this.fileLocation.Size = new System.Drawing.Size(584, 26);
            this.fileLocation.TabIndex = 3;
            this.fileLocation.DoubleClick += new System.EventHandler(this.pickerOpen_Click);
            // 
            // pickerOpen
            // 
            this.pickerOpen.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pickerOpen.AutoSize = true;
            this.pickerOpen.Location = new System.Drawing.Point(593, 75);
            this.pickerOpen.Name = "pickerOpen";
            this.pickerOpen.Size = new System.Drawing.Size(79, 30);
            this.pickerOpen.TabIndex = 4;
            this.pickerOpen.Text = "Browse";
            this.pickerOpen.UseVisualStyleBackColor = true;
            this.pickerOpen.Click += new System.EventHandler(this.pickerOpen_Click);
            // 
            // ExecutableLocationPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ExecutableLocationPicker";
            this.Size = new System.Drawing.Size(675, 108);
            this.Load += new System.EventHandler(this.ExecutableLocationPicker_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button pickerOpen;
        private System.Windows.Forms.TextBox fileLocation;
        private System.Windows.Forms.Label Args;
        private System.Windows.Forms.Label exec_label;
        private System.Windows.Forms.TextBox commandArgs;
    }
}

namespace DAFManager
{
    partial class DevConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevConsole));
            this.cmdText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.console = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdText
            // 
            this.cmdText.BackColor = System.Drawing.Color.Black;
            this.cmdText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cmdText.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdText.ForeColor = System.Drawing.Color.White;
            this.cmdText.Location = new System.Drawing.Point(20, 430);
            this.cmdText.Name = "cmdText";
            this.cmdText.Size = new System.Drawing.Size(789, 19);
            this.cmdText.TabIndex = 0;
            this.cmdText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmdText_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(4, 431);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = ">";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // console
            // 
            this.console.BackColor = System.Drawing.Color.Black;
            this.console.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.console.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.console.ForeColor = System.Drawing.Color.White;
            this.console.Location = new System.Drawing.Point(0, 0);
            this.console.Multiline = true;
            this.console.Name = "console";
            this.console.ReadOnly = true;
            this.console.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.console.Size = new System.Drawing.Size(810, 428);
            this.console.TabIndex = 2;
            this.console.Text = "Консоль разработчика. (C) VNCOMPANY 2019\r\n\r\n";
            // 
            // DevConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(811, 454);
            this.Controls.Add(this.console);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdText);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DevConsole";
            this.Text = "Консоль разработчика";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DevConsole_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cmdText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox console;
    }
}
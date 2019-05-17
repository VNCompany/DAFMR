namespace DAFManager
{
    partial class Priorities_Add
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Priorities_Add));
            this.label1 = new System.Windows.Forms.Label();
            this.n_key = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.n_label = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.n_key)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 14);
            this.label1.TabIndex = 15;
            this.label1.Text = "Ключ: ";
            // 
            // n_key
            // 
            this.n_key.Location = new System.Drawing.Point(96, 7);
            this.n_key.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.n_key.Name = "n_key";
            this.n_key.Size = new System.Drawing.Size(61, 22);
            this.n_key.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 14);
            this.label2.TabIndex = 13;
            this.label2.Text = "Название: ";
            // 
            // n_label
            // 
            this.n_label.Location = new System.Drawing.Point(96, 41);
            this.n_label.MaxLength = 32;
            this.n_label.Name = "n_label";
            this.n_label.Size = new System.Drawing.Size(308, 22);
            this.n_label.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 12;
            this.label3.Text = "Цвет:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(96, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(32, 18);
            this.panel1.TabIndex = 16;
            this.panel1.Click += new System.EventHandler(this.Panel1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(162, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Создать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Priorities_Add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(438, 174);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.n_label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.n_key);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Priorities_Add";
            this.Text = "Новый приоритет:";
            ((System.ComponentModel.ISupportInitialize)(this.n_key)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown n_key;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox n_label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
    }
}
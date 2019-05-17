namespace DAFManager
{
    partial class ResDebt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResDebt));
            this.label1 = new System.Windows.Forms.Label();
            this.amountSum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.createDate = new System.Windows.Forms.DateTimePicker();
            this.setHistory = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.desc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.amountSum)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Сумма выплаты: ";
            // 
            // amountSum
            // 
            this.amountSum.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.amountSum.Location = new System.Drawing.Point(144, 16);
            this.amountSum.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.amountSum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.amountSum.Name = "amountSum";
            this.amountSum.Size = new System.Drawing.Size(120, 23);
            this.amountSum.TabIndex = 1;
            this.amountSum.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Дата:";
            // 
            // createDate
            // 
            this.createDate.Location = new System.Drawing.Point(109, 15);
            this.createDate.Name = "createDate";
            this.createDate.Size = new System.Drawing.Size(200, 23);
            this.createDate.TabIndex = 3;
            // 
            // setHistory
            // 
            this.setHistory.AutoSize = true;
            this.setHistory.Checked = true;
            this.setHistory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setHistory.Location = new System.Drawing.Point(16, 68);
            this.setHistory.Name = "setHistory";
            this.setHistory.Size = new System.Drawing.Size(170, 20);
            this.setHistory.TabIndex = 4;
            this.setHistory.Text = "Сохранить в истории";
            this.setHistory.UseVisualStyleBackColor = true;
            this.setHistory.CheckedChanged += new System.EventHandler(this.SetHistory_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.desc);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.createDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(16, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(619, 152);
            this.panel1.TabIndex = 5;
            // 
            // desc
            // 
            this.desc.Location = new System.Drawing.Point(109, 55);
            this.desc.MaxLength = 200;
            this.desc.Multiline = true;
            this.desc.Name = "desc";
            this.desc.Size = new System.Drawing.Size(495, 84);
            this.desc.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Описание:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(560, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Ок";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // ResDebt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(647, 282);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.setHistory);
            this.Controls.Add(this.amountSum);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResDebt";
            this.Text = "Выплата долга";
            ((System.ComponentModel.ISupportInitialize)(this.amountSum)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown amountSum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker createDate;
        private System.Windows.Forms.CheckBox setHistory;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox desc;
        private System.Windows.Forms.Button button1;
    }
}
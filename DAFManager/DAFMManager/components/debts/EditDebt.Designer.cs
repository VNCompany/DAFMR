﻿namespace DAFManager
{
    partial class EditDebt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDebt));
            this.button1 = new System.Windows.Forms.Button();
            this.desc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.createDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.priority = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.amount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.debtorName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.amount)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(240, 330);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 29);
            this.button1.TabIndex = 17;
            this.button1.Text = "Изменить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // desc
            // 
            this.desc.Location = new System.Drawing.Point(150, 231);
            this.desc.MaxLength = 140;
            this.desc.Multiline = true;
            this.desc.Name = "desc";
            this.desc.Size = new System.Drawing.Size(320, 77);
            this.desc.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 14);
            this.label5.TabIndex = 19;
            this.label5.Text = "Краткое описание:";
            // 
            // createDate
            // 
            this.createDate.Location = new System.Drawing.Point(150, 169);
            this.createDate.Name = "createDate";
            this.createDate.Size = new System.Drawing.Size(220, 22);
            this.createDate.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 14);
            this.label4.TabIndex = 18;
            this.label4.Text = "Дата:";
            // 
            // priority
            // 
            this.priority.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.priority.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priority.FormattingEnabled = true;
            this.priority.Location = new System.Drawing.Point(150, 115);
            this.priority.Name = "priority";
            this.priority.Size = new System.Drawing.Size(220, 22);
            this.priority.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 14);
            this.label3.TabIndex = 15;
            this.label3.Text = "Приоритет:";
            // 
            // amount
            // 
            this.amount.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.amount.Location = new System.Drawing.Point(150, 62);
            this.amount.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(120, 22);
            this.amount.TabIndex = 11;
            this.amount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 14);
            this.label2.TabIndex = 12;
            this.label2.Text = "Сумма долга:";
            // 
            // debtorName
            // 
            this.debtorName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.debtorName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.debtorName.FormattingEnabled = true;
            this.debtorName.Location = new System.Drawing.Point(150, 16);
            this.debtorName.Name = "debtorName";
            this.debtorName.Size = new System.Drawing.Size(320, 22);
            this.debtorName.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "Имя задолжника:";
            // 
            // EditDebt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(593, 371);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.desc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.createDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.priority);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.debtorName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "EditDebt";
            this.Text = "Редактирование долга";
            ((System.ComponentModel.ISupportInitialize)(this.amount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox desc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker createDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox priority;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown amount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox debtorName;
        private System.Windows.Forms.Label label1;
    }
}
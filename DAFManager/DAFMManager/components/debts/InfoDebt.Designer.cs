namespace DAFManager
{
    partial class InfoDebt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoDebt));
            this.label1 = new System.Windows.Forms.Label();
            this.amount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.date = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.desc = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.history = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Сумма долга:";
            // 
            // amount
            // 
            this.amount.AutoSize = true;
            this.amount.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.amount.ForeColor = System.Drawing.Color.DarkCyan;
            this.amount.Location = new System.Drawing.Point(128, 19);
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(35, 16);
            this.amount.TabIndex = 1;
            this.amount.Text = "0 р.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Задолжник:";
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.username.Location = new System.Drawing.Point(128, 50);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(491, 16);
            this.username.TabIndex = 3;
            this.username.Text = "Иванов Иван Иванович";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Дата:";
            // 
            // date
            // 
            this.date.AutoSize = true;
            this.date.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.date.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.date.Location = new System.Drawing.Point(128, 78);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(158, 16);
            this.date.TabIndex = 5;
            this.date.Text = "2019.01.01 23:59:59";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "Описание:";
            // 
            // desc
            // 
            this.desc.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.desc.Location = new System.Drawing.Point(128, 111);
            this.desc.Name = "desc";
            this.desc.Size = new System.Drawing.Size(491, 73);
            this.desc.TabIndex = 7;
            this.desc.Text = "без описания";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 213);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "История:";
            // 
            // history
            // 
            this.history.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.history.Location = new System.Drawing.Point(25, 246);
            this.history.Name = "history";
            this.history.Size = new System.Drawing.Size(606, 177);
            this.history.TabIndex = 9;
            this.history.UseCompatibleStateImageBehavior = false;
            this.history.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Дата";
            this.columnHeader1.Width = 153;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Сумма";
            this.columnHeader2.Width = 114;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Комментарий";
            this.columnHeader3.Width = 334;
            // 
            // InfoDebt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(657, 435);
            this.Controls.Add(this.history);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.desc);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.date);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "InfoDebt";
            this.Text = "Информация о долге";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label amount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label desc;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListView history;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
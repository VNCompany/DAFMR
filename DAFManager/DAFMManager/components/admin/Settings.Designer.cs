namespace DAFManager
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.save = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.autoupdateCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.autoupdateInterval = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoupdateInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Изменить логин";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Изменить пароль";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(747, 319);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(739, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Вход";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(739, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Окно";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // save
            // 
            this.save.Enabled = false;
            this.save.Location = new System.Drawing.Point(633, 323);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(103, 23);
            this.save.TabIndex = 3;
            this.save.Text = "Сохранить";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.Save_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(22, 25);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(332, 18);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Закрывать программу при нажатии на крестик";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.autoupdateInterval);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.autoupdateCheck);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(739, 292);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Система";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // autoupdateCheck
            // 
            this.autoupdateCheck.AutoSize = true;
            this.autoupdateCheck.Checked = true;
            this.autoupdateCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoupdateCheck.Location = new System.Drawing.Point(22, 20);
            this.autoupdateCheck.Name = "autoupdateCheck";
            this.autoupdateCheck.Size = new System.Drawing.Size(201, 18);
            this.autoupdateCheck.TabIndex = 1;
            this.autoupdateCheck.Text = "Включить автообновление";
            this.autoupdateCheck.UseVisualStyleBackColor = true;
            this.autoupdateCheck.CheckedChanged += new System.EventHandler(this.AutoupdateCheck_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Интервал проверки обновлений:";
            // 
            // autoupdateInterval
            // 
            this.autoupdateInterval.Location = new System.Drawing.Point(247, 52);
            this.autoupdateInterval.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.autoupdateInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.autoupdateInterval.Name = "autoupdateInterval";
            this.autoupdateInterval.Size = new System.Drawing.Size(38, 22);
            this.autoupdateInterval.TabIndex = 3;
            this.autoupdateInterval.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.autoupdateInterval.ValueChanged += new System.EventHandler(this.AutoupdateInterval_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "ч";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(748, 351);
            this.Controls.Add(this.save);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.Text = "Настройки";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoupdateInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox autoupdateCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown autoupdateInterval;
        private System.Windows.Forms.Label label1;
    }
}
namespace dip_wf2
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.label1_nameServer = new System.Windows.Forms.Label();
            this.label2_type = new System.Windows.Forms.Label();
            this.label3_nameUser = new System.Windows.Forms.Label();
            this.label4_pass = new System.Windows.Forms.Label();
            this.label4_DB = new System.Windows.Forms.Label();
            this.comboBox1_type = new System.Windows.Forms.ComboBox();
            this.textBox1_nameServer = new System.Windows.Forms.TextBox();
            this.textBox2_nameUser = new System.Windows.Forms.TextBox();
            this.textBox3_pass = new System.Windows.Forms.TextBox();
            this.textBox4_DB = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1_nameServer
            // 
            this.label1_nameServer.AutoSize = true;
            this.label1_nameServer.Location = new System.Drawing.Point(12, 18);
            this.label1_nameServer.Name = "label1_nameServer";
            this.label1_nameServer.Size = new System.Drawing.Size(77, 13);
            this.label1_nameServer.TabIndex = 0;
            this.label1_nameServer.Text = "Имя сервера:";
            // 
            // label2_type
            // 
            this.label2_type.AutoSize = true;
            this.label2_type.Location = new System.Drawing.Point(12, 47);
            this.label2_type.Name = "label2_type";
            this.label2_type.Size = new System.Drawing.Size(128, 13);
            this.label2_type.TabIndex = 1;
            this.label2_type.Text = "Проверка подлинности:";
            // 
            // label3_nameUser
            // 
            this.label3_nameUser.AutoSize = true;
            this.label3_nameUser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3_nameUser.Location = new System.Drawing.Point(36, 75);
            this.label3_nameUser.Name = "label3_nameUser";
            this.label3_nameUser.Size = new System.Drawing.Size(106, 13);
            this.label3_nameUser.TabIndex = 2;
            this.label3_nameUser.Text = "Имя пользователя:";
            this.label3_nameUser.Click += new System.EventHandler(this.label3_nameUser_Click);
            // 
            // label4_pass
            // 
            this.label4_pass.AutoSize = true;
            this.label4_pass.Location = new System.Drawing.Point(36, 101);
            this.label4_pass.Name = "label4_pass";
            this.label4_pass.Size = new System.Drawing.Size(48, 13);
            this.label4_pass.TabIndex = 3;
            this.label4_pass.Text = "Пароль:";
            this.label4_pass.Click += new System.EventHandler(this.label4_pass_Click);
            // 
            // label4_DB
            // 
            this.label4_DB.AutoSize = true;
            this.label4_DB.Location = new System.Drawing.Point(12, 147);
            this.label4_DB.Name = "label4_DB";
            this.label4_DB.Size = new System.Drawing.Size(101, 13);
            this.label4_DB.TabIndex = 4;
            this.label4_DB.Text = "Имя базы данных:";
            // 
            // comboBox1_type
            // 
            this.comboBox1_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1_type.FormattingEnabled = true;
            this.comboBox1_type.Location = new System.Drawing.Point(158, 44);
            this.comboBox1_type.Name = "comboBox1_type";
            this.comboBox1_type.Size = new System.Drawing.Size(213, 21);
            this.comboBox1_type.TabIndex = 5;
            this.comboBox1_type.SelectedIndexChanged += new System.EventHandler(this.comboBox1_type_SelectedIndexChanged);
            // 
            // textBox1_nameServer
            // 
            this.textBox1_nameServer.Location = new System.Drawing.Point(158, 18);
            this.textBox1_nameServer.MaxLength = 20;
            this.textBox1_nameServer.Name = "textBox1_nameServer";
            this.textBox1_nameServer.Size = new System.Drawing.Size(213, 20);
            this.textBox1_nameServer.TabIndex = 6;
            this.textBox1_nameServer.TextChanged += new System.EventHandler(this.textBox1_nameServer_TextChanged);
            // 
            // textBox2_nameUser
            // 
            this.textBox2_nameUser.Location = new System.Drawing.Point(173, 72);
            this.textBox2_nameUser.MaxLength = 20;
            this.textBox2_nameUser.Name = "textBox2_nameUser";
            this.textBox2_nameUser.Size = new System.Drawing.Size(198, 20);
            this.textBox2_nameUser.TabIndex = 7;
            this.textBox2_nameUser.TextChanged += new System.EventHandler(this.textBox2_nameUser_TextChanged);
            // 
            // textBox3_pass
            // 
            this.textBox3_pass.Location = new System.Drawing.Point(173, 98);
            this.textBox3_pass.MaxLength = 30;
            this.textBox3_pass.Name = "textBox3_pass";
            this.textBox3_pass.PasswordChar = '*';
            this.textBox3_pass.Size = new System.Drawing.Size(198, 20);
            this.textBox3_pass.TabIndex = 8;
            this.textBox3_pass.TextChanged += new System.EventHandler(this.textBox3_pass_TextChanged);
            // 
            // textBox4_DB
            // 
            this.textBox4_DB.Location = new System.Drawing.Point(158, 140);
            this.textBox4_DB.MaxLength = 20;
            this.textBox4_DB.Name = "textBox4_DB";
            this.textBox4_DB.Size = new System.Drawing.Size(213, 20);
            this.textBox4_DB.TabIndex = 9;
            this.textBox4_DB.TextChanged += new System.EventHandler(this.textBox4_DB_TextChanged);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(146, 175);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 10;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 208);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.textBox4_DB);
            this.Controls.Add(this.textBox3_pass);
            this.Controls.Add(this.textBox2_nameUser);
            this.Controls.Add(this.textBox1_nameServer);
            this.Controls.Add(this.comboBox1_type);
            this.Controls.Add(this.label4_DB);
            this.Controls.Add(this.label4_pass);
            this.Controls.Add(this.label3_nameUser);
            this.Controls.Add(this.label2_type);
            this.Controls.Add(this.label1_nameServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.Text = "Настройка подкючения";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1_nameServer;
        private System.Windows.Forms.Label label2_type;
        private System.Windows.Forms.Label label3_nameUser;
        private System.Windows.Forms.Label label4_pass;
        private System.Windows.Forms.Label label4_DB;
        private System.Windows.Forms.TextBox textBox1_nameServer;
        private System.Windows.Forms.TextBox textBox2_nameUser;
        private System.Windows.Forms.TextBox textBox3_pass;
        private System.Windows.Forms.TextBox textBox4_DB;
        private System.Windows.Forms.ComboBox comboBox1_type;
        private System.Windows.Forms.Button OK;
    }
}
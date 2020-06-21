using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_wf2
{
    public partial class Form3 : Form
    {
        static string conectionstr = "";
        static string nameServer = "";
        static string nameUser = "";
        static string password = "";
        static string db__name = "";

        public Form3()
        {
            InitializeComponent();
            comboBox1_type.Items.AddRange(new string[] { "Проверка подлинности Windows", "Проверка подлинности SQL Server" });
            comboBox1_type.SelectedItem = "Проверка подлинности Windows";
            OK.Enabled = false;
            Icon = Icon.ExtractAssociatedIcon(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\icon.ico");
        }

        private void validation_OK()
        {
            if (comboBox1_type.SelectedItem == "Проверка подлинности Windows" && !String.IsNullOrWhiteSpace(nameServer) && !String.IsNullOrWhiteSpace(db__name))
            {
                OK.Enabled = true;
            }
            else if (comboBox1_type.SelectedItem == "Проверка подлинности SQL Server" && !String.IsNullOrWhiteSpace(textBox1_nameServer.Text)
                    && !String.IsNullOrWhiteSpace(textBox4_DB.Text) && !String.IsNullOrWhiteSpace(textBox2_nameUser.Text)
                    && !String.IsNullOrWhiteSpace(textBox3_pass.Text))
            {
                OK.Enabled = true;
            }
            else
            {
                OK.Enabled = false;
            }
        }

        private void textBox1_nameServer_TextChanged(object sender, EventArgs e)
        {
            nameServer = textBox1_nameServer.Text;
            validation_OK();
        }

        private void comboBox1_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1_type.SelectedItem == "Проверка подлинности Windows")
            {
                label3_nameUser.ForeColor = Color.Gray;
                label4_pass.ForeColor = Color.Gray;
                textBox2_nameUser.Enabled = false;
                textBox3_pass.Enabled = false;
            }
            else
            {
                OK.Enabled = false;
                label3_nameUser.ForeColor = Color.Black;
                label4_pass.ForeColor = Color.Black;
                textBox2_nameUser.Enabled = true;
                textBox3_pass.Enabled = true;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            var conectionstr_path = (Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\conectionstr.txt");
            System.IO.File.WriteAllText(conectionstr_path, String.Empty);

            if (comboBox1_type.SelectedItem == "Проверка подлинности Windows")
            {
                   conectionstr = String.Format(@"Data Source={0}; Initial Catalog={1}; Integrated Security={2};", nameServer, db__name, "True");
                   System.IO.File.WriteAllText(conectionstr_path, conectionstr);
            }
            else
            {
                conectionstr = String.Format(@"Data Source={0}; Initial Catalog={1}; Integrated Security={2}; User Id = {3}; Password = {4}; ", 
                                            nameServer, db__name, "False", nameUser, db__name);
                System.IO.File.WriteAllText(conectionstr_path, conectionstr);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox2_nameUser_TextChanged(object sender, EventArgs e)
        {
            nameUser = textBox2_nameUser.Text;
            validation_OK();
        }

        private void label3_nameUser_Click(object sender, EventArgs e){}

        private void textBox3_pass_TextChanged(object sender, EventArgs e)
        {
            password = textBox3_pass.Text;
            validation_OK();
        }

        private void label4_pass_Click(object sender, EventArgs e){}

        private void textBox4_DB_TextChanged(object sender, EventArgs e)
        {
            db__name = textBox4_DB.Text;
            validation_OK();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}

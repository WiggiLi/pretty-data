using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace dip_wf2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\icon.ico");
        }

        private void Form2_Load(object sender, EventArgs e){}

        public Form2(string f)
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            int width = 831;
            int height = 463;
            this.ClientSize = new System.Drawing.Size(width, height);

            textBox1.Text = f.Replace("\n", Environment.NewLine);
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {}

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e){}

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("Файл сохранен");
        }
    }
}

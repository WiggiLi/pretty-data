using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Data.SqlTypes;
using System.Collections;
using Saxon.Api;
using System.Xml.Serialization;
using System.Xml.XPath;
using JR.Utils.GUI.Forms;
using System.Xml.Xsl;
using System.Drawing;
using System.Net;

namespace dip_wf2
{
    public partial class Form1 : Form
    {
        private readonly DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
        private readonly DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();
        static int sort_flag = 0;
        static int translate_flag = 0;
        static int hide_flag = 0;
        static int logu_choose = 0;
        static int doc_choose = 0;
        static string dt1_choose = "";
        static string dt2_choose = "";
        static string conditions = "";
        static string connectionString = "";// String.Format(@"Data Source={0}; Initial Catalog={1}; Integrated Security={2};", "MSI", "veld2_L_small", "True");

        static int ROWS_COUNT = 0; //unknown
        static int pageSize = 18; // размер страницы
        static int curentNumber = 1; // текущая страница
        static int pageNumbers = 1;  // unknown

        static Boolean btnClearWasClicked1 = false;
        static Boolean btnClearWasClicked2 = false;
        static string text = "";
        public Form1()
        {
            InitializeComponent();
            constructDataGridView();
            //var conectionstr_path = (Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString(), @"..\..\..")) + @"\conectionstr.txt");
            Icon = Icon.ExtractAssociatedIcon(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\icon.ico");
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }
     

        private void constructDataGridView()
        {
            //ADD COLUMNS
            dataGridView1.ColumnCount = 6;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.Columns[0].Width = 40;

            addButtonColumn1(); //fourth column
            dataGridView1.Columns.Add(btn1);
            addButtonColumn2(); //fourth column
            dataGridView1.Columns.Add(btn2);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
        }

        private void addButtonColumn1() //properties for fourth column
        {
            btn1.HeaderText = "Документ";
            btn1.Name = "Documents";
            btn1.Text = "Показать";
            btn1.UseColumnTextForButtonValue = true;
        }

        private void addButtonColumn2() //properties for fourth column
        {
            btn2.HeaderText = "Изменения";
            btn2.Name = "Documents";
            btn2.Text = "Показать";
            btn2.UseColumnTextForButtonValue = true;
        }

        private void addRows() //fill datagridview
        {
            ArrayList row = new ArrayList();           
            ROWS_COUNT = myGetRowsCount();
            pageNumbers = (int)Math.Ceiling((decimal)ROWS_COUNT / pageSize);//ROWS_COUNT <= pageSize ? 1 : ROWS_COUNT / pageSize + (ROWS_COUNT % pageSize);

   
            for (int i = 0; i < (pageSize * curentNumber <= ROWS_COUNT ? pageSize : (curentNumber==1 ? ROWS_COUNT : ROWS_COUNT % (pageSize * (curentNumber - 1)))); i++) //(pageSize < ROWS_COUNT  ? pageSize : ROWS_COUNT)
            {
                string inf = FillToDataGridView(1, pageSize * (curentNumber - 1) + i, true);
                var items = inf.Split('#');

                row.Add(pageSize * (curentNumber - 1) + i + 1);
                row.Add(items[0]);
                row.Add(items[1]);
                row.Add(items[2]);
                row.Add(items[3]);
                row.Add(items[4]);
                dataGridView1.Rows.Add(row.ToArray());
                row.Clear();
            }        
        }

        private int myGetRowsCount() {         
            var inner_count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = String.Format("select count(*) from logu {0}", conditions);
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) // если есть данные
                    {
                        inner_count = Int32.Parse(reader.GetValue(0).ToString());
                    }
                    return inner_count;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message + ". Строка подключения не задана или некоректна");
                return 0;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) // handle clicks in button column
        {         
            if (e.ColumnIndex == 6 && e.RowIndex!=-1 || e.ColumnIndex == 7 && e.RowIndex != -1)
            {
                Form2 frm2 = new Form2(FillToDataGridView(1, Convert.ToInt32((e.RowIndex)), false, e.ColumnIndex));
                frm2.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e) // bytton export all
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\ChangeLog .txt", FillToDataGridView(ROWS_COUNT, 1, false)); //first argumet can be changed
            MessageBox.Show("Файл сохранен", "");
        }

    
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// quantity - 1st how many rows select from table
        /// pageNum - number of row in table or from what row start select rows
        /// flag - one document for MessegeBox (true) || all documents for export button (false)
        public static string FillToDataGridView(int quantity, int pageNum, bool flag, int col = 0)
        {   
            int kol = -1; /// number of header in current document
            List<StringBuilder> sb = new List<StringBuilder>(); ///for all files
             
            for (int i = 0; i < quantity; i++)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = String.Format("select LOGU_id, dt, doc_id, rwd, docnazv, dxml, dxml3 from logu {0} ORDER BY (SELECT NULL) offset ( {1} * 1 )  rows fetch next 1 rows only",
                                                        conditions, (pageNum)); //,pageNum //+ (curentNumber-1) * pageSize
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read()) // построчно считываем данные
                            {
                                object logu = reader.GetValue(0); // LOGU_idT
                                object dt = reader.GetValue(1); // dt
                                object doc_id = reader.GetValue(2); // doc_id
                                object rwd = reader.GetValue(3); // doc_id
                                object docnazv = reader.GetValue(4); // doc_id
                                                                    // Console.WriteLine(doc_id.ToString());
                                if (flag)
                                {
                                    return (logu.ToString() + "#" + dt.ToString() + "#" + doc_id.ToString() + "#" + rwd.ToString() + "#" + docnazv.ToString()); //FIRST return - data for usual rows
                                }

                                object str;

                                if (col == 6)
                                {
                                    str = reader.GetValue(5); // xml document
                                } 
                                else
                                {
                                    str = reader.GetValue(6); // xml document
                                }

                                if (String.IsNullOrEmpty(str.ToString()))
                                { // check for null
                                    Console.WriteLine("NULL");
                                    return "Данных нет - NULL";
                                }
                                Console.WriteLine("STR "+str.ToString());
                                peformingXSLT(str); //--//

                                var temp = System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.txt", //, @"..\..\.."
                                            Encoding.GetEncoding("windows-1251")); //temp.GetType() = string //TEMP PROBLEM HERE

                                kol += 1;
                                sb.Add(new StringBuilder());
                                if (kol != 0)
                                {
                                    sb.Add(new StringBuilder("\n"));
                                    sb[kol].AppendLine("------------------------------------------------------------------");
                                }
                                sb[kol].AppendLine("LOGU_idT №" + logu.ToString() + "   dt: " + dt.ToString() + "   doc_id №" + doc_id.ToString() + "\n"); // state data  
                                sb[kol].AppendLine(temp + "\n"); // change data
                            }
                        }
                        reader.Close(); //  
                    } //close connection/ Process one row from table
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return "";
                }
            }
            foreach (var build in sb) { //convirting
               build.ToString();
            }
            return string.Join("\n", sb); //full document for export || one document for show button
        } //FillToDataGridView


        public static void peformingXSLT(object str) {
            var xslt_main = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\script7.xslt"); ///path for xslt forming text file

            byte[] byteArray = Encoding.UTF8.GetBytes(str.ToString()); //stream instead reading file 
            MemoryStream stream_in = new MemoryStream(byteArray);
            //byte[] storage = new byte[255];
           // MemoryStream stream_out = new MemoryStream(byteArray);//

            var output = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.txt"); //insted variable for output text //TEMP

            var processor = new Processor();            //*
            var compiler = processor.NewXsltCompiler(); //*
            var serializer = processor.NewSerializer(); //*
            // outStream = null;                       //*
            FileInfo input = null;                      //*

            if (translate_flag == 1 || hide_flag == 1)
            {
                var output_xml = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.xml"); //temp insted variable
                var xslt_translate = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\script11.xslt");

                var executable1 = compiler.Compile(new Uri(xslt_translate.FullName));
                var transformer1 = executable1.Load30();

                FileStream outStream2 = new FileStream(output_xml.ToString(), FileMode.Create, FileAccess.Write);
                serializer.SetOutputStream(outStream2); //serializer.SetOutputStream(stream_out);

                Dictionary<QName, XdmValue> arg1 = new Dictionary<QName, XdmValue>();
                arg1.Add(new QName("translate"), new XdmAtomicValue(translate_flag));
                arg1.Add(new QName("hide"), new XdmAtomicValue(hide_flag));
                transformer1.SetStylesheetParameters(arg1);

                transformer1.ApplyTemplates(stream_in, serializer);
                outStream2.Close();

                input = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.xml");
            }     

            var executable = compiler.Compile(new Uri(xslt_main.FullName));
            var transformer = executable.Load30();

            FileStream outStream = new FileStream(output.ToString(), FileMode.Create, FileAccess.Write);
            serializer.SetOutputStream(outStream); //serializer.SetOutputStream(outStream);

            Dictionary<QName, XdmValue> people = new Dictionary<QName, XdmValue>();
            people.Add(new QName("sort"), new XdmAtomicValue(sort_flag));
            transformer.SetStylesheetParameters(people);

            if (input != null)
            {
                using (var inputStream = input.OpenRead())
                {
                    transformer.ApplyTemplates(inputStream, serializer);
                    outStream.Close();
                }
            }
            else
            {
                transformer.ApplyTemplates(stream_in, serializer); //ApplyTemplates Transform
                outStream.Close(); 
                //serializer.Close();
                //StreamReader reader = new StreamReader(stream_out);
                ///text = reader.ReadToEnd();
                //Console.WriteLine(text);
            }
        }


        private void button2_Click(object sender, EventArgs e) // get data for gridview
        {
            var conectionstr_path = (Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\conectionstr.txt");

            if (File.Exists(conectionstr_path))
            {
                var text = System.IO.File.ReadAllText(conectionstr_path);
                Console.WriteLine(text);
                connectionString = text;
            
                curentNumber = 1;
                textBox3.Text = "1";

                conditions = "";
                Boolean check_condition = false;
                if (true)
                {
                    if (doc_choose != 0)
                    {
                        conditions += String.Concat(" doc_id = ", doc_choose.ToString());
                        check_condition = true;
                    }
                    if (logu_choose != 0)
                    {
                        conditions += String.Concat((check_condition ? " and " : "") + " LOGU_id = ", logu_choose.ToString());
                        check_condition = true;
                    }
                    if (!string.IsNullOrWhiteSpace(dt1_choose))
                        Console.WriteLine("not empty dt1_choose =" + dt1_choose+".");
                    {
                        conditions += String.Concat( (check_condition ? " and " : "") + " dt >= '", dt1_choose, "'");
                        check_condition = true;
                    }
                    if (!string.IsNullOrWhiteSpace(dt2_choose))
                    {
                        conditions += String.Concat((check_condition ? " and " : "") + " dt <= '", dt2_choose, "'");
                        check_condition = true;
                    }
                }
                if (check_condition)
                {
                    conditions = String.Concat(" where ", conditions);
                    Console.WriteLine("conidions " + conditions);
                }
                else
                {
                    Console.WriteLine("empty conidions");
                }

                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                addRows(); //fill datagridview
            
                label6.Text = String.Format("Количество записей: {0}\n страниц: {1}", ROWS_COUNT, Math.Ceiling((decimal)ROWS_COUNT / pageSize));
            }
            else
            {
                MessageBox.Show("Нет данных о сервере и базе данных для подключения");
            }
        }
        

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox2 = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            sort_flag = checkBox2.Checked == true ? 1 : 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) 
        {
            CheckBox checkBox1 = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            translate_flag = checkBox1.Checked == true ?  1 :  0;
        }


        private void nextButton_Click(object sender, EventArgs e)
        {
            if (curentNumber < pageNumbers ) //ROWS_COUNT
            {
                curentNumber++;

                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                addRows();

                textBox3.Text = curentNumber.ToString();
            }            
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            if(curentNumber > 1)
            {
                curentNumber--;

                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                addRows();

                //label5.Text = curentNumber.ToString();
                textBox3.Text = curentNumber.ToString();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox1 = (TextBox)sender; // приводим отправителя к элементу типа CheckBox
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (textBox1.Text.Length == 6 && Int32.TryParse(textBox1.Text, out doc_choose))
                {
                    textBox1.BackColor = Color.White;
                    Console.WriteLine("doc_choose " + doc_choose);
                }
                else
                {
                    Console.WriteLine("Wrong doc");
                    textBox1.BackColor = ColorTranslator.FromHtml("#FFB6C1");
                }
            }
            else
            {
                doc_choose = 0;
                Console.WriteLine("Empty doc");
                textBox1.BackColor = Color.White;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox2 = (TextBox)sender; // приводим отправителя к элементу типа CheckBox
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (textBox2.Text.Length == 6 && Int32.TryParse(textBox2.Text, out logu_choose))
                {
                    // Console.WriteLine(doc_choose);
                    textBox2.BackColor = Color.White;
                }
                else
                {
                    Console.WriteLine("Wrong logu");
                    textBox2.BackColor = ColorTranslator.FromHtml("#FFB6C1");
                }
            }
            else
            {
                logu_choose = 0;
                Console.WriteLine("Empty logu");
                textBox2.BackColor = Color.White;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker1 = (DateTimePicker)sender;
            if (btnClearWasClicked1)
            {
                //dt1_choose = "";
                //dateTimePicker1.CustomFormat = " ";
                //dateTimePicker1.Format = DateTimePickerFormat.Custom;
                //Console.WriteLine("data EMPTY////////// " + dt1_choose);
                btnClearWasClicked1 = false;
                //dateTimePicker1.Checked = false;
                //Console.WriteLine("after " + dateTimePicker1.Checked + " | " + btnClearWasClicked1);

            }
            else if(dateTimePicker2.Checked == true)
            {
                dateTimePicker1.CustomFormat = "";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dt1_choose = dateTimePicker1.Value.ToString("yyyyMMdd");
                Console.WriteLine("data ////////// " + dateTimePicker1.Value.ToString("yyyyMMdd"));
            }
           
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker2 = (DateTimePicker)sender;
            if (btnClearWasClicked2)
            {
                //dt1_choose = "";
                //dateTimePicker1.CustomFormat = " ";
                //dateTimePicker1.Format = DateTimePickerFormat.Custom;
                Console.WriteLine("data EMPTY////////// " + dt2_choose);
                btnClearWasClicked2 = false;
                //dateTimePicker1.Checked = false;
                //Console.WriteLine("after " + dateTimePicker1.Checked + " | " + btnClearWasClicked1);

            }
            else if (dateTimePicker2.Checked == true)
            {            
                dateTimePicker2.CustomFormat = "";
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dt2_choose = dateTimePicker2.Value.AddDays(1).ToString("yyyyMMdd");
                Console.WriteLine("data ////////// " + dateTimePicker2.Value.ToString("yyyyMMdd"));           
            }          
        }
 

        private void Clear_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(dt1_choose))
            {
                btnClearWasClicked1 = true;
                dt1_choose = "";
            }
            Console.WriteLine("before "+dateTimePicker1.Checked + " | " + btnClearWasClicked1);
            dateTimePicker1.CustomFormat = " ";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.Checked = false;
           // dateTimePicker1.Value = DateTime.Parse("");
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(dt2_choose))
            {
                btnClearWasClicked2 = true;
                dt2_choose = "";
            }
            dateTimePicker2.CustomFormat = " ";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int temp = 0;
            if (Int32.TryParse(textBox3.Text, out temp))
            {
                if (temp >= 1 && temp <= pageNumbers)
                {
                    curentNumber = temp;
                    textBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandler);
                    textBox3.BackColor = Color.White;
                }
                else
                {
                    Console.WriteLine("out page " + temp + ". While pages: " + pageNumbers);
                    textBox3.BackColor = ColorTranslator.FromHtml("#FFB6C1");
                }
            }
            else
            {
                Console.WriteLine("Wrong page");
                textBox3.BackColor = ColorTranslator.FromHtml("#FFB6C1");
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                addRows();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            
            Form3 newForm = new Form3();
            //newForm.Show();
            
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("OK");
            }
            else{
               // Console.WriteLine(newForm.TheValue);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void groupBox1_Enter(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox3 = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            hide_flag = checkBox3.Checked == true ? 1 : 0;
        }
    } // Form1

}

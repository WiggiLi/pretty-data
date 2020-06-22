using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using Saxon.Api;


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
        static string connectionString = ""; // information about DB and user for connecting

        static int ROWS_COUNT = 0; //count of all rows in DB
        static int pageSize = 18; //count of rows in one page
        static int curentNumber = 1; //current page
        static int pageNumbers = 1; //count of pages

        static Boolean btnClearWasClicked1 = false;
        static Boolean btnClearWasClicked2 = false;

        public Form1()
        {
            InitializeComponent();
            constructDataGridView();
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

            addButtonColumn1(); //7th column
            dataGridView1.Columns.Add(btn1);
            addButtonColumn2(); //8th column
            dataGridView1.Columns.Add(btn2);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
        }

        private void addButtonColumn1() //properties for 7th column
        {
            btn1.HeaderText = "Документ";
            btn1.Name = "Documents";
            btn1.Text = "Показать";
            btn1.UseColumnTextForButtonValue = true;
        }

        private void addButtonColumn2() //properties for 8th column
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
            pageNumbers = (int)Math.Ceiling((decimal)ROWS_COUNT / pageSize);

            for (int i = 0; i < (pageSize * curentNumber <= ROWS_COUNT ? pageSize : (curentNumber==1 ? ROWS_COUNT : ROWS_COUNT % (pageSize * (curentNumber - 1)))); i++) 
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

        private int myGetRowsCount() { // get quantity of rows in table        
            var inner_count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = String.Format("select count(*) from logu {0}", conditions);
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) 
                    {
                        inner_count = Int32.Parse(reader.GetValue(0).ToString());
                    }
                    return inner_count;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message + ".\n\n Строка подключения не задана или некоректна");
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


        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// quantity -  how many rows select from table
        /// pageNum - number of row in table or which  row to start select rows from
        /// flag - get data for rwos in table (not data for buttun column) (true) || get data for buttun column (false)
        /// col - what button was clicked
        public static string FillToDataGridView(int quantity, int pageNum, bool flag, int col = 0)
        {   
            int kol = -1; /// number of header in current document
            List<StringBuilder> sb = new List<StringBuilder>(); // container for all files
             
            for (int i = 0; i < quantity; i++)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = String.Format("select LOGU_id, dt, doc_id, rwd, docnazv, dxml, dxml3 from logu {0} ORDER BY (SELECT NULL) offset ( {1} * 1 )  rows fetch next 1 rows only",
                                                        conditions, (pageNum)); 
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows) 
                        {
                            while (reader.Read()) 
                            {
                                object logu = reader.GetValue(0); // LOGU_idT
                                object dt = reader.GetValue(1); // dt
                                object doc_id = reader.GetValue(2); // doc_id
                                object rwd = reader.GetValue(3); // doc_id
                                object docnazv = reader.GetValue(4); // doc_id
                                                                 
                                if (flag)
                                {
                                    return (logu.ToString() + "#" + dt.ToString() + "#" + doc_id.ToString() + "#" + rwd.ToString() + "#" + docnazv.ToString()); 
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
                                { 
                                    return "Данных нет - NULL";
                                }
                                peformingXSLT(str); //--//

                                var temp = System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.txt", 
                                            Encoding.GetEncoding("windows-1251")); 

                                kol += 1;
                                sb.Add(new StringBuilder());
                                if (kol != 0)
                                {
                                    sb.Add(new StringBuilder("\n"));
                                    sb[kol].AppendLine("------------------------------------------------------------------");
                                }
                                sb[kol].AppendLine("LOGU_idT №" + logu.ToString() + "   dt: " + dt.ToString() + "   doc_id №" + doc_id.ToString() + "\n");  
                                sb[kol].AppendLine(temp + "\n"); 
                            }
                        }
                        reader.Close(); //  
                    } //close connection // Process one row from table
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return "";
                }
            }
            foreach (var build in sb) { 
               build.ToString();
            }
            return string.Join("\n", sb); 
        } 


        public static void peformingXSLT(object str) {
            var xslt_main = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString(), @"..\..\..")) + @"\pretty.xslt"); ///path for xslt forming text file

            byte[] byteArray = Encoding.UTF8.GetBytes(str.ToString()); 
            MemoryStream stream_in = new MemoryStream(byteArray);

            var output = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.txt"); 

            var processor = new Processor();            //*
            var compiler = processor.NewXsltCompiler(); //*
            var serializer = processor.NewSerializer(); //*
            FileInfo input = null;                      //*

            if (translate_flag == 1 || hide_flag == 1)
            {
                var output_xml = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.xml"); //temp insted variable
                var xslt_translate = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString(), @"..\..\..")) + @"\translate_hide.xslt");

                var executable1 = compiler.Compile(new Uri(xslt_translate.FullName));
                var transformer1 = executable1.Load30();

                FileStream outStream2 = new FileStream(output_xml.ToString(), FileMode.Create, FileAccess.Write);
                serializer.SetOutputStream(outStream2); 

                Dictionary<QName, XdmValue> arg1 = new Dictionary<QName, XdmValue>(); // pass arguments to XSLT-script for translate
                arg1.Add(new QName("translate"), new XdmAtomicValue(translate_flag));
                Console.WriteLine(translate_flag+ " "+ hide_flag);
                arg1.Add(new QName("hide"), new XdmAtomicValue(hide_flag));
                transformer1.SetStylesheetParameters(arg1);

                transformer1.ApplyTemplates(stream_in, serializer);
                outStream2.Close();

                input = new FileInfo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\output.xml");
            }     

            var executable = compiler.Compile(new Uri(xslt_main.FullName));
            var transformer = executable.Load30();

            FileStream outStream = new FileStream(output.ToString(), FileMode.Create, FileAccess.Write);
            serializer.SetOutputStream(outStream); 

            Dictionary<QName, XdmValue> people = new Dictionary<QName, XdmValue>(); // pass arguments to XSLT-script for form output 
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
                transformer.ApplyTemplates(stream_in, serializer); 
                outStream.Close(); 
            }
        }



        private void button1_Click(object sender, EventArgs e) // bytton for exporting all documents
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\ChangeLog .txt", FillToDataGridView(ROWS_COUNT, 1, false));
            MessageBox.Show("Файл сохранен", "");
        }

        private void button2_Click(object sender, EventArgs e) // load data for gridview
        {
            var conectionstr_path = (Path.GetFullPath(Path.Combine(Environment.CurrentDirectory.ToString())) + @"\conectionstr.txt");

            if (File.Exists(conectionstr_path))
            {
                var text = System.IO.File.ReadAllText(conectionstr_path);
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

        private void button3_Click(object sender, EventArgs e) //setting for connecting string
        {
            Form3 newForm = new Form3();
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("OK");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox1 = (CheckBox)sender;
            translate_flag = checkBox1.Checked == true ? 1 : 0;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox2 = (CheckBox)sender; 
            sort_flag = checkBox2.Checked == true ? 1 : 0;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox3 = (CheckBox)sender;
            hide_flag = checkBox3.Checked == true ? 1 : 0;
        }

        private void nextButton_Click(object sender, EventArgs e) // > - navigation through pages
        {
            if (curentNumber < pageNumbers ) 
            {
                curentNumber++;
                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                addRows();
                textBox3.Text = curentNumber.ToString();
            }            
        }

        private void backButton_Click(object sender, EventArgs e) // < - navigation through pages
        {
            if(curentNumber > 1)
            {
                curentNumber--;
                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                addRows();
                textBox3.Text = curentNumber.ToString();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox1 = (TextBox)sender; 
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (textBox1.Text.Length == 6 && Int32.TryParse(textBox1.Text, out doc_choose))
                {
                    textBox1.BackColor = Color.White;
                }
                else
                {
                    textBox1.BackColor = ColorTranslator.FromHtml("#FFB6C1");
                }
            }
            else
            {
                doc_choose = 0;
                textBox1.BackColor = Color.White;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox2 = (TextBox)sender; 
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (textBox2.Text.Length == 6 && Int32.TryParse(textBox2.Text, out logu_choose))
                {
                    textBox2.BackColor = Color.White;
                }
                else
                {
                    textBox2.BackColor = ColorTranslator.FromHtml("#FFB6C1");
                }
            }
            else
            {
                logu_choose = 0;
                textBox2.BackColor = Color.White;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker1 = (DateTimePicker)sender;
            if (btnClearWasClicked1)
            {
                btnClearWasClicked1 = false;

            }
            else if(dateTimePicker2.Checked == true)
            {
                dateTimePicker1.CustomFormat = "";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dt1_choose = dateTimePicker1.Value.ToString("yyyyMMdd");
            }
           
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker2 = (DateTimePicker)sender;
            if (btnClearWasClicked2)
            {
                btnClearWasClicked2 = false;
            }
            else if (dateTimePicker2.Checked == true)
            {            
                dateTimePicker2.CustomFormat = "";
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dt2_choose = dateTimePicker2.Value.AddDays(1).ToString("yyyyMMdd");        
            }          
        }
 

        private void Clear_Click(object sender, EventArgs e) // clear data1
        {
            if (!string.IsNullOrWhiteSpace(dt1_choose))
            {
                btnClearWasClicked1 = true;
                dt1_choose = "";
            }
            dateTimePicker1.CustomFormat = " ";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
        }


        private void button4_Click(object sender, EventArgs e) // clear data2
        {
            if (!string.IsNullOrWhiteSpace(dt2_choose))
            {
                btnClearWasClicked2 = true;
                dt2_choose = "";
            }
            dateTimePicker2.CustomFormat = " ";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
        }

        private void textBox3_TextChanged(object sender, EventArgs e) //field with number of page
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
                    textBox3.BackColor = ColorTranslator.FromHtml("#FFB6C1");
                }
            }
            else
            {
                curentNumber = 1;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void groupBox1_Enter(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

     
    } 

}

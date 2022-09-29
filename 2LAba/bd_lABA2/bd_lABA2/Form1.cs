using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Data.SqlClient;

namespace bd_lABA2
{
    public partial class Form1 : Form
    {
        Image o;
        string filename;
        string filename1;

        //DataSet ds;
        //SQLiteDataAdapter adapter;
        //SqlCommandBuilder commandBuilder;
        //string connectionString = @"Data Source=LAba2.db; Version=3;";
        //string sql = "Select *  from [table] ";

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "files Images(*.jpg)|*.jpg|All files(*.*)|*.*";

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=LAba2.db; Version=3;"))

            {

                Connect.Open();

                string commandText1 = "Select id  from [table1] ";

                SQLiteCommand command = new SQLiteCommand(commandText1, Connect);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // Работает 
                {



                    string data = Convert.ToString(reader.GetValue(0));

                    if (!comboBox1.Equals(data))
                    {
                        comboBox1.Items.Add(data);
                    }
                    


                    
                }

                Connect.Close();

               
            }

        }

            private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;


            // получаем выбранный файл
            filename = openFileDialog1.FileName;
            string imgPath = filename; // изображение

            FileInfo _imgInfo = new FileInfo(imgPath);

            long _numBytes = _imgInfo.Length;

            FileStream _fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read); // читаем изображение

            BinaryReader _binReader = new BinaryReader(_fileStream);

            byte[] _imageBytes = _binReader.ReadBytes((int)_numBytes); // изображение в байтах



            string imgFormat = Path.GetExtension(imgPath).Replace(".", "").ToLower(); // запишем в переменную расширение изображения в нижнем регистре, не забыв удалить точку перед расширением, получим «png»

            string imgName = Path.GetFileName(imgPath).Replace(Path.GetExtension(imgPath), ""); // запишем в переменную имя файла, не забыв удалить расширение с точкой, получим «image-01»



            // записываем информацию в базу данных Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"usersdata.db"))

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=LAba2.db; Version=3;"))

            {
                Connect.Open();

                // в запросе есть переменные, они начинаются на @, обратите на это внимание

                string commandText = "INSERT INTO [table1] ([Picture]) VALUES(@image)";

                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);

                Command.Parameters.AddWithValue("@image", _imageBytes); // присваиваем переменной значение


                SQLiteDataReader reader1 = Command.ExecuteReader();
               // Command.ExecuteNonQuery();

                Connect.Close();



                Connect.Open();

                string commandText1 = "Select id  from [table1] ";

                SQLiteCommand command = new SQLiteCommand(commandText1, Connect);

                SQLiteDataReader reader = command.ExecuteReader();

                comboBox1.Items.Clear();
                
                while (reader.Read()) // Работает 
                {



                    string data = Convert.ToString(reader.GetValue(0));

                    if (!comboBox1.Equals(data))
                    {
                        comboBox1.Items.Add(data);
                    }
                    


                    
                }

                




                Connect.Close();



            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection Connect3 = new SQLiteConnection(@"Data Source=LAba2.db; Version=3;"))

            {
                Connect3.Open();

                // в запросе есть переменные, они начинаются на @, обратите на это внимание

                // string commandText = "DELETE FROM [table] ([Picture]) VALUES(@image)";
               // string commandText = "DELETE  [Picture]  WHERE id=" + comboBox1.Text +"";
                string commandText = "delete from [table1] where id=" + comboBox1.Text + "";
                SQLiteCommand Command3 = new SQLiteCommand(commandText, Connect3);

                


                Command3.ExecuteNonQuery();

                Connect3.Close();

                comboBox1.Items.Remove(comboBox1.Text);

            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.ToString() != null)
                textBox1.Text = comboBox1.Text;

            string connectionString = @"Data Source=LAba2.db;Initial Catalog=usersdb;Integrated Security=True";
            List<Image> images = new List<Image>();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=LAba2.db")) // в строке указывается к какой базе подключаемся
            {
                connection.Open();
                string sql = "Select Picture  from [table1]";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) // Работает 
                {

                    //byte data1 = Convert.ToByte((byte[])reader.GetValue(0));

                    string data = Convert.ToString(reader.GetValue(0));


                    //byte data12 = (byte)Convert.ToByte(data);
                    //long _numBytes1 = data.Length;

                    byte[] _imageBytes1 = (byte[])reader.GetValue(0); // изображение в байтах

                    Image img;
                    MemoryStream stream1 = new MemoryStream();//сюда
                    stream1 = new MemoryStream(_imageBytes1);
                    img = Image.FromStream(stream1);

                    pictureBox2.Image = img;

                    //textBox1.Text += Convert.ToString(data12) +"    ";
                    stream1.Dispose();
                }
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime r = DateTime.Now;
            string commandText = "INSERT INTO [table1] ([date]) VALUES(@r)";

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=LAba2.db")) // в строке указывается к какой базе подключаемся
            {
                connection.Open();
                // string sql = "Select Picture  from [table1]";
                SQLiteCommand command = new SQLiteCommand(commandText, connection);

                command.Parameters.AddWithValue("@r", r);

                SQLiteDataReader reader = command.ExecuteReader();
                //Console.WriteLine(DateTime.Now);
              //  MessageBox.Show("время добавил");
                connection.Close();

            }

            string r2 = comboBox2.Text;
            string commandText2 = "INSERT INTO [table1] ([bool]) VALUES(@bl)";
            bool bl= true;
            if (r2 == "Да")
            {
                bl = true;
            }
            else { bl = false; }
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=LAba2.db")) // в строке указывается к какой базе подключаемся
            {
                connection.Open();
                // string sql = "Select Picture  from [table1]";
                SQLiteCommand command = new SQLiteCommand(commandText2, connection);

                command.Parameters.AddWithValue("@bl", bl);

                SQLiteDataReader reader = command.ExecuteReader();
                //Console.WriteLine(DateTime.Now);
                MessageBox.Show("bool добавил");
                connection.Close();

            }

        }
    }
}


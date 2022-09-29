using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace sqlite
{
    public partial class Form1 : Form
    {
        private sqliteclass mydb = null;
        private string sCurDir = string.Empty;
        private string sPath = string.Empty;
        private string sSql = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sPath = Path.Combine(Application.StartupPath, "mybd.db");
            Text = sPath; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = @"CREATE TABLE if not exists [birthday]([id] INTEGER PRIMARY KEY AUTOINCREMENT,[FIO] TEXT NOT NULL,[bdate] datetime NOT NULL,[gretinyear] INTEGER DEFAULT 0);";
            //Пыьаемся создать таблицу
            mydb.iExecuteNonQuery(sPath, sSql, 0);
            sSql = @"insert into birthday (FIO,bdate,gretinyear) values('Александр Сергеевич Пушкин','1799-06-06',0);";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка проверки таблицы на запись, таблица или не создана или не прошла запись тестовой строки!";
                mydb = null;
                return;
            }
            sSql = "select * from  birthday";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                Text = "Ошибка проверки таблицы на чтение!";
                mydb = null;
                return;
            }
            Text = "";
            foreach (DataRow dr in datarows)
            {
                Text += dr["id"].ToString().Trim() + dr["FIO"].ToString().Trim() + dr["bdate"].ToString().Trim() + " ";
            }

            sSql = "delete from  birthday";
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка проверки таблицы на удаление записи!";
                mydb = null;
                return;
            }

            Text = "Таблица создана!";
            mydb = null;
            return;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = @"insert into birthday (FIO,bdate,gretinyear) values('Александр Сергеевич Пушкин','1799-06-06',0);";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка записи!";
            }
            mydb = null;
            Text = "Запись 1 добавлена!";
            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            //Ошибка в дате намеренная
            sSql = @"insert into birthday (FIO,bdate,gretinyear) values('Толстой Лев Николаевич','1928-08-28',0);";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка записи!";
            }
            mydb = null;
            Text = "Запись 2 добавлена!";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = "select * from  birthday";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            
            if (datarows == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            Text = "";

            dataGridView1.Rows.Clear();

            foreach (DataRow dr in datarows)
            {
                
                dataGridView1.Rows.Add(dr["id"], dr["FIO"]);
            }
                foreach (DataRow dr in datarows)
            {
                
                Text += dr["id"].ToString().Trim() + "  " + dr["FIO"].ToString().Trim() + "  " + dr["bdate"].ToString().Trim() + " ";
                textBox1.Text= dr["id"].ToString().Trim() + "  " + dr["FIO"].ToString().Trim() + "  " + dr["bdate"].ToString().Trim() + " ";
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = "delete from  birthday";
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка удаления записи!";
                mydb = null;
                return;
            }
            mydb = null;
            Text = "Записи удалены из БД!";
            return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = @"Update birthday set bdate='1828-08-28' where FIO like('%Толстой%');";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка обновления записи!";
                mydb = null;
                return;
            }
            mydb = null;
            Text = "Запись 2 исправлена!";
        }
      
    }
}

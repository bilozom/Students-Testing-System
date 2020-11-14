using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace test_app
{
    public partial class Testirovanie : Form
    {
        public Testirovanie()
        {
            InitializeComponent();
            connection = @"Data Source=ПК\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";
        }
        
        public string connection;
        public SqlDataAdapter sda;
        public string l;
        public string testname;
        int c = 0;
        int i;
        int otvet = 0;
        int answer;
        int w;
        int questnumber = 1;

        private void button1_Click(object sender, EventArgs e)
        {
            // Начало отсчёта
            timer1.Enabled = true;
            comboBox1.Items.Clear();

            update();

            DataSet ds1 = new DataSet();
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                sqlconnection.Open();
                // Вывод количества вопросов теста из бд
                string query = "select COUNT(question_number) as c from Question where test_number = '" + i + "'";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds1);
                label5.Text = "Баллы " + otvet + " / " + ds1.Tables[0].Rows[0]["c"].ToString();
                                
                label4.Text = "Вопрос " + questnumber + " / " + ds1.Tables[0].Rows[0]["c"].ToString();

                if (questnumber != Convert.ToInt32(ds1.Tables[0].Rows[0]["c"]))
                {
                    questnumber++;
                }
            }
            
            c++;
            if (c != 0)
            {
                button1.Text = "Далее";                
            }

            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlconnection = new SqlConnection(connection))
                {
                    sqlconnection.Open();
                    // Вывод вопросов теста из бд
                    string query = "SELECT Question.question_number as qnum, Question.question as quest, Question.option1 as o1, Question.option2 as o2, Question.option3 as o3, Question.answer as otvet " +
                        "FROM Test, Question WHERE Question.test_number = Test.test_number and Test.test_number = '" + i + "' " +
                        "and Question.question_number = '" + c + "'";
                    sda = new SqlDataAdapter(query, sqlconnection);
                    sda.Fill(ds);
                    // Добавление вопросов теста в comboBox
                    comboBox1.Items.Add(ds.Tables[0].Rows[0]["o1"].ToString());
                    comboBox1.Items.Add(ds.Tables[0].Rows[0]["o2"].ToString());
                    comboBox1.Items.Add(ds.Tables[0].Rows[0]["o3"].ToString());
                    label1.Text = testname;
                    label2.Text = ds.Tables[0].Rows[0]["quest"].ToString();
                    w = Convert.ToInt32(ds.Tables[0].Rows[0]["otvet"]);
                }
            }
            catch
            {
                button1.Enabled = false;

                int k = 0;
                DataSet ds = new DataSet();
                using (SqlConnection sqlconnection = new SqlConnection(connection))
                {
                    sqlconnection.Open();
                    string query = "SELECT  COUNT(Question.answer) as k FROM Test, Question WHERE Question.test_number = Test.test_number and Test.test_number = '" + i + "'";
                    sda = new SqlDataAdapter(query, sqlconnection);
                    sda.Fill(ds);
                    k = Convert.ToInt32(ds.Tables[0].Rows[0]["k"]);
                }
                rez = Math.Round((100.0 / k) * otvet);

                if (sck == 0)
                {
                    DataSet ds2 = new DataSet();
                    // Заполнение строки бд информацией после прохождения теста
                    using (SqlConnection sqlconnection = new SqlConnection(connection))
                    {
                        sqlconnection.Open();
                        string query = "SELECT ISNULL(MAX(History.history_number), 0) AS max FROM History";
                        sda = new SqlDataAdapter(query, sqlconnection);
                        sda.Fill(ds2);
                        int cc = Convert.ToInt32(ds2.Tables[0].Rows[0]["max"].ToString()) + 1;
                        query = "INSERT INTO [test].[dbo].[History]([history_number],[login],[test_number],[result] ,[score],[time] ,[date]) " + "VALUES('" + cc + "','" + l + "','" + i + "','" + otvet + "','" + rez + "','" + t.ToString() + "','" + Convert.ToString(DateTime.Today.Date.ToShortDateString()) + "')";
                        sda = new SqlDataAdapter(query, sqlconnection);
                        sda.Fill(ds2);
                        sqlconnection.Close();
                    }
                }
                sck++;

                Result result = new Result();
                result.res = this.rez.ToString();
                result.Show();
                timer1.Enabled = false;
            }
            comboBox1.Text = "";
        }
        
        public void update()
        {
            DataSet ds = new DataSet();
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                sqlconnection.Open();
                string query = "SELECT Test.test_number as tnum, name FROM Test WHERE Test.name= '" + testname + "'";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds);
                i = Convert.ToInt32(ds.Tables[0].Rows[0]["tnum"]);
            }
        }

        double rez;
        int t = 0;
        int m = 0;
        int s = 0;

        int sck = 0;

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            t++;
            s++;
            if (s > 59)
            {
                m++;
                s -= 60;
            }
            label3.Text = m + " : " + s;
            if (t == 600)
            {
                timer1.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            answer = comboBox1.SelectedIndex + 1;
            if (w == answer)
            {
                otvet++;
            }
        }
    }
}

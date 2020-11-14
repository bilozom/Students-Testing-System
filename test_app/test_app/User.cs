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
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
            connection = @"Data Source=ПК\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";
            update();
        }

        public string connection;
        public SqlDataAdapter sda;
        public string l;
        public string testname;

        private void button2_Click(object sender, EventArgs e)
        {
            // Закрытие формы
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                sqlconnection.Open();
                testname = comboBox1.SelectedValue.ToString();
                DataSet ds = new DataSet();
                // Вывод необходимой ифнормации о конкретном пользователе из бд по указанному тесту
                string query = "select Test.name as Название_теста, History.result as Результат, History.score as Количество_баллов, History.time as Время, History.date as Дата from History, Test where login = '" + l + "' and Test.test_number = History.test_number and Test.name= '" + testname + "'";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                sqlconnection.Close();
            }
        }

        // Метод вывода в comboBox навзания всех тестов
        public void update()
        {
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                DataSet ds1 = new DataSet();
                string query = "select Test.name as tid from test.dbo.Test";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds1);
                comboBox1.DataSource = ds1.Tables[0];
                comboBox1.DisplayMember = "tid";
                comboBox1.ValueMember = "tid";
            }
        }

        private void User_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                sqlconnection.Open();
                DataSet ds = new DataSet();
                // Вывод необходимой ифнормации о конкретном пользователе из бд
                string query = "select Test.name as Название_теста, History.result as Результат, History.score as Количество_баллов, History.time as Время, History.date as Дата from History, Test where login = '" + l + "' and Test.test_number = History.test_number";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                sqlconnection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Переход к форме Test
            Test test = new Test();
            test.l = this.l;
            test.Show();
        }
    }
}

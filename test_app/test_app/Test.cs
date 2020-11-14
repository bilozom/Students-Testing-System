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
    public partial class Test : Form
    {
        public Test()
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

        // Метод вывода в comboBox названия всех тестов
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

        private void button1_Click(object sender, EventArgs e)
        {
            testname = comboBox1.SelectedValue.ToString();
            Testirovanie testirovanie = new Testirovanie();
            testirovanie.l = this.l;
            testirovanie.testname = this.testname;
            testirovanie.Show();
        }
    }
}

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
using System.Data.OleDb;
using Word = Microsoft.Office.Interop.Word;

namespace test_app
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            connection = @"Data Source=ПК\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";
            update();
            update2();
        }

        public string connection;
        public SqlDataAdapter sda;

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        // Метод вывода в comboBox логина всех пользователей
        public void update()
        {
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                DataSet ds1 = new DataSet();
                string query = "select Users.login as uid from test.dbo.Users";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds1);
                comboBox1.DataSource = ds1.Tables[0];
                comboBox1.DisplayMember = "uid";
                comboBox1.ValueMember = "uid";

                comboBox6.DataSource = ds1.Tables[0];
                comboBox6.DisplayMember = "uid";
                comboBox6.ValueMember = "uid";                
            }
        }

        // Метод вывода в comboBox названия всех тестов
        public void update2()
        {
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                DataSet ds1 = new DataSet();
                string query = "select Test.name as tid from test.dbo.Test";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds1);
                comboBox5.DataSource = ds1.Tables[0];
                comboBox5.DisplayMember = "tid";
                comboBox5.ValueMember = "tid";

                comboBox7.DataSource = ds1.Tables[0];
                comboBox7.DisplayMember = "tid";
                comboBox7.ValueMember = "tid";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            label6.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            button3.Visible = true;

            DataSet ds = new DataSet();
            using (SqlConnection sqlconnection = new SqlConnection(connection))
            {
                sqlconnection.Open();
                // Получение информации о указанном пользователе
                string query = "SELECT Users.login as login, Users.password as password, Users.surename as surename, Users.name as name, Users.patronymic as patronymic FROM test.dbo.Users WHERE Users.login= '" + comboBox1.SelectedValue + "'";
                sda = new SqlDataAdapter(query, sqlconnection);
                sda.Fill(ds);
                // Заполнение текстовых полей значениями из базы данных
                textBox1.Text = ds.Tables[0].Rows[0]["login"].ToString();
                textBox2.Text = ds.Tables[0].Rows[0]["password"].ToString();
                textBox3.Text = ds.Tables[0].Rows[0]["surename"].ToString();
                textBox4.Text = ds.Tables[0].Rows[0]["name"].ToString();
                textBox5.Text = ds.Tables[0].Rows[0]["patronymic"].ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlconnection = new SqlConnection(connection))
                {
                    sqlconnection.Open();
                    // Изменение данных в бд
                    string query = "Update test.dbo.Users SET login = '" + textBox1.Text + "', password = '" + textBox2.Text + "', surename = '" + textBox3.Text + "', name = '" + textBox4.Text + "', patronymic = '" + textBox5.Text + "' WHERE Users.login= '" + comboBox1.SelectedValue + "'";
                    sda = new SqlDataAdapter(query, sqlconnection);
                    sda.Fill(ds);
                    MessageBox.Show("Операция совершена!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //C:\Users\Кирилл\Desktop\Dannye_dlya_importa.xlsx
                // Выбор excel файла
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + textBox7.Text + "; Extended Properties=\"Excel 12.0 Xml;HDR=YES;\";";
                OleDbConnection con = new OleDbConnection(constr);
                // Получение всех данных из указанной страницы  файла excel
                OleDbDataAdapter sda = new OleDbDataAdapter("Select " + "*" + " From [" + textBox6.Text + "$]", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        int i = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlconnection = new SqlConnection(connection))
                {
                    sqlconnection.Open();
                    string query = "SELECT ISNULL(MAX(Question.question_id), 0) AS max FROM Question";
                    sda = new SqlDataAdapter(query, sqlconnection);
                    sda.Fill(ds);
                    int c = Convert.ToInt32(ds.Tables[0].Rows[0]["max"].ToString()) + 1;
                    // Занесение данных в бд
                    while (i <= dataGridView1.Rows.Count - 1)
                    {
                        query = "IF NOT EXISTS(SELECT * FROM Test WHERE test_number = '" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and disciplin_number = '" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "' and name = '" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "')BEGIN INSERT INTO [dbo].[Test]([test_number],[disciplin_number],[name]) " +
                                "VALUES('" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'); END; ";
                        sda = new SqlDataAdapter(query, sqlconnection);
                        sda.Fill(ds);

                        query = "IF NOT EXISTS(SELECT * FROM Question WHERE question_number = '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "' and test_number = '" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "')BEGIN INSERT INTO [dbo].[Question]([question_id],[test_number],[question_number],[question],[option1],[option2],[option3],[answer]) " +
                                "VALUES('" + c + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[6].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[7].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[8].Value.ToString() + "'); END; ";
                        sda = new SqlDataAdapter(query, sqlconnection);
                        sda.Fill(ds);
                        c++;
                        i++;
                    }
                    MessageBox.Show("Операция совершена!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                if (dataGridView1.CurrentRow != null)
                {
                    MessageBox.Show("Операция совершена!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private readonly string TemplateFileName = @"C:\Users\Кирилл\Desktop\08.09\test.docx";

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection sqlconnection = new SqlConnection(connection))
                {
                    sqlconnection.Open();
                    string testname = comboBox5.SelectedValue.ToString();
                    string user = comboBox6.SelectedValue.ToString();
                    // Получение данных об указанном пользователе из бд
                    string query = "Select Users.surename as sname, Users.name as name, Users.patronymic as patr, Test.name as testname, History.score as res , History.date as date " +
                        "From Users,Test,History " +
                        "Where Test.test_number = History.test_number and History.login = Users.login and Users.login = '" + user + "' " +
                        "and Test.name = '" + testname + "' and History.result = (Select MAX(History.result) from History where login = '" + user + "') ";
                    sda = new SqlDataAdapter(query, sqlconnection);
                    sda.Fill(ds);
                    string sname = ds.Tables[0].Rows[0]["sname"].ToString();
                    string name = ds.Tables[0].Rows[0]["name"].ToString();
                    string patr = ds.Tables[0].Rows[0]["patr"].ToString();
                    string test = ds.Tables[0].Rows[0]["testname"].ToString();
                    string res = ds.Tables[0].Rows[0]["res"].ToString();
                    string date = ds.Tables[0].Rows[0]["date"].ToString();

                    // Создание word документа
                    var wordApp = new Word.Application();
                    wordApp.Visible = false;

                    var wordDocument = wordApp.Documents.Open(TemplateFileName);
                    // Вставка значений в word документ
                    ReplaceWordsStub("{res}", res, wordDocument);
                    ReplaceWordsStub("{sname}", sname, wordDocument);
                    ReplaceWordsStub("{name}", name, wordDocument);
                    ReplaceWordsStub("{patr}", patr, wordDocument);
                    ReplaceWordsStub("{date}", date, wordDocument);

                    // Сохранение word документа с указанным именем
                    wordDocument.SaveAs(@"C:\Users\Кирилл\Desktop\08.09\"+sname+" "+name+" "+patr+"_"+test+".docx");
                    wordApp.Visible = true;
                }
            }
            catch
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReplaceWordsStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText:stubToReplace, ReplaceWith: text);

        }

        public string testname;

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                chart1.Series[0].Points.Clear();
                using (SqlConnection sqlconnection = new SqlConnection(connection))
                {
                    sqlconnection.Open();
                    DataSet ds = new DataSet();
                    testname = comboBox7.SelectedValue.ToString();
                    // Получение количества пользователей
                    string query = "select count(History.login) as c " +
                        "from History, Test where Test.name = '" + testname + "' and Test.test_number = History.test_number";
                    sda = new SqlDataAdapter(query, sqlconnection);
                    sda.Fill(ds);
                    int c = Convert.ToInt32(ds.Tables[0].Rows[0]["c"].ToString());

                    DataSet ds2 = new DataSet();
                    // Получение данных из бд
                    string query2 = "select Test.name as Название_теста, History.login as Пользователь, History.result as Результат, History.score as Количество_баллов, History.time as Время, History.date as Дата from History, Test where Test.name = '" + testname + "' and Test.test_number = History.test_number";
                    sda = new SqlDataAdapter(query2, sqlconnection);
                    sda.Fill(ds2);

                    // Вывод данных из бд в chart
                    for (int i = 0; i < c; i++)
                    {
                        chart1.Series[0].Points.AddXY(ds2.Tables[0].Rows[i]["Пользователь"].ToString(), ds2.Tables[0].Rows[i]["Количество_баллов"].ToString());
                    }
                    sqlconnection.Close();
                }
            }
            catch
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

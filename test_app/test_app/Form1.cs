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
    public partial class Form1 : Form
    {
        public string connection;
        public SqlDataAdapter sda;

        public Form1()
        {
            InitializeComponent();
            connection = @"Data Source=ПК\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Получение данных из заполненных полей
            string login = textBox6.Text;
            string password = textBox7.Text;
            string password2 = textBox8.Text;
            string surename = textBox3.Text;
            string name = textBox4.Text;
            string patronymic = textBox5.Text;

            if (login == "" || password == "" || password2 == "" || surename == "" || name == "")
            {
                MessageBox.Show("Вы не заполнили одно из полей!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (password.Length < 6)
                {
                    MessageBox.Show("Длина пароля должна быть не менее 6 символов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (password != password2)
                    {
                        MessageBox.Show("Пароль не совпадает с подтверждением пароля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Занесение пользователя в базу данных
                        DataSet ds = new DataSet();
                        using (SqlConnection sqlconnection = new SqlConnection(connection))
                        {
                            sqlconnection.Open();
                            // Получение списка пользователей
                            string query = "SELECT * FROM Users WHERE login = '" + login + "'";
                            sda = new SqlDataAdapter(query, sqlconnection);
                            sda.Fill(ds);

                            if (ds.Tables[0].Rows.Count == 1)
                            {
                                MessageBox.Show("Пользователь с данным логином уже зарегистрирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                // Занесение пользователя в базу данных
                                query = "INSERT INTO [dbo].[Users]([login],[password],[surename],[name] ,[patronymic]) " +
                                    "VALUES('" + login + "','" + password + "','" + surename + "','" + name + "','" + patronymic + "')";
                                sda = new SqlDataAdapter(query, sqlconnection);
                                sda.Fill(ds);
                                MessageBox.Show("Вы зарегистрировались!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            sqlconnection.Close();
                        }
                        tabControl1.SelectedTab = tabPage1;
                        clear();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Показ или скрытие пароля в текстовом поле
            if (textBox2.PasswordChar == '*')
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        public string login;
        string password;

        private void button1_Click(object sender, EventArgs e)
        {
            login = textBox1.Text;
            password = textBox2.Text;

            // Вход в систему под администратором
            if (login == "kirill" && password == "kirill")
            {
                MessageBox.Show("Вы зашли как администратор!", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                // Открытие формы администратора
                Admin admin = new Admin();
                admin.Show();
            }
            else
            {
                string query = "SELECT login,password FROM Users WHERE login = '" + login + "' AND password = '" + password + "'";
                using (SqlConnection sqlconnection = new SqlConnection(connection))
                {
                    sqlconnection.Open();

                    sda = new SqlDataAdapter(query, sqlconnection);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    // Проверка корректности введенных логина и пароля
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("Логин или пароль указаны неверно!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Вы зашли как пользователь " + login + "!", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        // Открытие формы пользователя
                        User user = new User();
                        user.l = login;
                        user.Show();
                    }
                    sqlconnection.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Переход на страницу с регистрацией
            tabControl1.SelectedTab = tabPage2;
        }

        private void clear()
        {
            // Метод очитски текстовых полей
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Очитска текстовых полей
            clear();
        }
    }
}

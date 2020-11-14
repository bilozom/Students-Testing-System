using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_app
{
    public partial class Result : Form
    {
        public Result()
        {
            InitializeComponent();
        }
        
        public string res;

        private void button1_Click(object sender, EventArgs e)
        {
            // Закрытие формы
            this.Close();
        }

        private void Result_Load(object sender, EventArgs e)
        {
            label2.Text = "Вы набрали "+res+" баллов из 100";
        }
    }
}

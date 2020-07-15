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

namespace Program
{
    public partial class FormLogin : Form
    {
        SQLiteConnection conn = new SQLiteConnection("Data Source = database.db");
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            string sqlselect = "select*from tbLogin where Username = '" + txtuser.Text + "'and Password='" + txtpass.Text + "'";
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sqlselect, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read() == true)
            {
                this.Hide();
                FormMain f2 = new FormMain();
                f2.Show();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu","Đăng nhập thất bại");
            }
            conn.Close();
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

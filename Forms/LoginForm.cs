using System;
using System.Linq;
using System.Windows.Forms;
using Windows_Forms.Models;

namespace Windows_Forms.Forms
{
    public partial class LoginForm : Form
    {

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền Username và Password");
                return;
            }

            DatabaseDataContext db = new DatabaseDataContext();

            var user = db.Users.FirstOrDefault(u => u.Username == username
                                               && u.Password == password);

            if (user != null)
            {
                MessageBox.Show("Đăng nhập thành công! Welcome " + user.Username);
                QLSVForm form = new QLSVForm();
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai Username hoặc Password");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

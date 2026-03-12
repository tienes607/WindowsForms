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
                MessageBox.Show("Please enter username and password.");
                return;
            }

            DatabaseDataContext db = new DatabaseDataContext();

            var user = db.Users
                         .FirstOrDefault(u => u.Username == username
                                           && u.Password == password);

            if (user != null)
            {
                MessageBox.Show("Login successful! Welcome " + user.Username);
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
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

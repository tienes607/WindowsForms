using System;
using System.Windows.Forms;
using Windows_Forms.Models;

namespace Windows_Forms.Forms
{
    public partial class QLSVForm : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        public QLSVForm()
        {
            InitializeComponent();
        }
        private void btnNavLop_Click(object sender, EventArgs e)
        {
            QLLopForm lopForm = new QLLopForm();
            lopForm.Show();
            this.Hide();
        }
    }
}

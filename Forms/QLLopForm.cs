using System;
using System.Windows.Forms;
using Windows_Forms.Models;

namespace Windows_Forms.Forms
{
    public partial class QLLopForm : Form
    {
        private DatabaseDataContext db = new DatabaseDataContext();
        public QLLopForm()
        {
            InitializeComponent();
        }
        private void btnNavSinhVien_Click(object sender, EventArgs e)
        {
            QLSVForm svForm = new QLSVForm();
            svForm.Show();
            this.Close();
        }
    }
}

using System;
using System.Linq;
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
            LoadLop();
        }

        int currentPage = 1;
        int pageSize = 5;
        int totalPage = 0;
        void LoadData()
        {
            DatabaseDataContext db = new DatabaseDataContext();

            var query = db.Lops.AsQueryable();

            int totalRecord = query.Count();

            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            var data = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            dgvLop.DataSource = data;

            lblPage.Text = $"Page {currentPage}/{totalPage}";
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                LoadData();
            }
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }
        private void QLLopForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadLop(string keyword = "")
        {
            var lops = db.Lops.ToList();

            var query = from lop in lops
                        where keyword == "" || lop.TenLop.Contains(keyword)
                        select new
                        {
                            lop.MaLop,
                            lop.TenLop,
                            SoSinhVien = db.SinhViens.Count(sv => sv.MaLop == lop.MaLop)
                        };

            dgvLop.DataSource = query.ToList();

            if (dgvLop.Columns.Count > 0)
            {
                dgvLop.Columns["MaLop"].HeaderText = "Mã Lớp";
                dgvLop.Columns["TenLop"].HeaderText = "Tên Lớp";
                dgvLop.Columns["SoSinhVien"].HeaderText = "Số Sinh Viên";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenLop.Text))
            {
                MessageBox.Show("Vui lòng nhập tên lớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Lop lop = new Lop
            {
                TenLop = txtTenLop.Text.Trim()
            };

            db.Lops.InsertOnSubmit(lop);
            db.SubmitChanges();

            MessageBox.Show("Thêm lớp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
            LoadLop();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvLop.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn lớp cần sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTenLop.Text))
            {
                MessageBox.Show("Vui lòng nhập tên lớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maLop = (int)dgvLop.SelectedRows[0].Cells["MaLop"].Value;
            var lop = db.Lops.FirstOrDefault(l => l.MaLop == maLop);

            if (lop != null)
            {
                lop.TenLop = txtTenLop.Text.Trim();
                db.SubmitChanges();
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadLop();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvLop.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn lớp cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maLop = (int)dgvLop.SelectedRows[0].Cells["MaLop"].Value;

            int soSV = db.SinhViens.Count(sv => sv.MaLop == maLop);
            if (soSV > 0)
            {
                MessageBox.Show($"Không thể xóa! Lớp này còn {soSV} sinh viên.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa lớp này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var lop = db.Lops.FirstOrDefault(l => l.MaLop == maLop);
                if (lop != null)
                {
                    db.Lops.DeleteOnSubmit(lop);
                    db.SubmitChanges();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadLop();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadLop(txtSearch.Text.Trim());
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLop.Rows[e.RowIndex];
                txtTenLop.Text = row.Cells["TenLop"].Value?.ToString();
            }
        }

        private void btnNavSinhVien_Click(object sender, EventArgs e)
        {
            QLSVForm svForm = new QLSVForm();
            svForm.Show();
            this.Close();
        }

        private void btnNavLop_Click(object sender, EventArgs e)
        {
            btnNavLop.BackColor = System.Drawing.Color.FromArgb(30, 136, 229);
            btnNavLop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnNavSinhVien.BackColor = System.Drawing.Color.Transparent;
            btnNavSinhVien.Font = new System.Drawing.Font("Segoe UI", 10F);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ClearForm()
        {
            txtTenLop.Text = "";
            dgvLop.ClearSelection();
        }
    }
}

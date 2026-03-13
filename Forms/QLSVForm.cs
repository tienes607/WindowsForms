using System;
using System.Linq;
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
            LoadLop();
            LoadSinhVien();
        }

        private void LoadLop()
        {
            cbLop.DataSource = db.Lops.ToList();
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "MaLop";
            cbLop.SelectedIndex = -1;
        }

        private void LoadSinhVien(string keyword = "")
        {
            var sinhViens = db.SinhViens.ToList();
            var lops = db.Lops.ToList();

            var query = from sv in sinhViens
                        where keyword == "" || sv.TenSV.Contains(keyword)
                        select new
                        {
                            sv.MSSV,
                            sv.TenSV,
                            NgaySinh = sv.NgaySinh.HasValue ? sv.NgaySinh.Value.ToShortDateString() : "",
                            sv.GioiTinh,
                            TenLop = lops.FirstOrDefault(l => l.MaLop == sv.MaLop) != null
                                     ? lops.FirstOrDefault(l => l.MaLop == sv.MaLop).TenLop
                                     : ""
                        };

            dgvSinhVien.DataSource = query.ToList();

            if (dgvSinhVien.Columns.Count > 0)
            {
                dgvSinhVien.Columns["MSSV"].HeaderText = "MSSV";
                dgvSinhVien.Columns["TenSV"].HeaderText = "Tên Sinh Viên";
                dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
                dgvSinhVien.Columns["TenLop"].HeaderText = "Lớp";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenSV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SinhVien sv = new SinhVien
            {
                TenSV = txtTenSV.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = cbGioiTinh.Text,
                MaLop = cbLop.SelectedIndex == -1 ? (int?)null : (int)cbLop.SelectedValue
            };

            db.SinhViens.InsertOnSubmit(sv);
            db.SubmitChanges();

            MessageBox.Show("Thêm sinh viên thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
            LoadSinhVien();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTenSV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mssv = (int)dgvSinhVien.SelectedRows[0].Cells["MSSV"].Value;
            var sv = db.SinhViens.FirstOrDefault(s => s.MSSV == mssv);

            if (sv != null)
            {
                sv.TenSV = txtTenSV.Text.Trim();
                sv.NgaySinh = dtpNgaySinh.Value;
                sv.GioiTinh = cbGioiTinh.Text;
                sv.MaLop = cbLop.SelectedIndex == -1 ? (int?)null : (int)cbLop.SelectedValue;

                db.SubmitChanges();
                MessageBox.Show("Cập nhật thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadSinhVien();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mssv = (int)dgvSinhVien.SelectedRows[0].Cells["MSSV"].Value;
            string tenSV = dgvSinhVien.SelectedRows[0].Cells["TenSV"].Value?.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa sinh viên \"{tenSV}\"?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var sv = db.SinhViens.FirstOrDefault(s => s.MSSV == mssv);
                if (sv != null)
                {
                    db.SinhViens.DeleteOnSubmit(sv);
                    db.SubmitChanges();
                    MessageBox.Show("Xóa thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadSinhVien();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtSearch.Text = "";
            LoadSinhVien();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSinhVien(txtSearch.Text.Trim());
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

            txtTenSV.Text = row.Cells["TenSV"].Value?.ToString();

            string ngaySinhStr = row.Cells["NgaySinh"].Value?.ToString();
            if (DateTime.TryParse(ngaySinhStr, out DateTime ngaySinh))
                dtpNgaySinh.Value = ngaySinh;

            cbGioiTinh.Text = row.Cells["GioiTinh"].Value?.ToString();

            string tenLop = row.Cells["TenLop"].Value?.ToString();
            foreach (var lop in db.Lops.ToList())
            {
                if (lop.TenLop == tenLop)
                {
                    cbLop.SelectedValue = lop.MaLop;
                    break;
                }
            }
        }

        private void btnNavSinhVien_Click(object sender, EventArgs e)
        {
        }

        private void btnNavLop_Click(object sender, EventArgs e)
        {
            QLLopForm lopForm = new QLLopForm();
            lopForm.Show();
            this.Close();
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
            txtTenSV.Text = "";
            dtpNgaySinh.Value = DateTime.Today;
            cbGioiTinh.SelectedIndex = -1;
            cbLop.SelectedIndex = -1;
            dgvSinhVien.ClearSelection();
        }
    }
}

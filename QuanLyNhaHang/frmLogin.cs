using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using QuanLyNhaHang.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Services;
namespace QuanLyNhaHang
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();      
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;

            // 1. Kiểm tra nhập liệu trống
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                lblError.Text = "Vui lòng nhập đầy đủ thông tin!";
                return;
            }

            try
            {
                // 2. Truy vấn kiểm tra tài khoản (mật khẩu so sánh trực tiếp)
                string sql = "SELECT MaND FROM NguoiDung WHERE TenDangNhap=@u AND MatKhau=@p AND TrangThai=1";

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", user);
                        cmd.Parameters.AddWithValue("@p", pass);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 3. Đăng nhập thành công -> Chuyển màn hình chính
                                this.Hide();
                                new MainForm().ShowDialog();
                                this.Close();
                            }
                            else
                            {
                                // 4. Đăng nhập thất bại
                                lblError.Text = "Sai tên đăng nhập hoặc mật khẩu!";
                                txtPassword.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
       
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
           
        }
    }
}
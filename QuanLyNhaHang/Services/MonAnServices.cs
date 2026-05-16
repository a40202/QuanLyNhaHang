using MySql.Data.MySqlClient;
using QuanLyNhaHang.Database;
using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Services
{
    /// <summary>Xử lý toàn bộ nghiệp vụ liên quan đến Món Ăn.</summary>
    public class MonAnServices
    {
        /// <summary>Lấy danh sách tất cả món ăn, join với danh mục.</summary>
        public List<MonAn> GetAll()
        {
            var list = new List<MonAn>();
            string sql = @"SELECT m.MaMA, m.TenMon, m.MaDM, d.TenDanhMuc,
                                  m.GiaBan, m.MoTa, m.TrangThai
                           FROM MonAn m
                           LEFT JOIN DanhMuc d ON m.MaDM = d.MaDM
                           ORDER BY m.TenMon";
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new MonAn
                            {
                                MaMA = reader.GetInt32("MaMA"),
                                TenMon = reader.GetString("TenMon"),
                                MaDM = reader.GetInt32("MaDM"),
                                TenDanhMuc = reader.IsDBNull(reader.GetOrdinal("TenDanhMuc"))
                                             ? "" : reader.GetString("TenDanhMuc"),
                                GiaBan = reader.GetDecimal("GiaBan"),
                                MoTa = reader.IsDBNull(reader.GetOrdinal("MoTa"))
                                             ? "" : reader.GetString("MoTa"),
                                TrangThai = reader.GetBoolean("TrangThai")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tải danh sách món ăn: " + ex.Message);
            }
            return list;
        }

        /// <summary>Tìm kiếm theo tên hoặc danh mục.</summary>
        public List<MonAn> Search(string keyword, int maDM = 0)
        {
            var list = new List<MonAn>();
            string sql = @"SELECT m.MaMA, m.TenMon, m.MaDM, d.TenDanhMuc,
                                  m.GiaBan, m.MoTa, m.TrangThai
                           FROM MonAn m
                           LEFT JOIN DanhMuc d ON m.MaDM = d.MaDM
                           WHERE m.TenMon LIKE @kw
                             AND (@maDM = 0 OR m.MaDM = @maDM)
                           ORDER BY m.TenMon";
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
                    cmd.Parameters.AddWithValue("@maDM", maDM);
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            list.Add(new MonAn
                            {
                                MaMA = reader.GetInt32("MaMA"),
                                TenMon = reader.GetString("TenMon"),
                                MaDM = reader.GetInt32("MaDM"),
                                TenDanhMuc = reader.IsDBNull(3) ? "" : reader.GetString("TenDanhMuc"),
                                GiaBan = reader.GetDecimal("GiaBan"),
                                TrangThai = reader.GetBoolean("TrangThai")
                            });
                }
            }
            return list;
        }

        /// <summary>Thêm món ăn mới. Trả về số dòng bị ảnh hưởng.</summary>
        public int Add(MonAn ma)
        {
            string sql = "INSERT INTO MonAn(TenMon,MaDM,GiaBan,MoTa,TrangThai) " +
                         "VALUES(@ten,@maDM,@gia,@moTa,@tt)";
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", ma.TenMon);
                    cmd.Parameters.AddWithValue("@maDM", ma.MaDM);
                    cmd.Parameters.AddWithValue("@gia", ma.GiaBan);
                    cmd.Parameters.AddWithValue("@moTa", ma.MoTa ?? "");
                    cmd.Parameters.AddWithValue("@tt", ma.TrangThai ? 1 : 0);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Cập nhật thông tin món ăn.</summary>
        public int Update(MonAn ma)
        {
            string sql = "UPDATE MonAn SET TenMon=@ten, MaDM=@maDM, GiaBan=@gia, " +
                         "MoTa=@moTa, TrangThai=@tt WHERE MaMA=@id";
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", ma.TenMon);
                    cmd.Parameters.AddWithValue("@maDM", ma.MaDM);
                    cmd.Parameters.AddWithValue("@gia", ma.GiaBan);
                    cmd.Parameters.AddWithValue("@moTa", ma.MoTa ?? "");
                    cmd.Parameters.AddWithValue("@tt", ma.TrangThai ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", ma.MaMA);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Xóa món ăn theo mã (chỉ xóa nếu chưa có trong hóa đơn).</summary>
        public int Delete(int maMA)
        {
            // Kiểm tra ràng buộc
            string check = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaMA=@id";
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(check, conn))
                {
                    cmd.Parameters.AddWithValue("@id", maMA);
                    long count = (long)cmd.ExecuteScalar();
                    if (count > 0)
                        throw new Exception("Không thể xóa vì món đã có trong hóa đơn!");
                }
                string sql = "DELETE FROM MonAn WHERE MaMA=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", maMA);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
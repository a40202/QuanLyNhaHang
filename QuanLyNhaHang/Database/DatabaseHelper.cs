using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace QuanLyNhaHang.Database
{
    /// <summary>
    /// Quản lý kết nối MySQL — Singleton pattern
    /// </summary>
    public class DatabaseHelper
    {
        private static readonly string connectionString =
            "Server=localhost;Port=3306;Database=quanlynhahang;" +
            "Uid=root;Pwd=;CharSet=utf8mb4;";

        /// <summary>
        /// Tạo và trả về kết nối MySQL mới.
        /// Gọi .Open() trước khi dùng.
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Kiểm tra kết nối tới server có thành công không.
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

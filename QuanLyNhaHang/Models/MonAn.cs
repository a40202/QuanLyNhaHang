using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Models
{
    /// <summary>Đại diện cho một món ăn trong nhà hàng.</summary>
    public class MonAn
    {
        public int MaMA { get; set; }
        public string TenMon { get; set; }
        public int MaDM { get; set; }
        public string TenDanhMuc { get; set; }   
        public decimal GiaBan { get; set; }
        public string MoTa { get; set; }
        public bool TrangThai { get; set; } = true;
    }
}
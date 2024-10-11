using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cls.DAL.DataAccess.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsDone { get; set; }
        public DateTime Date { get; set; }

        // Many-to-Many relation with Product
        public ICollection<ProductBill> ProductBills { get; set; }

    }
}

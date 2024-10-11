using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cls.DAL.DataAccess.Models
{
    public class ProductBill
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }

        public int Quantity { get; set; }
        //public decimal Price { get; set; }

    }
}

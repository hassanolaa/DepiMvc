using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cls.DAL.DataAccess.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }  // Foreign Key
        public string Image { get; set; }
        public string Description { get; set; }

        // Navigation Property to Category
        public Category Category { get; set; }

        // Many-to-Many relation with Bill
        public ICollection<ProductBill> ProductBills { get; set; }

    }
}

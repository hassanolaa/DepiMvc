using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_cls.DAL.DataAccess.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        // Navigation Property to represent the products in this category
        public ICollection<Product> Products { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Models
{
    internal class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Price { get; set; }
        [MaxLength(50)]
        public string? Color { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public bool IsChosen { get; set; }
        public int Stock { get; set; }
    }
}

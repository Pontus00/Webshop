using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public List<OrderItem>? Items { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal ShippingPrice { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal VAT { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal TotalPrice { get; set; }
    }

    public enum PaymentMethod
    {
        Invoice = 0,
        CreditCard = 1,
        PayPal = 2,
        Swish = 3
    }
    
    public enum ShippingMethod
    {
        Budget = 1,
        Standard = 2,
        Express = 3,
    }
}

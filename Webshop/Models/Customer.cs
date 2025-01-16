using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public int Age { get; set; }
        [MaxLength(50)]
        public string? Phone { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Card>? Cards { get; set; }
        public List<Address>? Addresses { get; set; }
    }

    internal class Address
    {
        public AddressType Type { get; set; }
        [MaxLength(100)]
        public string? Street { get; set; }
        [MaxLength(10)]
        public string? ZipCode { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(100)]
        public string? Country { get; set; }
    }
    public enum AddressType
    {
        Delivery = 1,
        Invoice = 2
    }
}

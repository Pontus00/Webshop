using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Models
{
    internal class Card
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Bank { get; set; }
        [MaxLength(50)]
        public string? Number {  get; set; }
        public int ExpiresYear { get; set; }
        public int ExpiresMonth { get; set; }
        [MaxLength(100)]
        public string? CardHolderName { get; set; }
        [MaxLength(3)]
        public string? CVV { get; set; }

        public Customer? Customer { get; set; }
    }
}

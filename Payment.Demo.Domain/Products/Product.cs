using Payment.Demo.Domain.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Demo.Domain.Products
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public required string ImageUrl {get; set;}

        public decimal Price { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = [];
    }
}

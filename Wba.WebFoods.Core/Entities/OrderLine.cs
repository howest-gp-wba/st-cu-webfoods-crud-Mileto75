using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wba.WebFoods.Core.Entities
{
    public class OrderLine
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
    }
}

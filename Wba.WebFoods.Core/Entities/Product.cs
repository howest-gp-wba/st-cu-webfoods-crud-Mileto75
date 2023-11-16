using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wba.WebFoods.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        //one to many
        public Category Category { get; set; }
        //unshadowed foreign key property
        public int? CategoryId { get; set; }
        public string Image { get; set; }
        //many to many
        public ICollection<Property> Properties { get; set; }
        public ICollection<OrderLine> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wba.WebFoods.Core.Entities
{
    public class Category : BaseEntity
    {
        //the many
        public ICollection<Product> Products { get; set; }
    }
}

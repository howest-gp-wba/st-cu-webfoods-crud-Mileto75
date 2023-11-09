using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wba.WebFoods.Core.Entities
{
    public class Property : BaseEntity
    {
        public ICollection<Product> Products { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wba.WebFoods.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName{ get; set; }
        [Required]
        public string LastName{ get; set; }
        //many to many
        public ICollection<OrderLine> Orders { get; set; }
    }
}

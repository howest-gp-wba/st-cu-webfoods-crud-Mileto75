using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Wba.WebFoods.Web.ViewModels
{
    public class ProductsCreateViewModel
    {
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0.01,100,ErrorMessage = "Please provide a positive value")]
        public decimal Price { get; set; }
        //category
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}

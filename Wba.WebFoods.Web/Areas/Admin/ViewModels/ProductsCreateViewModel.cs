using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Wba.WebFoods.Web.Areas.Admin.ViewModels
{
    public class ProductsCreateViewModel
    {
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0.01,100,ErrorMessage = "Please provide a positive value")]
        public decimal Price { get; set; }
        //category one to many
        public IEnumerable<SelectListItem> Categories { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        //properties many to many
        public IEnumerable<SelectListItem> Properties { get; set; }
       // [Required(ErrorMessage = "Please select at least one property!")]
        public IEnumerable<int> PropertyIds { get; set; }
        [Display(Name = "Image")]
        public IFormFile Image { get; set; }
    }
}

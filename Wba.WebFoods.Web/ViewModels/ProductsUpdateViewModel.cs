using Microsoft.AspNetCore.Mvc;

namespace Wba.WebFoods.Web.ViewModels
{
    public class ProductsUpdateViewModel : ProductsCreateViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
    }
}

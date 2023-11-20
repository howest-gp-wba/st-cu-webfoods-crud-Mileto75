using Microsoft.AspNetCore.Mvc;

namespace Wba.WebFoods.Web.Areas.Admin.ViewModels
{
    public class ProductsUpdateViewModel : ProductsCreateViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
    }
}

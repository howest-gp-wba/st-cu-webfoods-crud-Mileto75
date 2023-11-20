

namespace Wba.WebFoods.Web.Areas.Admin.ViewModels
{
    public class ProductsInfoViewModel : BaseViewModel
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public BaseViewModel Category { get; set; }
        public string Image { get; set; }
        public IEnumerable<BaseViewModel> Properties { get; set; }
    }
}

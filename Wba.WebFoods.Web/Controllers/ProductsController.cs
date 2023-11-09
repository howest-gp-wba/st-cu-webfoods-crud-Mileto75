using Microsoft.AspNetCore.Mvc;
using Wba.WebFoods.Web.Data;
using Wba.WebFoods.Web.ViewModels;

namespace Wba.WebFoods.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebFoodsDbContext _webFoodsDbContext;

        //dependency injection
        public ProductsController(WebFoodsDbContext webFoodsDbContext)
        {
            _webFoodsDbContext = webFoodsDbContext;
        }

        public IActionResult Index()
        {
            //show a product list
            //get the products
            var products = _webFoodsDbContext.Products.ToList();
            //create a viewmodel
            var productsIndexViewModel = new ProductsIndexViewModel
            {
                Products = products.Select(p => new BaseViewModel
                {
                    Id = p.Id,
                    Value = p.Name,
                })
            };
            //fill the model
            //pass to the view
            return View(productsIndexViewModel);
        }
    }
}

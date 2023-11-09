using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Info(int id)
        {
            //get the product
            var product = _webFoodsDbContext
                .Products
                .Include(p => p.Category)
                .Include(p => p.Properties)
                .FirstOrDefault(p => p.Id == id);
            //check if null
            if(product == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            //fill the model
            var productsInfoViewModel = new ProductsInfoViewModel
            {
                Id = product.Id,
                Value = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = new BaseViewModel
                {
                    Id = product.Category.Id,
                    Value = product.Category.Name
                },
                Properties = product.Properties.Select
                (p => new BaseViewModel { Id = p.Id, Value = p.Name })
            };
            //pass to the view
            return View(productsInfoViewModel);
        }
    }
}

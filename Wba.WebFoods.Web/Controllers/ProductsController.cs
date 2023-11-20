using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Wba.WebFoods.Core.Entities;
using Wba.WebFoods.Web.Data;
using Wba.WebFoods.Web.ViewModels;

namespace Wba.WebFoods.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebFoodsDbContext _webFoodsDbContext;
        private readonly ILogger<ProductsController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //dependency injection
        public ProductsController(WebFoodsDbContext webFoodsDbContext, ILogger<ProductsController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _webFoodsDbContext = webFoodsDbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
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
        [HttpGet]
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
            //check if image == null
            var imageUrl = "https://placehold.co/600x400";
            if (product.Image != null)
            {
                imageUrl = $"/images/{product.Image}";
            }
            var productsInfoViewModel = new ProductsInfoViewModel
            {
                Id = product.Id,
                Value = product.Name,
                Description = product.Description,
                Price = Math.Round(product.Price,2),
                Image = imageUrl, 
                Category = new BaseViewModel
                {
                    Id = product?.Category?.Id ?? 0,
                    Value = product?.Category?.Name ?? "NoCat"
                },
                Properties = product.Properties.Select
                (p => new BaseViewModel { Id = p.Id, Value = p.Name })
            };
            //pass to the view
            return View(productsInfoViewModel);
        }
    }
}

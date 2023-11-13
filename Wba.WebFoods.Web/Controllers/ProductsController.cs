using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wba.WebFoods.Core.Entities;
using Wba.WebFoods.Web.Data;
using Wba.WebFoods.Web.ViewModels;

namespace Wba.WebFoods.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebFoodsDbContext _webFoodsDbContext;
        private readonly ILogger<ProductsController> _logger;

        //dependency injection
        public ProductsController(WebFoodsDbContext webFoodsDbContext, ILogger<ProductsController> logger)
        {
            _webFoodsDbContext = webFoodsDbContext;
            _logger = logger;
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
            var productsInfoViewModel = new ProductsInfoViewModel
            {
                Id = product.Id,
                Value = product.Name,
                Description = product.Description,
                Price = product.Price,
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
        [HttpGet]
        public IActionResult Create()
        {
            //show the form
            //fill the categories list
            //get the categories
            var categories = _webFoodsDbContext
                .Categories.ToList();
            var productsCreateViewModel = new ProductsCreateViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),Text = c.Name
                })
            };
            //fill the properties list
            var properties = _webFoodsDbContext
                .Properties.ToList();
            productsCreateViewModel.Properties
                = properties.Select(p => new SelectListItem 
                {
                    Value = p.Id.ToString(),
                    Text = p.Name   
                });
            return View(productsCreateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductsCreateViewModel productsCreateViewModel)
        {
            var categories = _webFoodsDbContext .Categories.ToList();
            productsCreateViewModel.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            //fill the properties
            var properties = _webFoodsDbContext
                .Properties.ToList();
            productsCreateViewModel.Properties
                = properties.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                });
            //check the modelstate = validation
            if (!ModelState.IsValid)
            {
                return View(productsCreateViewModel);
            }
            //create the product
            //check if product exists in db
            if(_webFoodsDbContext
                .Products.Any(p => p.Name.ToUpper()
                .Equals(productsCreateViewModel.Name.ToUpper())))
            {
                //add custom error
                ModelState.AddModelError("Name","Product already in database!");
                return View(productsCreateViewModel);
            }
            //get the selected properties
            var selectedProperties = _webFoodsDbContext
                .Properties
                .Where(p => productsCreateViewModel.PropertyIds.Contains(p.Id))
                .ToList();
            var product = new Product 
            {
                Name = productsCreateViewModel.Name,
                Description = productsCreateViewModel.Description,
                Price = productsCreateViewModel.Price,
                //add the category, use the unshadowed foreign key
                CategoryId = productsCreateViewModel.CategoryId,
                Properties = selectedProperties,
            };
            //add to the databasecontext
            _webFoodsDbContext.Products.Add(product);
            //save the changes
            try 
            {
                _webFoodsDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbUpdateException)
            {
                //log the error
                _logger.LogCritical(dbUpdateException.Message);
                //add custom error message
                ModelState.AddModelError("", "Unkown error, try again later!");
                return View(productsCreateViewModel);
            }
        }
    }
}

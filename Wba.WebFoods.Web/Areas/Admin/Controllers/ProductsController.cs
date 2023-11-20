using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Wba.WebFoods.Core.Entities;
using Wba.WebFoods.Web.Areas.Admin.ViewModels;
using Wba.WebFoods.Web.Data;
using Wba.WebFoods.Web.Services;
using Wba.WebFoods.Web.Services.Interfaces;

namespace Wba.WebFoods.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly WebFoodsDbContext _webFoodsDbContext;
        private readonly ILogger<ProductsController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFormBuilderService _formBuilderService;
        private readonly IFileService _fileService; 

        //dependency injection
        public ProductsController(WebFoodsDbContext webFoodsDbContext, ILogger<ProductsController> logger, IWebHostEnvironment webHostEnvironment, IFormBuilderService formBuilderService, IFileService fileService)
        {
            _webFoodsDbContext = webFoodsDbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _formBuilderService = formBuilderService;
            _fileService = fileService;
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
            if (product == null)
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
                Price = Math.Round(product.Price, 2),
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
        [HttpGet]
        public IActionResult Create()
        {
            //show the form
            //fill the categories list
            //get the categories
            var productsCreateViewModel = new ProductsCreateViewModel
            {
                Categories = _formBuilderService.GetCategoryList(),
            };
            //fill the properties list
            var properties = _webFoodsDbContext
                .Properties.ToList();
            productsCreateViewModel.Properties
                = _formBuilderService.GetPropertiesList();
            return View(productsCreateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductsCreateViewModel productsCreateViewModel)
        {
            var categories = _webFoodsDbContext.Categories.ToList();

            productsCreateViewModel.Categories =
                _formBuilderService.GetCategoryList();
            //fill the properties
            var properties = _webFoodsDbContext
                .Properties.ToList();
            productsCreateViewModel.Properties
                = _formBuilderService.GetPropertiesList();
            //check the modelstate = validation
            if (!ModelState.IsValid)
            {
                return View(productsCreateViewModel);
            }
            //create the product
            //check if product exists in db
            if (_webFoodsDbContext
                .Products.Any(p => p.Name.ToUpper()
                .Equals(productsCreateViewModel.Name.ToUpper())))
            {
                //add custom error
                ModelState.AddModelError("Name", "Product already in database!");
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
            //null check on image
            if (productsCreateViewModel.Image != null)
            {
                //put the filename in database
                var filename = _fileService.StoreFile(productsCreateViewModel.Image);
                //check if error
                if(filename.Equals("Error"))
                {
                    ModelState.AddModelError("", "File not saved.Try again later!");
                    return View(productsCreateViewModel);
                }
                product.Image = filename;
            }
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
        public IActionResult ConfirmDelete(int id)
        {
            //get the product from the database
            var product = _webFoodsDbContext
                .Products
                .FirstOrDefault(p => p.Id == id);
            //check if null
            if (product == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            var productsDeleteViewModel
                = new ProductsDeleteViewModel
                {
                    Id = product.Id,
                    Value = product.Name,
                };
            return View(productsDeleteViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            //get the product
            var product = _webFoodsDbContext
                .Products
                .FirstOrDefault(p => p.Id == id);
            //get the image for deletion
            var image = product.Image;
            //delete product from dbcontext
            _webFoodsDbContext
                .Products.Remove(product);
            //save the changes
            try
            {
                _webFoodsDbContext.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException.Message);
                return RedirectToAction(nameof(Index));
            }
            //check if image != null => delete
            if (image != null)
            {
                //delete file use service class methode
                _fileService.DeleteFile(product.Image);
            }
            //product deleted
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            //show the product form
            var product = _webFoodsDbContext
                .Products
                .Include(p => p.Category)
                .Include(p => p.Properties)
                .FirstOrDefault(p => p.Id == id);
            //check if null
            if (product == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            //fill the categories list
            //get the categories
            var productsUpdateViewModel = new ProductsUpdateViewModel
            {
                Categories = _formBuilderService.GetCategoryList(),
            };
            //fill the properties list
            productsUpdateViewModel.Properties
                = _formBuilderService.GetPropertiesList();
            //fill the model
            productsUpdateViewModel.Id = product.Id;
            productsUpdateViewModel.Name = product.Name;
            productsUpdateViewModel.Description = product.Description;
            productsUpdateViewModel.CategoryId = (int)product.CategoryId;
            productsUpdateViewModel.Price = product.Price;
            productsUpdateViewModel.PropertyIds = product
                .Properties.Select(pr => pr.Id);
            return View(productsUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductsUpdateViewModel productsUpdateViewModel)
        {
            if(!ModelState.IsValid)
            {
                productsUpdateViewModel.Categories = _formBuilderService
                    .GetCategoryList();
                productsUpdateViewModel.Properties
                    = _formBuilderService.GetPropertiesList(); 
                return View(productsUpdateViewModel);
            }
            //get the product
            var product = _webFoodsDbContext
                .Products
                .Include(p => p.Category)
                .Include(p => p.Properties)
                .FirstOrDefault(p => p.Id == productsUpdateViewModel.Id);
            //check if null
            if (product == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            //update the properties
            product.Name = productsUpdateViewModel.Name;
            product.Description = productsUpdateViewModel.Description;
            product.CategoryId = productsUpdateViewModel.CategoryId;
            product.Price = productsUpdateViewModel.Price;
            product.Properties = _webFoodsDbContext
                .Properties
                .Where(pr => productsUpdateViewModel.PropertyIds.Contains(pr.Id))
                .ToList();
            //check for image
            if(productsUpdateViewModel.Image != null)
            {
                //delete old image
                _fileService.DeleteFile(product.Image);
                //store new image
                var filename = _fileService.StoreFile(productsUpdateViewModel.Image);
                if(filename.Equals("Error"))
                {
                    ModelState.AddModelError("image", "File not saved.Try again later!");
                    return View(productsUpdateViewModel);
                }
                product.Image = filename;
            }
            //save the changes
            try
            {
                _webFoodsDbContext.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException.Message);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Info), new { id = product.Id });
        }
        [HttpGet]
        public IActionResult BulkUpdate()
        {
            //show a list of foods with checkboxes
            //get the foods and put in the model
            var productsBulkUpdateViewModel =
                new ProductsBulkUpdateViewModel
                {
                    Products = _formBuilderService.GetProductCheckBoxesList(),
                };
            return View(productsBulkUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessBulkUpdate(ProductsBulkUpdateViewModel 
            productsBulkUpdateViewModel)
        {
            //get the products based on selected checkbox
            var selectedProductIds = productsBulkUpdateViewModel
                .Products.Where(p => p.IsSelected == true)
                .Select(p => p.Value);
            var productsToUpdate = _webFoodsDbContext
                .Products.Where(p => selectedProductIds.Contains(p.Id)).ToList();
            //fill the model
            var productsProcessBulkUpdateViewModel =
                new ProductsProcessBulkUpdateViewModel 
                {
                    Products = productsToUpdate
                        .Select(p => new ProductsUpdateViewModel 
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            CategoryId = (int)p.CategoryId,
                            Categories = _formBuilderService.GetCategoryList(),
                        }).ToList(),
                };
            //show the form to edit
            return View(productsProcessBulkUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBulkUpdate(ProductsProcessBulkUpdateViewModel
            productsProcessBulkUpdateViewModel)
        {
            //check the modelstate
            if(!ModelState.IsValid)
            {
                foreach(var product in
                    productsProcessBulkUpdateViewModel.Products)
                {
                    product.Categories = _formBuilderService.GetCategoryList();
                }
                return View("ProcessBulkUpdate",productsProcessBulkUpdateViewModel);
            }
            //get the products
            var selectedProductIds = productsProcessBulkUpdateViewModel
                .Products.Select(p => p.Id);
            var productsToUpdate
                = _webFoodsDbContext
                .Products.Where(p => selectedProductIds.Contains(p.Id)).ToList();
            //loop over products and save the changes
            foreach(var product in productsToUpdate)
            { 
                var selectedProduct = 
                    productsProcessBulkUpdateViewModel.Products.FirstOrDefault(p => p.Id == product.Id);
                product.Name = selectedProduct.Name;
                product.Price = selectedProduct.Price;
                product.CategoryId = selectedProduct.CategoryId;
            }
            try
            {
                _webFoodsDbContext.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogCritical(dbUpdateException.Message);
                ModelState.AddModelError("",dbUpdateException.Message);
                return View("ProcessBulkUpdate", productsProcessBulkUpdateViewModel);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

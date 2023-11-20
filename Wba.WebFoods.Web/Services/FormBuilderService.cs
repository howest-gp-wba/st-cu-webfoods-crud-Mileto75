using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wba.WebFoods.Core.Entities;
using Wba.WebFoods.Web.Areas.Admin.ViewModels;
using Wba.WebFoods.Web.Data;
using Wba.WebFoods.Web.Services.Interfaces;


namespace Wba.WebFoods.Web.Services
{
    public class FormBuilderService : IFormBuilderService
    {
        private readonly WebFoodsDbContext _webFoodsDbContext;

        public FormBuilderService(WebFoodsDbContext webFoodsDbContext)
        {
            _webFoodsDbContext = webFoodsDbContext;
        }

        public IEnumerable<SelectListItem> GetCategoryList()
        {
            return _webFoodsDbContext
                .Categories
                .Select(c => new SelectListItem 
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
        }

        public List<CheckboxModel> GetProductCheckBoxesList()
        {
            return _webFoodsDbContext
                .Products
                .Select(c => new CheckboxModel
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList();
        }

        public IEnumerable<SelectListItem> GetPropertiesList()
        {
            return _webFoodsDbContext
                .Properties
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
        }
    }
}

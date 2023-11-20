using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wba.WebFoods.Web.Areas.Admin.ViewModels;

namespace Wba.WebFoods.Web.Services.Interfaces
{
    public interface IFormBuilderService
    {
        IEnumerable<SelectListItem> GetCategoryList();
        IEnumerable<SelectListItem> GetPropertiesList();
        List<CheckboxModel> GetProductCheckBoxesList();
    }
}

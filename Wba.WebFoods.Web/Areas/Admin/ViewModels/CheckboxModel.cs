using Microsoft.AspNetCore.Mvc;

namespace Wba.WebFoods.Web.Areas.Admin.ViewModels
{
    public class CheckboxModel
    {
        public bool IsSelected { get; set; }
        public string Text { get; set; }
        [HiddenInput]
        public int Value { get; set; }
    }
}

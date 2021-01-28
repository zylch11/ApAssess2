using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ApAssess2.Models;

namespace ApAssess2.ViewModels
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public IEnumerable<SelectListItem> CategoryDropDown { get; set; }

    }
}
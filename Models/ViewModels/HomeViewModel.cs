using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ApAssess2.Models;

namespace ApAssess2.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }
}
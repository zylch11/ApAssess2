using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ApAssess2.Models;

namespace ApAssess2.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            product = new Product();
        }
        public Product product { get; set; }
        public bool ExistsInCart { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApAssess2.Models;
using ApAssess2.Data;
using Microsoft.EntityFrameworkCore;
using ApAssess2.ViewModels;
using ApAssess2.Utility;

namespace ApAssess2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = _db.Product.Include(u => u.Category),
                Categories = _db.Category
            };
            return View(homeViewModel);
        }

        public IActionResult Details(Guid? id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Constants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Constants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(Constants.SessionCart);
            }

            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                product = _db.Product.Include(u => u.Category).Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                {
                    detailsViewModel.ExistsInCart = true;
                }
            }

            return View(detailsViewModel);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(Guid id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Constants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Constants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(Constants.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(Constants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(Guid id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Constants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Constants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(Constants.SessionCart);
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }
            
            HttpContext.Session.Set(Constants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

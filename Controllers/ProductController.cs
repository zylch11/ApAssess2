using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using ApAssess2.Models;
using ApAssess2.ViewModels;
using System.IO;
using Microsoft.EntityFrameworkCore;
using ApAssess2.Data;

namespace ApAssess2.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product.Include(u => u.Category);
            return View(objList);
        }

        public IActionResult Create()
        {
            Product tempObj = new Product();
            var tempGuid = Guid.NewGuid();
            tempObj.Id = tempGuid;

            ProductViewModel productViewModel = new ProductViewModel()
            {
                product = tempObj,
                CategoryDropDown = _db.Category.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
            };

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                //Creating
                string upload = webRootPath + Constants.ImagesPath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productViewModel.product.Image = fileName + extension;

                _db.Product.Add(productViewModel.product);

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productViewModel.CategoryDropDown = _db.Category.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(productViewModel);
        }


        public IActionResult Edit(Guid? Id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                product = new Product(),
                CategoryDropDown = _db.Category.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
            };

            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                productViewModel.product = _db.Product.Find(Id);
                if (productViewModel == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                // Updating
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productViewModel.product.Id);

                if (files.Count > 0)
                {
                    string upload = webRootPath + Constants.ImagesPath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    var oldFile = Path.Combine(upload, objFromDb.Image);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.product.Image = fileName + extension;
                }
                else
                {
                    productViewModel.product.Image = objFromDb.Image;
                }
                _db.Product.Update(productViewModel.product);

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            productViewModel.CategoryDropDown = _db.Category.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(productViewModel);
        }

        public IActionResult Delete(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u => u.Category).FirstOrDefault(u => u.Id == Id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Guid? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + Constants.ImagesPath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }

}
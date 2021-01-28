using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApAssess2.Data;
using ApAssess2.Models;
using Microsoft.AspNetCore.Mvc;

namespace testPostgreSQL.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Category;
            return View(objList);
        }

        public IActionResult Create()
        {
            Category tempObj = new Category();
            var tempGuid = Guid.NewGuid();
            tempObj.Id = tempGuid;
            return View(tempObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var _obj = _db.Category.Find(Id);
            if (_obj == null)
            {
                return NotFound();
            }

            return View(_obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var _obj = _db.Category.Find(Id);
            if (_obj == null)
            {
                return NotFound();
            }

            return View(_obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }

}
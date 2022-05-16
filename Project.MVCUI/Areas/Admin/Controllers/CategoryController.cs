using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Areas.Admin.Data.AdminWMClasses;
using Project.MVCUI.AuthenticationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{

    [AdminAuthentication]

    public class CategoryController : Controller
    {
        CategoryRepository _cRep;

        public CategoryController()
        {
            _cRep = new CategoryRepository();
        }

        public ActionResult GetAllCategories() // Bütün Kategorileri getir.
        {
            CategoryVM cvm = new CategoryVM
            {
                Categories = _cRep.GetAll()
            };
            return View(cvm);
        }

        public ActionResult CategoryList(int? id) // ID gelirse o id'de ki kategoriyi getir, gelmezse hepsini getir..
        {
            // id null ise Aktif olan Kategorileri al, null değilse kategorilere ID si id olan tek bir eleman koy.
            CategoryVM cvm = id == null ? new CategoryVM
            {
                Categories = _cRep.GetActives()
            }: new CategoryVM { Categories = _cRep.Where(x => x.ID == id) };


            return View(cvm);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            _cRep.Add(category);
            return RedirectToAction("CategoryList");
        }

        public ActionResult UpdateCategory(int id)
        {
            CategoryVM cvm = new CategoryVM { Category = _cRep.Find(id) };
            return View(cvm);
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            _cRep.Update(category);
            return RedirectToAction("CategoryList");
        }
        public ActionResult DeleteCategory(int id)
        {
            _cRep.Delete(_cRep.Find(id));
            return RedirectToAction("CategoryList");
        }
    }
}
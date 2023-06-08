using ITSD.Areas.ITSD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Areas.ITSD.Controllers
{
    public class CategoryController : Controller
    {
        // GET: ITSD/Category
        tbl_Category cat = new tbl_Category();
        tbl_SubCategory sub = new tbl_SubCategory();
        public ActionResult Index(string Search = "", string SearchSub = "")
        {
            var list = cat.List(Search);
            var listsub = sub.List(SearchSub);
            ViewBag.list = listsub;
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_Category m)
        {
            if (ModelState.IsValid)
            {
                cat.Create(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }

        public ActionResult CreateSub()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateSub(tbl_SubCategory m)
        {
            if (ModelState.IsValid)
            {
                sub.Create(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }

        public ActionResult Edit(int ID)
        {
            var item = cat.Find(ID);
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(tbl_Category m)
        {
            if (ModelState.IsValid)
            {
                cat.Update(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }

        public ActionResult EditSub(int ID)
        {
            var item = sub.Find(ID);
            return View(item);
        }
        [HttpPost]
        public ActionResult EditSub(tbl_SubCategory m)
        {
            if (ModelState.IsValid)
            {
                sub.Update(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }



        public ActionResult Detail(int ID)
        {
            var item = cat.Find(ID);
            return View(item);
        }
        public ActionResult Delete(int ID)
        {
            var item = cat.Find(ID);
            return View(item);
        }
        [HttpPost]
        public ActionResult Delete(tbl_Category m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteSub(int ID)
        {
            var item = sub.Find(ID);
            return View(item);
        }
        [HttpPost]
        public ActionResult DeleteSub(tbl_SubCategory m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }
    }
}
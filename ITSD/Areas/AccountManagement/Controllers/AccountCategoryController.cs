using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITSD.Areas.AccountManagement.Models;

namespace ITSD.Areas.AccountManagement.Controllers
{
    public class AccountCategoryController : Controller
    {
        // GET: AccountManagement/AccountCategory
        tbl_AccCategory mod = new tbl_AccCategory();
        public ActionResult Index(string Search = "")
        {
            var list = mod.List(Search);
            return View(list);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_AccCategory m)
        {
            if (ModelState.IsValid)
            {
                mod.Create(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }
        public ActionResult Edit(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(tbl_AccCategory m)
        {
            if (ModelState.IsValid)
            {
                mod.Update(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }
        public ActionResult Detail(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }
        public ActionResult Delete(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }
        [HttpPost]
        public ActionResult Delete(tbl_AccCategory m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }


    }
}
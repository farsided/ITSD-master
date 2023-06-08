using ITSD.Areas.LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Areas.LMS.Controllers
{
    public class LicenseController : Controller
    {
        // GET: LMS/License
        tbl_LMS mod = new tbl_LMS();
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
        public ActionResult Create(tbl_LMS m)
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
        public ActionResult Edit(tbl_LMS m)
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
        public ActionResult Delete(tbl_LMS m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }


    }
}
using ITSD.Areas.ITSD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Areas.ITSD.Controllers
{
    public class TemplateController : Controller
    {
        // GET: ITSD/Template
        tbl_Template mod = new tbl_Template();
        public ActionResult Index(string Search = "")
        {
            var list = mod.List();
            return View(list);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_Template m)
        {
            if (ModelState.IsValid)
            {
                mod.Create(m);
                return RedirectToAction("Index");
            }
            return View(m);
        }

  
        public ActionResult Edit()
        {
            var item = mod.Find(1);
            return View(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tbl_Template m)
        {
            if (ModelState.IsValid)
            {
                mod.Update(m);
                return RedirectToAction("General", "JobOrder");
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
        public ActionResult Delete(tbl_Template m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }


    }
}
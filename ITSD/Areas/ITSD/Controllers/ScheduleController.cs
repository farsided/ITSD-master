using ITSD.Areas.ITSD.Models;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Areas.ITSD.Controllers
{
    public class ScheduleController : Controller
    {
        tbl_Scheduler mod = new tbl_Scheduler();
        UserSessions session = new UserSessions();
        public ActionResult Index(string Search = "")
        {
            List<tbl_Scheduler> list = null;
            if (session.Level == 1 || session.Level == 2)
            {
                list = mod.List(Search);
            }
            else
            {
                list = mod.List(session.User.ID, Search);
            }
            return View(list);
        }

        public JsonResult Events()
        {
            var list = new List<object>();
            mod.List().ForEach(r =>
            {
                list.Add(new
                {
                    title = r.Title,
                    start = $"{r.Start:yyyy-MM-ddThh:mm}",
                    end = $"{r.End:yyyy-MM-ddThh:mm}",
                    extendedProps = new
                    {
                        Name = r.obj_encBy.Info.Fullname,
                        Department = r.obj_encBy.Info.department,
                        Description = r.Description,
                        Duration = $"<b>From:</b> {r.Start:MMM dd, yyyy hh:mm tt}<br><b>To:</b> {r.End:MMM dd, yyyy hh:mm tt}"
                    }
                });
            });
            var json = Json(list, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_Scheduler m)
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
        public ActionResult Edit(tbl_Scheduler m)
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
        public ActionResult Delete(tbl_Scheduler m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }


    }
}
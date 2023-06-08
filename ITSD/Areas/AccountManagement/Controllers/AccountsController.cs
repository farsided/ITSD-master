using Microsoft.Reporting.WebForms;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITSD.Areas.AccountManagement.Models;

namespace ITSD.Areas.AccountManagement.Controllers
{
    public class AccountsController : Controller
    {
        // GET: AccountManagement/Accounts
        tbl_Accounts mod = new tbl_Accounts();
        UserSessions session = new UserSessions();
        public ActionResult Index(string Search = "")
        {
            var list = mod.List(Search);
            return View(list);
        }

        public ActionResult Print()
        {
            ReportViewer rv = new ReportViewer(); //EXCELOPENXML FOR EXCEL & application/vnd.ms-excel FOR contentType
            rv.LocalReport.ReportPath = Server.MapPath("~/Areas/AccountManagement/Reports/Accounts.rdlc");
            var data = mod.List();
            rv.LocalReport.DataSources.Add(new ReportDataSource("Accounts", data));
            rv.LocalReport.SetParameters(new ReportParameter("Name", session.User?.Info.Fullname));
            var file = rv.LocalReport.Render("pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"inline;filename=AccountsMasterList.pdf");
            Response.Buffer = true;
            Response.Clear();
            Response.BinaryWrite(file);
            Response.End();
            return View(file);
        }

        public ActionResult Action(string Type, int? ID = null)
        {
            switch (Type)
            {
                case "Add":
                    return RedirectToAction("Create");
                case "Edit":
                    return RedirectToAction("Edit", new { ID = ID });
                default:
                    return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tbl_Accounts m)
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
        public ActionResult Edit(tbl_Accounts m)
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
        public ActionResult Delete(tbl_Accounts m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }


    }
}
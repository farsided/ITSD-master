using ITSD.Areas.ITSD.Models;
using ITSD.Hubs;
using Microsoft.Reporting.WebForms;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ITSD.Areas.ITSD.Controllers
{
    public class JobOrderController : Controller
    {
        // GET: ITSD/JobOrder
        tbl_JobOrder mod = new tbl_JobOrder();
        tbl_JobActionTaken mod2 = new tbl_JobActionTaken();
        tbl_Template tem = new tbl_Template();
        UserSessions user = new UserSessions();
        tbl_AllowedToUpdate atu = new tbl_AllowedToUpdate();

        [ActionName("General")]
        public ActionResult Index(string Search = "")
        {
            var list = mod.List(Search).Where(f => isDefault(f.Cancel) && !isDefault(f.Approved) && (f.Status == "Pending" || f.Status == "In progress"));
            return View(list); 
        }

        public PartialViewResult GeneralData(string Search = "")
        {
            var list = mod.List(Search).Where(f => isDefault(f.Cancel) && !isDefault(f.Approved) && (f.Status == "Pending" || f.Status == "In progress"));
            //TemplateHub.BroadCastData();
            return PartialView("GeneralData", list);
        }

        public ActionResult Pending(string Search = "")
        {
            var list = mod.List(Search, user.User.ID).Where(f => f.Status == "Pending" && isDefault(f.Cancel));
            return View(list);
        }

        public ActionResult MasterList(string Search = "")
        {
            var list = mod.List(Search).OrderByDescending(f => f.ID).Where(f => !isDefault(f.Approved));
            return View(list);
        }

        public ActionResult Done(string Search = "")
        {
            var list = mod.List(Search, user.User.ID).Where(f => f.Status == "Done" && !isDefault(f.Approved));
            return View(list);
        }

        public ActionResult Progress(string Search = "")
        {
            var list = mod.List(Search, user.User.ID).Where(f => f.Status == "In progress" && isDefault(f.Cancel) && !isDefault(f.Approved));
            return View(list);
        }

        public ActionResult Cancel(string Search = "")
        {
            var list = mod.List(Search, user.User.ID).Where(f => f.Cancel == true);
            return View(list);
        }

        public ActionResult ForApproval(string Search = "")
        {
            var list = mod.List(Search).Where(f => f.Status == "Pending" && isDefault(f.Approved) && f.encByInfo.Info.department == user.User.Info.department);
            return View(list);
        }

        public ActionResult DepartmentRequest(string Search = "")
        {
            try
            {
                var list = mod.List(Search).Where(f =>
                (f.Status == "Pending" || f.Status == "In progress") &&
                !isDefault(f.Approved) &&
                ( (f.encByInfo.Info.department != null) ? ( f.encByInfo.Info.department  == user.User.Info.department ) : false ) );
                return View(list);
            }
            catch
            {
                return View();
            }
        }

        bool isDefault(bool? field)
        {
            return field == false || field == null;
        }

        [HttpGet]
        public JsonResult Fetch(int ID)
        {
            var item = mod.Find(ID);
            var json = Json(item, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult Create()
        {
            var item = new tbl_JobOrder();
            item.JDate = DateTime.Now;
            item.JobDescription = tem.Find(1).Template;
            return View(item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(tbl_JobOrder m, bool isAuto = false)
        {
            int latestID = 0;
            if (ModelState.IsValid)
            {
                if (user.Level == 4)
                {
                    latestID = mod.Create(m);
                }
                else
                {
                    m.Approved = true;
                    m.ApprovedBy = user.User.ID;
                    m.ApprovedDate = DateTime.Now;
                    latestID = mod.Create(m);
                }
                if (isAuto)
                {
                    return RedirectToAction("Action", new { ID = latestID });
                }
                return RedirectToAction("Pending");
            }
            return View(m);
        }

        public async Task<ActionResult> Edit(int ID)
        {
            var item = mod.Find(ID);
            return View(await Task.Run(() => item));
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(tbl_JobOrder m)
        {
            if (ModelState.IsValid)
            {
                mod.Update(m);
                return RedirectToAction("Pending");
            }
            return View(m);
        }

        public ActionResult Action(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Action(tbl_JobOrder m)
        {
            ModelState.Remove("JobDescription");
            if (ModelState.IsValid)
            {
                mod.Action(m);
                return RedirectToAction("ActionUpdate", new { ID = m.ID });
            }
            return View(m);
        }

        public ActionResult EditAction(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditAction(tbl_JobOrder m)
        {
            ModelState.Remove("JobDescription");
            if (ModelState.IsValid)
            {
                mod.EditAction(m);
                return RedirectToAction("ActionUpdate", new { ID = m.ID });
            }
            return View(m);
        }

        public ActionResult ActionUpdate(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }

        public ActionResult AddStatus(int JOID)
        {
            mod2.JOID = JOID;
            mod2.ADate = DateTime.Now;
            return View(mod2);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddStatus(tbl_JobActionTaken m, string Status)
        {
            if (ModelState.IsValid)
            {
                mod2.Create(m);
                if (!string.IsNullOrEmpty(Status))
                {
                    mod.UpdateStatus(new tbl_JobOrder { ID = m.JOID, Status = Status });
                }
                return RedirectToAction("ActionUpdate", new { ID = m.JOID });
            }
            return View(m);
        }

        public ActionResult JOuserAllowed(int ID)
        {
            var item = new tbl_AllowedToUpdate();
            item.JOID = ID;
            return View(item);
        }

        [HttpPost]
        public ActionResult JOuserAllowed(tbl_AllowedToUpdate m)
        {
            if (ModelState.IsValid)
            {
                atu.Create(m);
                return RedirectToAction("ActionUpdate", new { ID = m.JOID });
            }
            return View(m);
        }

        public ActionResult JOuserRemove(int ID, int JOID)
        {
            atu.Delete(new tbl_AllowedToUpdate { ID = ID });
            return RedirectToAction("JOuserAllowed", new { ID = JOID });
        }

        public ActionResult EditStatus(int ID)
        {
            var item = mod2.Find(ID);
            return View(item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditStatus(tbl_JobActionTaken m, string Status)
        {
            if (ModelState.IsValid)
            {
                mod2.Update(m);
                if (!string.IsNullOrEmpty(Status))
                {
                    mod.UpdateStatus(new tbl_JobOrder { ID = m.JOID, Status = Status });
                }
                return RedirectToAction("ActionUpdate", new { ID = m.JOID });
            }
            return View(m);
        }

        public ActionResult Approvedrequest(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Approvedrequest(tbl_JobOrder m)
        {
            mod.ApprovedReq(m);
            return RedirectToAction("General");
        }

        public ActionResult Cancelrequest(int ID)
        {
            var item = mod.Find(ID);
            return View(item);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Cancelrequest(tbl_JobOrder m)
        {
            mod.CancelReq(m);
            return RedirectToAction("General");
        }

        public ActionResult Print(DateTime StartDate, DateTime EndDate, Status Status, Format Format = Format.PDF)
        {
            ReportViewer rv = new ReportViewer(); //EXCELOPENXML FOR EXCEL & application/vnd.ms-excel FOR contentType
            rv.LocalReport.ReportPath = Server.MapPath("~/Reports/ITSD.rdlc");
            var item = new JobOrderWithUpdates().List(StartDate, EndDate, Status);
            rv.LocalReport.DataSources.Add(new ReportDataSource("JobOrder", item));
            var file = rv.LocalReport.Render(Format == Format.PDF ? "pdf" : "EXCELOPENXML");
            Response.ContentType = Format == Format.PDF ? "application/pdf" : "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", Format == Format.PDF ? $"inline;filename=ITSD {DateTime.Now:yyyyMMddhhmmss}.pdf" : $"inline;filename=ITSD {DateTime.Now:yyyyMMddhhmmss}.xlsx");
            Response.Buffer = true;
            Response.Clear();
            Response.BinaryWrite(file);
            Response.End();
            return View(file);
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

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Delete(tbl_JobOrder m)
        {
            m.Delete(m);
            return RedirectToAction("Index");
        }


    }
}
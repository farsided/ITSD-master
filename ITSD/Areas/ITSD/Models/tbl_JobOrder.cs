using AAJdbController;
using ITSD.Areas.ITSD.Classes;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Areas.ITSD.Models
{
    public class tbl_JobOrder : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "#")]
        public Int32 ID { get; set; }
        public string IDDisplay { get { return $"{encDate:dd}{encDate:MM}{encDate:yy}-{ID:00}"; } }

        [Display(Name = "Job Title")]
        [Required]
        public String Title { get; set; }

        [Display(Name = "Company")]
        [Required]
        public String Company { get; set; }

        [Display(Name = "Job Description")]
        [DataType(DataType.Html)]
        [Required]
        public String JobDescription { get; set; }

        [Display(Name = "Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? JDate { get; set; }

        [Display(Name = "Requested By")]
        [Required]
        public String RequestedBy { get; set; }
        public tbl_user obj_RequestedBy { get; set; }

        [Display(Name = "Category")]
        [ScaffoldColumn(false)]
        public String Category { get; set; }

        [Display(Name = "SubCategory")]
        [ScaffoldColumn(false)]
        public String SubCategory { get; set; }

        [Display(Name = "Priority")]
        [ScaffoldColumn(false)]
        public Int32 Priority { get; set; }

        [Display(Name = "Priority")]
        public string PriorityDesc
        {
            get
            {
                switch (Priority)
                {
                    case 1:
                        return "Important/Urgent";
                    case 2:
                        return "Not Important/Urgent";
                    case 3:
                        return "Important/Not Urgent";
                    default:
                        return "Not Important/Not Urgent";
                }
            }
        }

        [Display(Name = "Status")]
        [ScaffoldColumn(false)]
        public String Status { get; set; }

        [Display(Name = "Action Taker")]
        [ScaffoldColumn(false)]
        public Int32 ATID { get; set; }
        public PMSRedirect.tbl_user ATIDInfo { get; set; }
        public DateTime? ATDate { get; set; }

        [Display(Name = "Approved")]
        [ScaffoldColumn(false)]
        public Boolean? Approved { get; set; }

        [Display(Name = "Approved By")]
        [ScaffoldColumn(false)]
        public Int32 ApprovedBy { get; set; }
        public PMSRedirect.tbl_user ApprovedInfo { get; set; }
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Cancel")]
        [ScaffoldColumn(false)]
        public Boolean? Cancel { get; set; }

        [Display(Name = "Cancel By")]
        [ScaffoldColumn(false)]
        public Int32 CancelBy { get; set; }
        public PMSRedirect.tbl_user CancelInfo { get; set; }
        public DateTime? CancelDate { get; set; }

        [Display(Name = "Encoded By")]
        [ScaffoldColumn(false)]
        public Int32 encBy { get; set; }
        public tbl_user encByInfo { get; set; }
        public string encByFullName { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public string Aging
        {
            get
            {
                string output = "";
                int days = DateTime.Now.Subtract(encDate.GetValueOrDefault()).Days;
                if (days <= 0)
                {
                    output = "New";
                }
                else
                {
                    output = $"{days} Day/s";
                }
                return output;
            }
        }

        public List<tbl_JobActionTaken> Actions { get; set; }
        static List<tbl_user> uList;
        public List<tbl_user> userList {
            get
            {
                return uList;
            }
            set
            {
            }
        }
        public tbl_JobOrder()
        {
        }
        public void InitStatusCount()
        {
            var actions = new tbl_JobActionTaken().List();
            uList = session.User.List();
            s.Query("tbl_JobOrder_List", p => { p.Add("@Search", $"%%"); p.Add("@encBy", 576); }, CommandType.StoredProcedure).ForEach(r =>
            {
                var item = new tbl_JobOrder(r);
                //item.obj_RequestedBy = userList.Find(f => f.Info?.AutoNo == item.RequestedBy);
                //item.Actions = actions.Where(f => f.JOID == item.ID).ToList();
                //item.ATIDInfo = userList.Find(f => f.ID == item.ATID);
                //item.ApprovedInfo = userList.Find(f => f.ID == item.ApprovedBy);
                //item.CancelInfo = userList.Find(f => f.ID == item.CancelBy);
                //item.encByInfo = userList.Find(f => f.ID == item.encBy);
                if (item.Status == "Pending" && (item.Cancel == false || item.Cancel == null))
                {
                    PendingCount++;
                }
                if (item.Status == "Done" && (item.Cancel == false || item.Cancel == null) && item.Approved == true)
                {
                    DoneCount++;
                }
                if (item.Status == "In progress" && (item.Cancel == false || item.Cancel == null) && item.Approved == true)
                {
                    ProgressCount++;
                }
            });

            s.Query("tbl_JobOrder_List", p => { p.Add("@Search", $"%%"); }, CommandType.StoredProcedure).ForEach(r =>
            {
                var item = new tbl_JobOrder(r);
                //item.obj_RequestedBy = userList.Find(f => f.Info?.AutoNo == item.RequestedBy);
                //item.Actions = actions.Where(f => f.JOID == item.ID).ToList();
                //item.ATIDInfo = userList.Find(f => f.ID == item.ATID);
                //item.ApprovedInfo = userList.Find(f => f.ID == item.ApprovedBy);
                //item.CancelInfo = userList.Find(f => f.ID == item.CancelBy);
                item.encByInfo = userList.Find(f => f.ID == item.encBy);
                if ((item.Cancel == false || item.Cancel == null) && item.Approved == true && (item.Status == "Pending" || item.Status == "In progress"))
                {
                    GeneralCount++;
                }
                if ((item.Cancel == false || item.Cancel == null) && (item.Approved == false || item.Approved == null) && item.encByInfo.Info.department == session.User.Info.department)
                {
                    ApprovalCount++;
                }
            });

        }
        public tbl_JobOrder(DataRow r) : base(r)
        {
        }

        public SelectList StatusList(string Except = "")
        {
            var items = new List<string> { "Pending", "In progress", "Done" };
            items.Remove(Except);
            var list = new SelectList(items);
            return list;
        }

        public SelectList CompanyList()
        {
            var items = new List<string> { "MHCI", "ISO", "SETS", "MYFC"};
            var list = new SelectList(items);
            return list;
        }

        /// <summary>
        /// Combine all EIS database using union all in sql 
        /// </summary>
        /// <returns></returns>
        public SelectList GeneralEIS()
        {
            var EISList = new List<object>();

            //release
            //s.Query("SELECT * FROM dbo.GeneralEIS()").ForEach(r =>
            //{
            //    EISList.Add(new { AutoNo = $"{r["AutoNo"]}", Fullname = $"{r["fname"]} {r["mn"]} {r["lname"]}" });
            //});

            //debug jose
            s.Query("SELECT AutoNo, fname, mn, lname FROM dbo.GeneralEIS()").ForEach(r =>
            {
                EISList.Add(new { AutoNo = $"{r["AutoNo"]}", Fullname = $"{r["fname"]} {r["mn"]} {r["lname"]}" });
            });

            //
            var list = new SelectList(EISList, "AutoNo", "Fullname");
            return list;
        }

        public int PendingCount { get; set; }
        public int DoneCount { get; set; }
        public int ProgressCount { get; set; }
        public int ApprovalCount { get; set; }
        public int GeneralCount { get; set; }
        public string EncByFullName { get; set; }

        public List<tbl_JobOrder> List(string Search = "", int encBy = 0)
        {
            var list = new List<tbl_JobOrder>();
            var actions = new tbl_JobActionTaken().List();
            s.Query("tbl_JobOrder_List", p => { p.Add("@Search", $"%{Search}%"); p.Add("@encBy", encBy); }, CommandType.StoredProcedure).ForEach(r =>
            {
                var item = new tbl_JobOrder(r);
                item.obj_RequestedBy = userList != null ? userList.Find(f => f.Info?.AutoNo == item.RequestedBy) : ( (userList = session.User.List()).Find(f => f.Info?.AutoNo == item.RequestedBy));
                item.Actions = actions.Where(f => f.JOID == item.ID).ToList();
                item.ATIDInfo = userList.Find(f => f.ID == item.ATID);
                item.ApprovedInfo = userList.Find(f => f.ID == item.ApprovedBy);
                item.CancelInfo = userList.Find(f => f.ID == item.CancelBy);
                item.encByInfo = userList.Find(f => f.ID == item.encBy);
                item.EncByFullName = userList.Find(f => f.ID == item.encBy) == null ? "NULL" : userList.Find(f => f.ID == item.encBy).Info.Fullname;
                list.Add(item);
            });
            actions = null;
            userList = null;

            return list;
        }
        public tbl_JobOrder Find(int ID)
        {
            var output = new tbl_JobOrder();
            var actions = new tbl_JobActionTaken().List();
            var listuser = session.User.List();
            s.Query("tbl_JobOrder_Find", p => p.Add("@ID", ID), CommandType.StoredProcedure).ForEach(r =>
            {
                var item = new tbl_JobOrder(r);
                item.obj_RequestedBy = listuser.Find(f => f.Info?.AutoNo == item.RequestedBy);
                item.Actions = actions.Where(f => f.JOID == item.ID).ToList();
                item.ATIDInfo = listuser.Find(f => f.ID == item.ATID);
                item.ApprovedInfo = listuser.Find(f => f.ID == item.ApprovedBy);
                item.CancelInfo = listuser.Find(f => f.ID == item.CancelBy);
                item.encByInfo = listuser.Find(f => f.ID == item.encBy);
                output = item;
            });
            return output;
        }
        public int Create(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return 0;
            }
            else
            {
                int latestID = 0;
                s.Query("tbl_JobOrder_Create", p =>
                {
                    p.Add("@Title", obj.Title);
                    p.Add("@Company", obj.Company);
                    p.Add("@JobDescription", obj.JobDescription);
                    p.Add("@JDate", obj.JDate);
                    p.Add("@RequestedBy", obj.RequestedBy);
                    p.Add("@ATID", obj.ATID);
                    p.Add("@ATDate", obj.ATDate);
                    p.Add("@Approved", obj.Approved);
                    p.Add("@ApprovedBy", obj.ApprovedBy);
                    p.Add("@ApprovedDate", obj.ApprovedDate);
                    p.Add("@encBy", session.User.ID);
                }, CommandType.StoredProcedure).ForEach(r =>
                {
                    latestID = Convert.ToInt32(r[0]);
                });
                return latestID;
            }
        }
        public void Update(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Query("tbl_JobOrder_Update", p =>
            {
                p.Add("@ID", obj.ID);
                p.Add("@Title", obj.Title);
                p.Add("@Company", obj.Company);
                p.Add("@JobDescription", obj.JobDescription);
                p.Add("@JDate", obj.JDate);
                p.Add("@RequestedBy", obj.RequestedBy);
            }, CommandType.StoredProcedure);
        }

        public void Action(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("tbl_JobOrder", obj.ID, p =>
            {
                p.Add("Category", obj.Category);
                p.Add("SubCategory", obj.SubCategory);
                p.Add("Priority", obj.Priority);
                p.Add("Status", "In progress");
                p.Add("ATID", session.User.ID);
                p.Add("ATDate", DateTime.Now);
            });
        }

        public void EditAction(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("tbl_JobOrder", obj.ID, p =>
            {
                p.Add("Category", obj.Category);
                p.Add("SubCategory", obj.SubCategory);
                p.Add("Priority", obj.Priority);
                p.Add("ATID", session.User.ID);
                p.Add("ATDate", DateTime.Now);
            });
        }

        public void UpdateStatus(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("tbl_JobOrder", obj.ID, p =>
            {
                p.Add("Status", obj.Status);
            });
        }

        public void ApprovedReq(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("tbl_JobOrder", obj.ID, p =>
            {
                p.Add("Approved", true);
                p.Add("ApprovedBy", session.User.ID);
                p.Add("ApprovedDate", DateTime.Now);
            });
        }

        public void CancelReq(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("tbl_JobOrder", obj.ID, p =>
            {
                p.Add("Status", "Cancel");
                p.Add("Cancel", true);
                p.Add("CancelBy", session.User.ID);
                p.Add("CancelDate", DateTime.Now);
            });
        }

        public void Delete(tbl_JobOrder obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Query("DELETE FROM [tbl_JobOrder] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


    public class JobOrderWithUpdates : tbl_JobOrder
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();

        public Int32 JOID { get; set; }

        public DateTime? ADate { get; set; }

        public String ActionTaken { get; set; }

        public int encByA { get; set; }

        public tbl_user encByAInfo { get; set; }
        public string Fullname { get { return $"{encByAInfo?.Info.fname} {encByAInfo?.Info.mn} {encByAInfo?.Info.lname}"; } }

        public DateTime? encADate { get; set; }

        public JobOrderWithUpdates()
        {

        }

        public JobOrderWithUpdates(DataRow r)
        {
            var type = this.GetType();
            var props = type.GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.CanWrite && r.Table.Columns.Contains(p.Name))
                {
                    var item = type.GetProperty(p.Name);
                    item.SetValue(this, Convert.IsDBNull(r[p.Name]) ? null : r[p.Name], null);
                }
            }
        }

        public List<JobOrderWithUpdates> List(DateTime StartDate, DateTime EndDate, Status Status)
        {
            var list = new List<JobOrderWithUpdates>();
            var status = "";
            switch (Status)
            {
                case Status.Ongoing:
                    status = "('In progress', 'Pending')";
                    break;
                case Status.DOne:
                    status = "('Done')";
                    break;
            }
            s.Query($"SELECT *, encADate = a.encDate, encByA = a.encBy FROM tbl_JobOrder jo LEFT JOIN tbl_JobActionTaken a ON jo.ID = a.JOID WHERE JDate BETWEEN @Start AND @End AND jo.[Status] IN {status}", p =>
            {
                p.Add("@Start", StartDate);
                p.Add("@End", EndDate);
               
            }).ForEach(r =>
            {
                var item = new JobOrderWithUpdates(r);
                item.encByAInfo = userList.Find(f => f.ID == item.encByA);
                list.Add(item);
            });
            return list;
        }
    }

    public enum Status
    {
        Ongoing,
        DOne
    }

    public enum Format
    {
       PDF,
       Excel
    }

}
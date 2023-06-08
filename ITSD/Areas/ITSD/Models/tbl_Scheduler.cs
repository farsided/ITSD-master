using AAJdbController;
using ITSD.Areas.ITSD.Classes;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace ITSD.Areas.ITSD.Models
{
    public class tbl_Scheduler : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "ID")]
        [ScaffoldColumn(false)]
        public Int32 ID { get; set; }

        [Display(Name = "Title")]
        [Required]
        public String Title { get; set; }

        [Display(Name = "Description")]
        [Required]
        public String Description { get; set; }

        [Display(Name = "Start")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        [DataType("datetime-local")]
        [Required]
        public DateTime? Start { get; set; }
        public string StartFormat { get { return $"{Start:yyyy-MM-ddThh:mm}"; } }

        [Display(Name = "End")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        [DataType("datetime-local")]
        [Required]
        public DateTime? End { get; set; }
        public string EndFormat { get { return $"{End:yyyy-MM-ddThh:mm}"; } }

        [Display(Name = "Cancel")]
        public Boolean? Cancel { get; set; }

        [Display(Name = "Active")]
        public Boolean? Active { get; set; }

        [Display(Name = "Encoded By")]
        [ScaffoldColumn(false)]
        public Int32 encBy { get; set; }
        public tbl_user obj_encBy { get; set; }

        [Display(Name = "Updated By")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdateDate { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public tbl_Scheduler()
        {

        }
        public tbl_Scheduler(DataRow r) : base(r)
        {

        }
        public List<tbl_Scheduler> List(string Search = "")
        {
            var list = new List<tbl_Scheduler>();
            var user = session.User.List();
            s.Query("SELECT * FROM [tbl_Scheduler] WHERE CONCAT(ID,Title,[Description],Start,[End],Cancel,Active,encBy,encDate) LIKE @Search", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_Scheduler(r);
                item.obj_encBy = user.Find(f => f.ID == item.encBy);
                list.Add(item);
            });
            return list;
        }

        public List<tbl_Scheduler> List(int EID, string Search = "")
        {
            var list = new List<tbl_Scheduler>();
            var user = session.User.List();
            s.Query("SELECT * FROM [tbl_Scheduler] WHERE encBy = @EID AND CONCAT(ID,Title,[Description],Start,[End],Cancel,Active,encBy,encDate) LIKE @Search", p => 
            {
                p.Add("@EID", EID);
                p.Add("@Search", $"%{Search}%");
            }).ForEach(r =>
            {
                var item = new tbl_Scheduler(r);
                item.obj_encBy = user.Find(f => f.ID == item.encBy);
                list.Add(item);
            });
            return list;
        }

        public tbl_Scheduler Find(int ID)
        {
            var output = new tbl_Scheduler();

            s.Query("SELECT * FROM [tbl_Scheduler] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_Scheduler(r);

                output = item;
            });
            return output;
        }
        public void Create(tbl_Scheduler obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Insert("[tbl_Scheduler]", p =>
            {
                p.Add("Title", obj.Title);
                p.Add("Description", obj.Description);
                p.Add("Start", obj.Start);
                p.Add("End", obj.End);
                p.Add("Cancel", obj.Cancel.GetValueOrDefault(false));
                p.Add("Active", true);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Update(tbl_Scheduler obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("[tbl_Scheduler]", obj.ID, p =>
            {
                p.Add("Title", obj.Title);
                p.Add("Description", obj.Description);
                p.Add("Start", obj.Start);
                p.Add("End", obj.End);
                p.Add("Cancel", obj.Cancel.GetValueOrDefault(false));
                p.Add("Active", obj.Active.GetValueOrDefault(false));
                p.Add("UpdateBy", session.User.ID);
                p.Add("UpdateDate", DateTime.Now);
            });
        }
        public void Delete(tbl_Scheduler obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Query("DELETE FROM [tbl_Scheduler] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }

        public enum ActiveStatus
        {
            No,
            Yes
        }
    }


}
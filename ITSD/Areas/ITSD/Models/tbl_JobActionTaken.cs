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
    public class tbl_JobActionTaken : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "ID")]
        [ScaffoldColumn(false)]
        public Int32 ID { get; set; }

        [Display(Name = "JOID")]
        public Int32 JOID { get; set; }

        [Display(Name = "Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? ADate { get; set; }

        [Display(Name = "Findings/Action Taken")]
        [DataType(DataType.Html)]
        [Required]
        public String ActionTaken { get; set; }

        [Display(Name = "Encoded By")]
        [ScaffoldColumn(false)]
        public Int32 encBy { get; set; }
        public tbl_user encByInfo { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public tbl_JobActionTaken()
        {
        }
        public tbl_JobActionTaken(DataRow r) : base(r)
        {
        }
        public List<tbl_JobActionTaken> List(string Search = "")
        {
            var list = new List<tbl_JobActionTaken>();
            var user = session.User.List();
            s.Query("SELECT * FROM [tbl_JobActionTaken] WHERE CONCAT(ID,JOID,ADate,ActionTaken,encBy,encDate) LIKE @Search ORDER BY ID DESC", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_JobActionTaken(r);
                item.encByInfo = user.Find(f => f.ID == item.encBy);
                list.Add(item);
            });

            user = null;
            return list;
        }
        public tbl_JobActionTaken Find(int ID)
        {
            var output = new tbl_JobActionTaken();
            var user = session.User.List();
            s.Query("SELECT * FROM [tbl_JobActionTaken] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_JobActionTaken(r);
                item.encByInfo = user.Find(f => f.ID == item.encBy);
                output = item;
            });
            return output;
        }
        public void Create(tbl_JobActionTaken obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Insert("[tbl_JobActionTaken]", p =>
            {
                p.Add("JOID", obj.JOID);
                p.Add("ADate", obj.ADate);
                p.Add("ActionTaken", obj.ActionTaken);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Update(tbl_JobActionTaken obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("[tbl_JobActionTaken]", obj.ID, p =>
            {
                p.Add("JOID", obj.JOID);
                p.Add("ADate", obj.ADate);
                p.Add("ActionTaken", obj.ActionTaken);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Delete(tbl_JobActionTaken obj)
        {
            s.Query("DELETE FROM [tbl_JobActionTaken] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
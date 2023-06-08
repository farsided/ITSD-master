using AAJdbController;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using ITSD.Classes;
using ITSD.Areas.AccountManagement.Classes;

namespace ITSD.Areas.AccountManagement.Models
{
    public class tbl_Accounts : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "ID")]
        public Int32 ID { get; set; }

        [Display(Name = "Username")]
        [Required]
        public String User { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        public String Pass { get; set; }

        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Status")]
        public String Status { get; set; }

        [Display(Name = "Owner")]
        public String Owner { get; set; }

        [Display(Name = "Category")]
        [Required]
        public String Category { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public String Remarks { get; set; }

        [Display(Name = "Encoded By")]
        public Int32 encBy { get; set; }
        public tbl_user obj_encBy { get; set; }

        [Display(Name = "Last Modified By")]
        public Int32 lastMod { get; set; }
        public tbl_user obj_lastMod { get; set; }

        [Display(Name = "Last Modified Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? lastModDate { get; set; }

        [Display(Name = "Encoded Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public tbl_Accounts()
        {
        }
        public tbl_Accounts(DataRow r) : base(r)
        {
        }
        public List<tbl_Accounts> List(string Search = "")
        {
            var list = new List<tbl_Accounts>();
            var _users = session.User?.List();

            s.Query("SELECT * FROM [tbl_Accounts] WHERE CONCAT(ID,User,Pass,Name,Status,Owner,Category,Remarks,encBy,lastMod,lastModDate,encDate) LIKE @Search", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_Accounts(r);
                item.obj_encBy = _users.Find(f => f.ID == item.encBy);
                item.obj_lastMod = _users.Find(f => f.ID == item.lastMod);

                list.Add(item);
            });
            return list;
        }
        public tbl_Accounts Find(int ID)
        {
            var output = new tbl_Accounts();
            var _users = session.User?.List();

            s.Query("SELECT * FROM [tbl_Accounts] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_Accounts(r);
                item.obj_encBy = _users.Find(f => f.ID == item.encBy);
                item.obj_lastMod = _users.Find(f => f.ID == item.lastMod);

                output = item;
            });
            return output;
        }
        public void Create(tbl_Accounts obj)
        {
            s.Insert("[tbl_Accounts]", p =>
            {
                p.Add("User", obj.User);
                p.Add("Pass", obj.Pass);
                p.Add("Name", obj.Name);
                p.Add("Status", obj.Status);
                p.Add("Owner", obj.Owner);
                p.Add("Category", obj.Category);
                p.Add("Remarks", obj.Remarks);
                p.Add("encBy", obj.encBy);
                p.Add("lastMod", session.User.ID);
                p.Add("lastModDate", DateTime.Now);
            });
        }
        public void Update(tbl_Accounts obj)
        {
            s.Update("[tbl_Accounts]", obj.ID, p =>
            {
                p.Add("User", obj.User);
                p.Add("Pass", obj.Pass);
                p.Add("Name", obj.Name);
                p.Add("Status", obj.Status);
                p.Add("Owner", obj.Owner);
                p.Add("Category", obj.Category);
                p.Add("Remarks", obj.Remarks);
                p.Add("lastMod", session.User.ID);
                p.Add("lastModDate", DateTime.Now);
            });
        }
        public void Delete(tbl_Accounts obj)
        {
            s.Query("DELETE FROM [tbl_Accounts] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
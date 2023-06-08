using AAJdbController;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITSD.Classes;
using ITSD.Areas.AccountManagement.Classes;

namespace ITSD.Areas.AccountManagement.Models
{
    public class tbl_AccCategory : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "ID")]
        public Int32 ID { get; set; }

        [Display(Name = "Name")]
        [Required]
        public String Name { get; set; }

        [Display(Name = "Encoded By")]
        [ScaffoldColumn(false)]
        public Int32 encBy { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public tbl_AccCategory()
        {
        }
        public tbl_AccCategory(DataRow r) : base(r)
        {
        }
        public List<tbl_AccCategory> List(string Search = "")
        {
            var list = new List<tbl_AccCategory>();

            s.Query("SELECT * FROM [tbl_AccCategory] WHERE CONCAT(ID,Name,encBy,encDate) LIKE @Search", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_AccCategory(r);

                list.Add(item);
            });
            return list;
        }

        public SelectList ListAccCategory()
        {
            var list = new SelectList(List(), "Name", "Name");
            return list;
        }

        public tbl_AccCategory Find(int ID)
        {
            var output = new tbl_AccCategory();

            s.Query("SELECT * FROM [tbl_AccCategory] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_AccCategory(r);

                output = item;
            });
            return output;
        }
        public void Create(tbl_AccCategory obj)
        {
            s.Insert("[tbl_AccCategory]", p =>
            {
                p.Add("Name", obj.Name);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Update(tbl_AccCategory obj)
        {
            s.Update("[tbl_AccCategory]", obj.ID, p =>
            {
                p.Add("Name", obj.Name);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Delete(tbl_AccCategory obj)
        {
            s.Query("DELETE FROM [tbl_AccCategory] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
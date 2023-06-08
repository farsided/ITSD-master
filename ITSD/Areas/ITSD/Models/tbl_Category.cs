using AAJdbController;
using ITSD.Areas.ITSD.Classes;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Areas.ITSD.Models
{
    public class tbl_Category : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "ID")]
        [ScaffoldColumn(false)]
        public Int32 ID { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Encoded By")]
        [ScaffoldColumn(false)]
        public Int32 encBy { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public tbl_Category()
        {
        }
        public tbl_Category(DataRow r) : base(r)
        {
        }
        public virtual List<tbl_Category> List(string Search = "")
        {
            var list = new List<tbl_Category>();

            s.Query("SELECT * FROM [tbl_Category] WHERE CONCAT(ID,Description,encBy,encDate) LIKE @Search", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_Category(r);

                list.Add(item);
            });
            return list;
        }

        public virtual SelectList SelectCat()
        {
            var list = new SelectList(List(), "Description", "Description");
            return list;
        }

        public virtual tbl_Category Find(int ID)
        {
            var output = new tbl_Category();

            s.Query("SELECT * FROM [tbl_Category] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_Category(r);

                output = item;
            });
            return output;
        }
        public virtual void Create(tbl_Category obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Insert("[tbl_Category]", p =>
            {
                p.Add("Description", obj.Description);
                p.Add("encBy", session.User.ID);
            });
        }
        public virtual void Update(tbl_Category obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("[tbl_Category]", obj.ID, p =>
            {
                p.Add("Description", obj.Description);
                p.Add("encBy", session.User.ID);
            });
        }
        public virtual void Delete(tbl_Category obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Query("DELETE FROM [tbl_Category] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
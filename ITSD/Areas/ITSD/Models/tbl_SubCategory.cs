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
    public class tbl_SubCategory : AAJ
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

        public tbl_SubCategory()
        {
        }
        public tbl_SubCategory(DataRow r) : base(r)
        {
        }
        public List<tbl_SubCategory> List(string Search = "")
        {
            var list = new List<tbl_SubCategory>();

            s.Query("SELECT * FROM [tbl_SubCategory] WHERE CONCAT(ID,Description,encBy,encDate) LIKE @Search ORDER BY Description", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_SubCategory(r);

                list.Add(item);
            });
            return list;
        }

        public virtual SelectList SelectCat()
        {
            var list = new SelectList(List(), "Description", "Description");
            return list;
        }

        public tbl_SubCategory Find(int ID)
        {
            var output = new tbl_SubCategory();

            s.Query("SELECT * FROM [tbl_SubCategory] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_SubCategory(r);

                output = item;
            });
            return output;
        }
        public void Create(tbl_SubCategory obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Insert("[tbl_SubCategory]", p =>
            {
                p.Add("Description", obj.Description);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Update(tbl_SubCategory obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("[tbl_SubCategory]", obj.ID, p =>
            {
                p.Add("Description", obj.Description);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Delete(tbl_SubCategory obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Query("DELETE FROM [tbl_SubCategory] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
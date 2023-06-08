using AAJdbController;
using ITSD.Areas.ITSD.Classes;
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace ITSD.Areas.LMS.Models
{
    public class tbl_LMS : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "ID")]
        public Int32 ID { get; set; }

        [Display(Name = "Software")]
        [Required]
        public String Software { get; set; }

        [Display(Name = "License/Key")]
        [Required]
        public String Key { get; set; }

        [Display(Name = "Bincard")]
        [Required]
        public String Reference { get; set; }

        [Display(Name = "Remarks")]
        public String Remarks { get; set; }

        [Display(Name = "Encoded By")]
        [ScaffoldColumn(false)]
        public Int32 encBy { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public tbl_LMS()
        {
        }
        public tbl_LMS(DataRow r) : base(r)
        {
        }
        public List<tbl_LMS> List(string Search = "")
        {
            var list = new List<tbl_LMS>();

            s.Query("SELECT * FROM [tbl_LMS] WHERE CONCAT(ID,Software,[Key],Reference,Remarks,encBy,encDate) LIKE @Search", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_LMS(r);

                list.Add(item);
            });
            return list;
        }
        public tbl_LMS Find(int ID)
        {
            var output = new tbl_LMS();

            s.Query("SELECT * FROM [tbl_LMS] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_LMS(r);

                output = item;
            });
            return output;
        }
        public void Create(tbl_LMS obj)
        {
            s.Insert("[tbl_LMS]", p =>
            {
                p.Add("Software", obj.Software);
                p.Add("Key", obj.Key);
                p.Add("Reference", obj.Reference);
                p.Add("Remarks", obj.Remarks);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Update(tbl_LMS obj)
        {
            s.Update("[tbl_LMS]", obj.ID, p =>
            {
                p.Add("Software", obj.Software);
                p.Add("Key", obj.Key);
                p.Add("Reference", obj.Reference);
                p.Add("Remarks", obj.Remarks);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Delete(tbl_LMS obj)
        {
            s.Query("DELETE FROM [tbl_LMS] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
using AAJdbController;
using ITSD.Areas.ITSD.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace ITSD.Areas.ITSD.Models
{
    public class tbl_Template : AAJ
    {
        dbcontrol s = new dbcontrol();
        [Display(Name = "ID")]
        [ScaffoldColumn(false)]
        public Int32 ID { get; set; }

        [Display(Name = "Template")]
        public String Template { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public tbl_Template()
        {
        }
        public tbl_Template(DataRow r) : base(r)
        {
        }
        public List<tbl_Template> List(string Search = "")
        {
            var list = new List<tbl_Template>();

            s.Query("SELECT * FROM [tbl_Template] WHERE CONCAT(ID,Template,encDate) LIKE @Search", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_Template(r);

                list.Add(item);
            });
            return list;
        }
        public tbl_Template Find(int ID)
        {
            var output = new tbl_Template();

            s.Query("SELECT * FROM [tbl_Template] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_Template(r);

                output = item;
            });
            return output;
        }
        public void Create(tbl_Template obj)
        {
            s.Insert("[tbl_Template]", p =>
            {
                p.Add("Template", obj.Template);
            });
        }
        public void Update(tbl_Template obj)
        {
            s.Update("[tbl_Template]", obj.ID, p =>
            {
                p.Add("Template", obj.Template);
            });
        }
        public void Delete(tbl_Template obj)
        {
            s.Query("DELETE FROM [tbl_Template] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
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
    public class tbl_AllowedToUpdate : AAJ
    {
        dbcontrol s = new dbcontrol();
        UserSessions session = new UserSessions();
        [Display(Name = "ID")]
        [ScaffoldColumn(false)]
        public Int32 ID { get; set; }

        [Display(Name = "JOID")]
        public Int32 JOID { get; set; }

        [Display(Name = "userID")]
        public Int32 userID { get; set; }
        public tbl_user UserInfo { get; set; }

        [Display(Name = "Encoded By")]
        public Int32 encBy { get; set; }

        [Display(Name = "Encoded Date")]
        [ScaffoldColumn(false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime? encDate { get; set; }

        public class UserFullname
        {
            public int ID { get; set; }
            public string Fullname { get; set; }
            public List<UserFullname> ListwithFullname()
            {
                var list = new List<UserFullname>();
                new UserSessions().User.List().ForEach(r =>
                {
                    if (r.Info != null)
                    {
                        list.Add(new UserFullname
                        {
                            ID = r.ID,
                            Fullname = r.Info?.Fullname
                        });
                    }
                });
                return list;
            }
        }

        public tbl_AllowedToUpdate()
        {
        }
        public tbl_AllowedToUpdate(DataRow r) : base(r)
        {
        }
        public List<tbl_AllowedToUpdate> List(string Search = "")
        {
            var list = new List<tbl_AllowedToUpdate>();
            var user = session.User.List();
            s.Query("SELECT * FROM [tbl_AllowedToUpdate] WHERE CONCAT(ID,JOID,userID,encBy,encDate) LIKE @Search", p => p.Add("@Search", $"%{Search}%")).ForEach(r =>
            {
                var item = new tbl_AllowedToUpdate(r);
                item.UserInfo = user.Find(f => f.ID == item.userID);
                list.Add(item);
            });
            return list;
        }

        public List<tbl_AllowedToUpdate> ListUserJO(int JOID)
        {
            var list = new List<tbl_AllowedToUpdate>();
            var user = session.User.List();
            s.Query("SELECT * FROM [tbl_AllowedToUpdate] WHERE JOID = @ID", p => p.Add("@ID", JOID)).ForEach(r =>
            {
                var item = new tbl_AllowedToUpdate(r);
                item.UserInfo = user.Find(f => f.ID == item.userID);
                list.Add(item);
            });
            return list;
        }

        public tbl_AllowedToUpdate Find(int ID)
        {
            var output = new tbl_AllowedToUpdate();
            var user = session.User.List();
            s.Query("SELECT * FROM [tbl_AllowedToUpdate] WHERE ID = @ID", p => p.Add("@ID", ID)).ForEach(r =>
            {
                var item = new tbl_AllowedToUpdate(r);
                item.UserInfo = user.Find(f => f.ID == item.userID);
                output = item;
            });
            return output;
        }
        public void Create(tbl_AllowedToUpdate obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Insert("[tbl_AllowedToUpdate]", p =>
            {
                p.Add("JOID", obj.JOID);
                p.Add("userID", obj.userID);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Update(tbl_AllowedToUpdate obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Update("[tbl_AllowedToUpdate]", obj.ID, p =>
            {
                p.Add("JOID", obj.JOID);
                p.Add("userID", obj.userID);
                p.Add("encBy", session.User.ID);
            });
        }
        public void Delete(tbl_AllowedToUpdate obj)
        {
            if (!session.HasSession)
            {
                return;
            }
            s.Query("DELETE FROM [tbl_AllowedToUpdate] WHERE ID = @ID", p =>
            {
                p.Add("@ID", obj.ID);
            });
        }
    }


}
using PMSRedirect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Controllers
{
    public class HomeController : Controller
    {
        UserSessions user = new UserSessions();
        public ActionResult Index()
        {
            //debug
            InitAdmin();
            return View();
        }

        void InitAdmin()
        {

            user = new UserSessions(@"SERVER=JOSEITSD;DATABASE=dbPMS;User=sa;pwd=1234");
            //user = new UserSessions(@"SERVER=JOSEITSD;DATABASE=dbPMS;User=sa;pwd=1234");
            user.InitializeAdmin(477);
        }

        public ActionResult Redirect()
        {
            user.Initialize(Company.General);
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            user.DestroySession();
            return Redirect("/");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
using System.Web.Mvc;

namespace ITSD.Areas.ITSD
{
    public class ITSDAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ITSD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ITSD_default",
                "ITSD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
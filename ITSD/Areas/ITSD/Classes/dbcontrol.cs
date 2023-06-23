using ITSD.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITSD.Areas.ITSD.Classes
{
    public class dbcontrol : MasterControl
    {
        public dbcontrol() : base("ITSD")
        {
            ErrorOccured += Dbcontrol_ErrorOccured;
        }

        private void Dbcontrol_ErrorOccured(AAJdbController.ErrorMessage e)
        {
            throw new Exception(e.ExceptionMessage);
        }
    }

    public class Authothorized : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }

    }
}
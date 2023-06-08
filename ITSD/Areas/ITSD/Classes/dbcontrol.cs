using ITSD.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}
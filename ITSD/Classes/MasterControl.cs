using AAJdbController;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ITSD.Classes
{
    public class MasterControl : AAJControl
    {
        public MasterControl()
        {

        }

        public MasterControl(string ConnectionStringName) : base(DatabaseType.SQLServer, ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString)
        {

        }
    }
}
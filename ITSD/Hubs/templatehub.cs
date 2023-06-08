using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ITSD.Hubs
{
    [HubName("TemplateHub")]
    public class TemplateHub : Hub
    {
        public static void BroadCastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TemplateHub>();
            context.Clients.All.refreshEmployeeData();
        }
    }
}
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using VectorArenaWebRole;

namespace VectorArenaWebRole
{
    public class Global : HttpApplication
    {
        public Game Game;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteTable.Routes.MapHubs();
            Game = new Game();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}

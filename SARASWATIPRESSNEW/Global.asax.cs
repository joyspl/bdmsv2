using SARASWATIPRESSNEW.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SARASWATIPRESSNEW
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
        }
        //void Application_Error(object sender, EventArgs args)
        //{
        //    BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        //    Exception e = Server.GetLastError(); // gets the thrown exception
        //    try
        //    {               
        //        objDbTrx.SaveSystemErrorLog(e, Request.UserHostAddress);
        //    }
        //    catch { }

        //}

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void Session_Start()
        {
            HttpContext.Current.Session.Add("UserSec", "");       
            HttpContext.Current.Session.Add("ReqSessionCode", "");
            HttpContext.Current.Session.Add("SchooldCode", "");
            HttpContext.Current.Session.Add("TopLimit", "");

            HttpContext.Current.Session.Add("BDMSLoginType", "");    
            
        }
    }
}
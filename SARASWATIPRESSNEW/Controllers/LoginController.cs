using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class LoginController : Controller
    {
       
        public ActionResult Index()
        {
            //Response.Redirect("/wbtextbookcorporation.org");
            Session.Abandon();
            //return Redirect("http://wbtextbookcorporation.org");
            return View();
        }

    }
}

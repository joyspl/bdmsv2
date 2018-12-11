using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class TbLogoutController : Controller
    {
        //
        // GET: /TbLogout/

        public ActionResult Index()
        {
            Session.Abandon();
            return View();
            //return View("~/Views/CircleLogin/Index.cshtml");
        }

    }
}

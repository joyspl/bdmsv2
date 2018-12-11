using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SARASWATIPRESSNEW.Models;
using System.Collections;
using SARASWATIPRESSNEW.BusinessLogicLayer;

namespace SARASWATIPRESSNEW.Controllers
{
    public class TBLoginController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TBLogin objcust)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DataTable dt = objDbTrx.GetSPLoginDtl(objcust.sp_user_name, objcust.sp_password);
                    if (dt.Rows.Count > 0)
                    {
                        Session["sp_name"] = dt.Rows[0]["SP_Name"].ToString();
                        Session["sp_user_name"] = dt.Rows[0]["sp_user_name"].ToString();
                        return RedirectToAction("Challan", "Home");
                    }
                }
                catch (Exception ex)
                {
                    objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
                }
                finally
                {
                }
            }
            return View();
        }

    }
}

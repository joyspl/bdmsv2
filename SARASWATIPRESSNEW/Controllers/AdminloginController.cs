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

namespace SARASWATIPRESSNEW.Controllers
{
    public class AdminloginController : Controller
    {
        //

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Admin objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("select * from admin_login where USER_NAME=@USER_NAME and PASSWORD=@PASSWORD", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@USER_NAME", objcust.admin_user_name);
                    cmd.Parameters.AddWithValue("@PASSWORD", objcust.admin_password);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt_login = new DataTable();
                    da.Fill(dt_login);
                    if (dt_login.Rows.Count > 0)
                    {
//ADMIN
//CIRCLE_LOGIN
//TB_LOGIN
//DIRECTOR_LOGIN
//DISTRICT_LOGIN
                        Session["BDMSLoginType"] = "ADMIN";
                        return RedirectToAction("Index", "HomePage");                        
                    }
                }
                catch (Exception ex)
                {
                    return View();
                }
                finally
                {
                    con.Close();
                }

            }
            return View();
        }

    }
}

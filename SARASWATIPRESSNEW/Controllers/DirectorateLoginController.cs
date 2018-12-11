using System;
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
    public class DirectorateLoginController : Controller
    {
        //
        // GET: /DirectorateLogin/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Directorate objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("select * from directoratelogin where user_name=@user_name and password=@password", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_name", objcust.directorate_user_name);
                    cmd.Parameters.AddWithValue("@password", objcust.directorate_password);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt_login = new DataTable();
                    da.Fill(dt_login);
                    if (dt_login.Rows.Count > 0)
                    {
                        return RedirectToAction("Index", "DirectorateCircleWiseSchoolReport");
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

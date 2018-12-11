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
    public class DistrictLoginController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(DistrictLogin objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("select district_login.*,district_master.district from district_login inner join district_master on district_login.district_id=district_master.id  where district_login.user_name=@user_name and district_login.PASSWORD=@PASSWORD ", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_name", objcust.district_user_name);
                    cmd.Parameters.AddWithValue("@PASSWORD", objcust.district_password);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Session["district_id"] = rdr["district_id"].ToString();
                        Session["district_name"] = rdr["district"].ToString();
                        Session["district_user_id"] = rdr["ID"].ToString();
                        return RedirectToAction("Index", "DistrictCircleWiseRequisitionStock");                        
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

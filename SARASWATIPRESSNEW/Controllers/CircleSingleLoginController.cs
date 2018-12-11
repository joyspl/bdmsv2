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
    public class CircleSingleLoginController : Controller
    {
       
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(CircleSingleLogin objcust)
        {
            SqlConnection con = null;
            string result = "";
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
            con.Open();
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("select * from circle_user_master where upper(USER_ID)='" + objcust.circle_user_name.ToString().Trim().ToUpper() + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    SqlCommand cmd1 = new SqlCommand("update circle_user_master set flag=FALSE where upper(USER_ID)='" + objcust.circle_user_name.ToString().Trim().ToUpper() + "'", con);
                    cmd1.CommandType = CommandType.Text;
                    result = cmd1.ExecuteNonQuery().ToString();
                    TempData["Message"] = "Circle Login Permission is enabled successfully...";
                }
                else
                {
                    TempData["Message"] = "No Data Found.....";
                }
                con.Close();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return View();
        }

    }
}

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
    public class CircleController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Circle objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                string result = "";
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("insert into circle_master (CIRCLE_CODE,CIRCLE_NAME,DISTRICT_ID) values (@CIRCLE_CODE,@CIRCLE_NAME,@DISTRICT_ID)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CIRCLE_CODE", objcust.Circle_code);
                    cmd.Parameters.AddWithValue("@CIRCLE_NAME", objcust.Circle_name);
                    cmd.Parameters.AddWithValue("@DISTRICT_ID", objcust.district_id);
                    con.Open();
                    result = cmd.ExecuteReader().ToString();
                    Response.Write("<script> alert ('Data has been submitted successfully...') </script> ");
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

            return RedirectToAction("Index", "CircleView");
        }

    }
}

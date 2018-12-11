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
    public class CircleUserController : Controller
    {

        public ActionResult Index()
        {
            return View(get_circle());
        }

        [HttpPost]
        public ActionResult Index(CircleUser objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                string result = "";
                try
                {
                    
                        con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                        SqlCommand cmd = new SqlCommand("insert into circle_user_master (CIRCLE_ID,CIRCLE_OFFICER_NAME,MOBILE_NO,EMAIL_ID,CIRCLE_ADDRESS,USER_ID,PASSWORD) values (@CIRCLE_ID,@CIRCLE_OFFICER_NAME,@MOBILE_NO,@EMAIL_ID,@CIRCLE_ADDRESS,@USER_ID,@PASSWORD)", con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CIRCLE_ID", objcust.CircleID);
                        cmd.Parameters.AddWithValue("@CIRCLE_OFFICER_NAME", objcust.CircleOfficerName);
                        cmd.Parameters.AddWithValue("@MOBILE_NO", objcust.MobileNo);
                        cmd.Parameters.AddWithValue("@EMAIL_ID", objcust.EmailId);
                        cmd.Parameters.AddWithValue("@CIRCLE_ADDRESS", objcust.Address);
                        cmd.Parameters.AddWithValue("@USER_ID", objcust.Userid);
                        cmd.Parameters.AddWithValue("@PASSWORD", objcust.Password);
                        con.Open();
                        result = cmd.ExecuteReader().ToString();
                   
                    
                    Response.Write("<script> alert ('Data has been submitted successfully...') </script> ");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }

            }

            return RedirectToAction("Index", "CircleUserView");
        }

        [HttpGet]
        public Models.CircleUser get_circle()
        {
            CircleUser cu = new CircleUser();
            List<Circle> lst_circle = new List<Circle>();
            try
            {
                SqlConnection con = null;
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select * from circle_master", con);
                cmd1.CommandType = CommandType.Text;
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                while (rdr1.Read())
                {
                    Circle rq = new Circle();
                    rq.Circle_name = Convert.ToString(rdr1["CIRCLE_NAME"].ToString());
                    rq.CircleID = Convert.ToInt32(rdr1["ID"].ToString());
                    lst_circle.Add(rq);
                }
                cu.CircleCollection = lst_circle;
                rdr1.Close();
                con.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return cu;
        }

    }
}

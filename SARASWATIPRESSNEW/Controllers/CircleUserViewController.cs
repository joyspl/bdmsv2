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
    public class CircleUserViewController : Controller
    {
        
         [HttpGet]
        public ActionResult Index()
        {
            List<CircleUser> lst_rq = new List<CircleUser>();
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from circle_user_master ", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CircleUser rq = new CircleUser();
                    rq.CircleID = Convert.ToInt32(Convert.ToString(rdr["CIRCLE_ID"]));
                    rq.CircleOfficerName = Convert.ToString(rdr["CIRCLE_OFFICER_NAME"]);
                    rq.MobileNo = Convert.ToString(rdr["MOBILE_NO"]);
                    rq.EmailId = Convert.ToString(rdr["EMAIL_ID"]);
                    rq.Address = Convert.ToString(rdr["CIRCLE_ADDRESS"]);
                    rq.Userid = Convert.ToString(rdr["USER_ID"]);
                    rq.Password = Convert.ToString(rdr["PASSWORD"]);
                    lst_rq.Add(rq);
                }
                rdr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(lst_rq);
        }

    }
}

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
    public class DistrictLogoutController : Controller
    {
        
        public ActionResult Index()
        {
            SqlConnection con = null;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
            SqlCommand cmddelete = new SqlCommand("delete from report_requisition_stock where district_id=" + Convert.ToInt32(Session["district_id"].ToString()) + "", con);
            cmddelete.CommandType = CommandType.Text;
            con.Open();
            cmddelete.ExecuteNonQuery();
            Session.Abandon();
            return View();
        }

    }
}

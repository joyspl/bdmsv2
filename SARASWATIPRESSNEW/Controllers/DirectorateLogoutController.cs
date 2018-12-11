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
    public class DirectorateLogoutController : Controller
    {

        public ActionResult Index()
        {
            SqlConnection con = null;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
            SqlCommand cmddelete = new SqlCommand("truncate table report_requisition_stock", con);
            cmddelete.CommandType = CommandType.Text;
            con.Open();
            cmddelete.ExecuteNonQuery();
            Session.Abandon();
            return View();
        }

    }
}

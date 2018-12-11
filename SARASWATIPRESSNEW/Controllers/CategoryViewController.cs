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
    public class CategoryViewController : Controller
    {
       [HttpGet]
        public ActionResult Index()
        {
            List<Category> lst_rq = new List<Category>();
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from book_category_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Category rq = new Category();
                    rq.Category_name = Convert.ToString(rdr["BOOK_CATEGORY"].ToString());                    
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
       protected override void OnActionExecuting(ActionExecutingContext filterContext)
       {
           HttpSessionStateBase session = filterContext.HttpContext.Session;
           if (session.IsNewSession || Session["UserSec"] == null)
           {
               filterContext.Result = new RedirectResult("/SessionExpire/Index");
               return;
           }
           base.OnActionExecuting(filterContext);
       }
    }
}

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
    public class CategoryController : Controller
    {
        // TEST BY SA
        public ActionResult Category()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Category(Category objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                string result = "";
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("insert into book_category_master (BOOK_CATEGORY) values (@category)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@category", objcust.Category_name);                    
                    con.Open();
                    result = cmd.ExecuteReader().ToString();
                    Response.Write("<script> alert ('Data has been submitted successfully...') </script> ");
                }
                catch(Exception ex)
                {
                    return View();
                }
                finally
                {
                    con.Close();
                }
               
            }

            return RedirectToAction("Index", "CategoryView"); 
        }
        
    }
}

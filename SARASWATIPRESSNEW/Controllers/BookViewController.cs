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
    public class BookViewController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<Book> lst_rq = new List<Book>();
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select book_master.*,book_category_master.BOOK_CATEGORY,language_master.LANGUAGE from book_category_master inner join book_master on book_master.CATEGORY_ID=book_category_master.ID INNER JOIN language_master ON book_master.LANGUAGE_ID=language_master.ID", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Book rq = new Book();
                    rq.BookName = Convert.ToString(rdr["BOOK_NAME"].ToString());
                  //  rq.CategoryName = Convert.ToString(rdr["BOOK_CATEGORY"].ToString());
                    rq.LanguageName = Convert.ToString(rdr["LANGUAGE"].ToString());
                    rq.BookCode = Convert.ToString(rdr["BOOK_CODE"].ToString());
                //    rq.unitprice = Convert.ToDecimal(rdr["UNIT_PRICE"].ToString());
                  //  rq.rate = Convert.ToDecimal(rdr["RATE"].ToString());
                    rq.quantity = Convert.ToInt32(rdr["QUANTITY"].ToString());
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

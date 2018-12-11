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
    public class BookMasterController : Controller
    {
       
        public ActionResult Index()
        {            
            return View(get_language());
        }

        [HttpPost]
        public ActionResult Index(Book objcust)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = null;
                string result = "";
                try
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                    SqlCommand cmd = new SqlCommand("insert into book_master (BOOK_CODE,BOOK_NAME,RATE,CATEGORY_ID,LANGUAGE_ID,QUANTITY,UNIT_PRICE) values (@bookcode,@bookname,@rate,@categoryID,@languageid,@qty,@price)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@bookcode", objcust.BookCode);
                    cmd.Parameters.AddWithValue("@bookname", objcust.BookName);
                    cmd.Parameters.AddWithValue("@rate", objcust.rate);
                    cmd.Parameters.AddWithValue("@categoryID", objcust.CategoryID);
                    cmd.Parameters.AddWithValue("@languageid", objcust.LanguageID);
                    cmd.Parameters.AddWithValue("@price", objcust.unitprice);
                    cmd.Parameters.AddWithValue("@qty", objcust.quantity);
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
                //List<Requisition> lst_rq = new List<Requisition>();

            }

            return RedirectToAction("Index", "BookView"); ;            
        }

        [HttpGet]
        public Models.Book get_language()
        {
            Book lst_req = new Book();
            List<Language> lst_language = new List<Language>();
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());
                SqlCommand cmd = new SqlCommand("select * from language_master", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                int C = 0;
                while (rdr.Read())
                {
                    Language rq = new Language();
                    rq.language_name = Convert.ToString(rdr["LANGUAGE"].ToString());
                    rq.LanguageID = Convert.ToInt32(rdr["ID"].ToString());
                    lst_language.Add(rq);
                }
                lst_req.languageCollection = lst_language;
                rdr.Close();
                con.Close();

                List<Category> lst_category = new List<Category>();
                try
                {
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("select * from book_category_master", con);
                    cmd1.CommandType = CommandType.Text;
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    while (rdr1.Read())
                    {
                        Category rq = new Category();
                        rq.Category_name = Convert.ToString(rdr1["BOOK_CATEGORY"].ToString());
                        rq.CategoryID = Convert.ToInt32(rdr1["ID"].ToString());
                        lst_category.Add(rq);
                    }
                    lst_req.categoryCollection = lst_category;
                    rdr1.Close();
                    con.Close();

                    lst_req.CategoryID = 1;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lst_req;
        }

    }
}

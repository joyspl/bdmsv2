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
using SARASWATIPRESSNEW.BusinessLogicLayer;

namespace SARASWATIPRESSNEW.Controllers
{
    public class BookWiseStockReportCircleController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View(get_dropdown());
        }

        [HttpPost]
        public ActionResult Index(BookWiseStockreportCircle objcust)
        {
            BookWiseStockreportCircle result = new BookWiseStockreportCircle();
            if (ModelState.IsValid)
            {               
               result= get_report(objcust.CircleID);
            }
            return View(result);         
        }

        
        public Models.BookWiseStockreportCircle get_report(int var)
        {

            BookWiseStockreportCircle lst_req = new BookWiseStockreportCircle();
            SqlConnection con = null;
            List<BookWiseStockreportCircle> lst_req1 = new List<BookWiseStockreportCircle>();

            try
            {
                DataTable dtMastData = objDbTrx.GetBookWiseReqStokDetails(Convert.ToInt64(var.ToString())) ;
                if (dtMastData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMastData.Rows.Count; i++)
                    {
                        BookWiseStockreportCircle rq = new BookWiseStockreportCircle();
                        rq.language = Convert.ToString(dtMastData.Rows[i]["language"].ToString());
                        rq.Category = Convert.ToString(dtMastData.Rows[i]["BOOK_CATEGORY"].ToString());
                        rq.BookName = Convert.ToString(dtMastData.Rows[i]["book_name"].ToString());
                        rq.stock_quantity = "0";
                        if (dtMastData.Rows[i]["stock_total"].ToString().Trim() != "")
                        {
                            rq.stock_quantity = Convert.ToString(dtMastData.Rows[i]["stock_total"].ToString());
                        }
                        rq.req_quantity = "0";
                        if (dtMastData.Rows[i]["total"].ToString().Trim() != "")
                        {
                            rq.req_quantity = Convert.ToString(dtMastData.Rows[i]["total"].ToString());
                        }

                        rq.BookCode = Convert.ToString(dtMastData.Rows[i]["book_code"].ToString());
                        rq.remaining_quantity = Convert.ToString(Convert.ToInt32(rq.req_quantity) - Convert.ToInt32(rq.stock_quantity));
                        lst_req1.Add(rq);
                    }
                }
                lst_req.bookwise_collection = lst_req1;     

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            List<Circle> lst_req2 = new List<Circle>();
            try
            {

                DataTable dtMastData = objDbTrx.GetCircleMasterDetails();
                if (dtMastData.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMastData.Rows.Count; iCnt++)
                    {
                        Circle rq = new Circle();
                        rq.CircleID = Convert.ToInt32(dtMastData.Rows[iCnt]["ID"].ToString());
                        rq.Circle_name = Convert.ToString(dtMastData.Rows[iCnt]["CIRCLE_NAME"].ToString());
                        lst_req2.Add(rq);
                    }
                    dtMastData.Dispose();
                }
                lst_req.circle_collection = lst_req2;
                lst_req.CircleID = var;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lst_req;
        }

         [HttpGet]
        public Models.BookWiseStockreportCircle get_dropdown()
        {
            BookWiseStockreportCircle lst_req = new BookWiseStockreportCircle();
            List<Circle> lst_req1 = new List<Circle>();

            try
            {
                DataTable dtMastData = objDbTrx.GetCircleMasterDetails();
                if (dtMastData.Rows.Count > 0)
                {
                    for (int iCnt = 0; iCnt < dtMastData.Rows.Count; iCnt++)
                    {
                        Circle rq = new Circle();
                        rq.CircleID = Convert.ToInt32(dtMastData.Rows[iCnt]["ID"].ToString());
                        rq.Circle_name = Convert.ToString(dtMastData.Rows[iCnt]["CIRCLE_NAME"].ToString());
                        lst_req1.Add(rq);
                    }
                    dtMastData.Dispose();
                }
                lst_req.circle_collection = lst_req1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return lst_req;
        }

    }
}

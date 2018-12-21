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
    [SessionAuthorize]
    public class BookWiseStockReportController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            //if (Convert.ToString(((UserSec)Session["UserSec"]).CircleID) != "")
            if (Convert.ToString(GlobalSettings.oUserData.CircleID) != "")
            {
                return View(get_report());
            }
            else
            {
                return RedirectToAction("Index", "CircleLogin");
            }
        }

        public Models.BookWiseStockreportCircle get_report()
        {

            BookWiseStockreportCircle lst_req = new BookWiseStockreportCircle();
            List<BookWiseStockreportCircle> lst_req1 = new List<BookWiseStockreportCircle>();
            try
            {
                //DataTable dtMastData = objDbTrx.GetBookWiseReqStokDetails(Convert.ToInt64(((UserSec)Session["UserSec"]).CircleID));
                DataTable dtMastData = objDbTrx.GetBookWiseReqStokDetails(Convert.ToInt64(GlobalSettings.oUserData.CircleID));
                //dtMastData = dtMastData.AsEnumerable()
                if (dtMastData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMastData.Rows.Count; i++)
                    {
                        BookWiseStockreportCircle rq = new BookWiseStockreportCircle();
                        rq.language = Convert.ToString(dtMastData.Rows[i]["language"].ToString());
                        //rq.Category = Convert.ToString(dtMastData.Rows[i]["BOOK_CATEGORY"].ToString());
                        rq.BookName = Convert.ToString(dtMastData.Rows[i]["book_name"].ToString());
                        rq.stock_quantity = "0";
                        rq.stock_damage_quantity = "0";
                        if (dtMastData.Rows[i]["stock_total"].ToString().Trim() != "")
                        {
                            rq.stock_quantity = Convert.ToString(dtMastData.Rows[i]["stock_total"].ToString());
                        }
                        rq.stock_damage_quantity = "0";
                        if (dtMastData.Rows[i]["stock_damage"].ToString().Trim() != "")
                        {
                            rq.stock_damage_quantity = Convert.ToString(dtMastData.Rows[i]["stock_damage"].ToString());
                        }
                        rq.req_quantity = "0";
                        if (dtMastData.Rows[i]["total"].ToString().Trim() != "")
                        {
                            rq.req_quantity = Convert.ToString(dtMastData.Rows[i]["total"].ToString());
                        }

                        rq.BookCode = Convert.ToString(dtMastData.Rows[i]["book_code"].ToString());
                        rq.remaining_quantity = Convert.ToString((Convert.ToInt32(rq.req_quantity) - Convert.ToInt32(rq.stock_quantity)) >= default(int) ? (Convert.ToInt32(rq.req_quantity) - Convert.ToInt32(rq.stock_quantity)) : default(int));
                        rq.QtyRcvdAtCircle = Convert.ToString(dtMastData.Rows[i]["QtyRcvd"].ToString());
                        rq.QtyDlvToSchool = Convert.ToString(dtMastData.Rows[i]["QtyDlvSch"].ToString());
                        lst_req1.Add(rq);
                    }
                }



                lst_req.bookwise_collection = lst_req1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lst_req;
        }
		
    }
}

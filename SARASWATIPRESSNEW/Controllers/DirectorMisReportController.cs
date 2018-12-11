using SARASWATIPRESSNEW.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SARASWATIPRESSNEW.Controllers
{
    public class DirectorMisReportController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string Command)
        {
            int CircleCnt = 0, BokCnt = 0;
            Int64 RemainingCnt = 0;
            StringBuilder strTableReport = new StringBuilder();
            StringBuilder strReport = new StringBuilder();
            StringBuilder strReport1 = new StringBuilder();
            StringBuilder strReport2 = new StringBuilder();
            StringBuilder strReportSchoolCodeHead = new StringBuilder();
            StringBuilder strReportSchoolNameHead = new StringBuilder();
            StringBuilder strTotReport = new StringBuilder();
            try
            {

                DataSet ds = objDbTrx.GetDistrictWiseMisReport();
                if (ds != null)
                {
                    CircleCnt = ds.Tables[1].Rows.Count;
                    for (int bkCnt = 0; bkCnt < ds.Tables[0].Rows.Count; bkCnt++)
                    {
                        strReport = new StringBuilder();
                        strReport1 = new StringBuilder();
                        strReport2 = new StringBuilder();
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'>No. of Books Required</td>");

                        strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                        strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                        strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                        strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                        strReport1.AppendLine("      <td style='text-align:Left;vertical-align: middle;'>Previous Year Quantity</td>");


                        strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                        strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                        strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                        strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                        strReport2.AppendLine("      <td style='text-align:Left;vertical-align: middle;'>Net Requirement</td>");
                        //PrintSchoolDtl = true;




                        for (int schCnt = 0; schCnt < ds.Tables[1].Rows.Count; schCnt++)
                        {
                            if (bkCnt == 0)
                            {
                                strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;vertical-align: middle;' > " + ds.Tables[1].Rows[schCnt]["DISTRICT"].ToString() + "    </td>");
                            }

                            bool found = false;
                            for (int iCnt = 0; iCnt < ds.Tables[2].Rows.Count; iCnt++)
                            {

                                if (ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() == ds.Tables[2].Rows[iCnt]["BOOK_CODE"].ToString() && ds.Tables[1].Rows[schCnt]["ID"].ToString() == ds.Tables[2].Rows[iCnt]["ID"].ToString())
                                {
                                    found = true;
                                    if (Convert.ToInt64(ds.Tables[2].Rows[iCnt]["TOT"].ToString()) > 0)
                                    {
                                        strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#808080' > " + ds.Tables[2].Rows[iCnt]["TOT"].ToString() + "      </td>");
                                    }
                                    else
                                    {
                                        strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + ds.Tables[2].Rows[iCnt]["TOT"].ToString() + "      </td>");
                                    }

                                    if (Convert.ToInt64(ds.Tables[2].Rows[iCnt]["STOCKQTY"].ToString()) > 0)
                                    {
                                        strReport1.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#99ff99' > " + ds.Tables[2].Rows[iCnt]["STOCKQTY"].ToString() + "      </td>");
                                    }
                                    else
                                    {
                                        strReport1.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + ds.Tables[2].Rows[iCnt]["STOCKQTY"].ToString() + "      </td>");
                                    }
                                    try
                                    {
                                        RemainingCnt = (Convert.ToInt64(ds.Tables[2].Rows[iCnt]["TOT"].ToString()) - Convert.ToInt64(ds.Tables[2].Rows[iCnt]["STOCKQTY"].ToString()));
                                        if (RemainingCnt < 0)
                                        {
                                            RemainingCnt = 0;
                                        }
                                    }
                                    catch
                                    {
                                        RemainingCnt = 0;
                                    }
                                    if (RemainingCnt > 0)
                                    {
                                        strReport2.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#ffff99' > " + RemainingCnt + "      </td>");
                                    }
                                    else
                                    {
                                        strReport2.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + RemainingCnt + "      </td>");
                                    }


                                    if (ds.Tables[2].Rows.Count != 1)
                                    {
                                        ds.Tables[2].Rows.RemoveAt(iCnt);
                                    }
                                }
                                if (found == true)
                                {
                                    break;
                                }
                            }
                            if (found == false)
                            {
                                strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' > 0</td>");
                                strReport1.AppendLine("      <td style='text-align:center;vertical-align: middle;' > 0</td>");
                                strReport2.AppendLine("      <td style='text-align:center;vertical-align: middle;' > 0</td>");
                            }
                        }

                        strTotReport.AppendLine("<tr>" + strReport.ToString() + "</tr>");
                        strTotReport.AppendLine("<tr>" + strReport1.ToString() + "</tr>");
                        strTotReport.AppendLine("<tr>" + strReport2.ToString() + "</tr>");
                    }
                    ds.Dispose();



                    strTableReport.AppendLine("<table border='1'>");
                    strTableReport.AppendLine("     <tr><td colspan='" + (CircleCnt + 5) + "'> " + "District Wise MIS Report</td></tr>");
                    strTableReport.AppendLine("     <tr>");
                    strTableReport.AppendLine("         <td style='text-align:Left;'>Book Code</td>");
                    strTableReport.AppendLine("         <td style='text-align:Left;'>Class</td>");
                    strTableReport.AppendLine("         <td style='text-align:Left;'>TITLE OF BOOKS</td>");
                    strTableReport.AppendLine("         <td>Language</td>");
                    strTableReport.AppendLine("         <td>&nbsp;</td>");
                    strTableReport.AppendLine("          " + strReportSchoolNameHead.ToString());
                    strTableReport.AppendLine("     </tr>");
                    strTableReport.AppendLine("          " + strTotReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "DistictMisReport" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
                    Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
                    String HTMLDataToExport = strTableReport.ToString();
                    //System.IO.FileInfo fiCSSInfo = new System.IO.FileInfo(Server.MapPath("/resources/css/ReportFormat.css"));
                    //System.Text.StringBuilder sbCSSInfo = new System.Text.StringBuilder();
                    //System.IO.StreamReader srCSSInfo = fiCSSInfo.OpenText();
                    //while (srCSSInfo.Peek() > 0)
                    //{
                    //    sbCSSInfo.Append(srCSSInfo.ReadLine());
                    //}
                    //srCSSInfo.Close();

                    Response.Write("<html><head><head>" +
                    HTMLDataToExport.Replace("<BR>", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<br>", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<BR >", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<BR />", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<br />", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<Br />", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<Br>", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<br >", "<br style='mso-data-placement:same-cell;'>") + "</html>");
                    Response.End();
                }


            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index1(string Command)
        {
            int SchoolCnt = 0, BokCnt = 0;
            StringBuilder strTableReport = new StringBuilder();
            StringBuilder strReport = new StringBuilder();
            StringBuilder strReportSchoolCodeHead = new StringBuilder();
            StringBuilder strReportSchoolNameHead = new StringBuilder();
            StringBuilder strTotReport = new StringBuilder();
            try
            {
                DataTable dt = objDbTrx.GetDirectorMisReport();
                if (dt.Rows.Count > 0)
                {
                    DataTable dtBook = new DataTable();
                    var qryBook = from rows in dt.AsEnumerable()
                                    orderby rows["BOOK_CODE"] ascending
                                    group rows by new { BOOK_CODE = rows["BOOK_CODE"], CLASS = rows["CLASS"], BOOK_NAME = rows["BOOK_NAME"], BOOK_CATEGORY = rows["BOOK_CATEGORY"], LANGUAGE = rows["LANGUAGE"] } into grp
                                    select grp.First();
                    dtBook = qryBook.CopyToDataTable();
                    BokCnt = dtBook.Rows.Count;
                    DataTable dtSchool = new DataTable();

                    var qrySchool = from rows in dt.AsEnumerable()
                                    orderby rows["DISTRICT"] ascending
                                    group rows by new { ID = rows["DDID"] } into grp
                                    select grp.First();
                    dtSchool = qrySchool.CopyToDataTable();
                    SchoolCnt = dtSchool.Rows.Count;
                    for (int bkCnt = 0; bkCnt < dtBook.Rows.Count; bkCnt++)
                    {
                        strReport = new StringBuilder();
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["BOOK_CATEGORY"].ToString() + " </td>");
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                        //PrintSchoolDtl = true;
                        for (int schCnt = 0; schCnt < dtSchool.Rows.Count; schCnt++)
                        {
                            if (bkCnt == 0)
                            {
                                strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtSchool.Rows[schCnt]["DISTRICT"].ToString() + "    </td>");
                            }

                            DataTable dtTotCnt = new DataTable();
                            bool found = false;
                            for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
                            {

                                if (dt.Rows[iCnt]["BOOK_ID"].ToString() == dtBook.Rows[bkCnt]["BOOK_ID"].ToString() && dt.Rows[iCnt]["DDID"].ToString() == dtSchool.Rows[schCnt]["DDID"].ToString())
                                {
                                    found = true;
                                    strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dt.Rows[iCnt]["Tot"].ToString() + "      </td>");
                                    if (dt.Rows.Count != 1)
                                    {
                                        dt.Rows.RemoveAt(iCnt);
                                    }
                                }
                                if (found == true)
                                {
                                    break;
                                }
                            }
                            dtTotCnt.Dispose();
                            if (found == false)
                            {
                                strReport.AppendLine("      <td style='text-align:Center;vertical-align: middle;'> 0</td>");
                            }
                        }
                        strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["StockQty"].ToString() + "      </td>");
                        strTotReport.AppendLine("<tr>" + strReport.ToString() + "</tr>");
                    }
                    dt.Dispose();
                    dtBook.Dispose();
                    dtSchool.Dispose();
                    strReportSchoolCodeHead.AppendLine("      <td style='text-align:Left;'> &nbsp; </td>");
                    strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;'> Stock Qty.   </td>");

                    strTableReport.AppendLine("<table border='1' width='" + (((SchoolCnt * 2) + 5) * 50) + "Px'>");
                    strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'> " + "MIS Report</td></td></tr>");                       
                    strTableReport.AppendLine("     <tr><td style='text-align:Left;'>Book Code</td><td style='text-align:Left;'>Class</td><td style='text-align:Left;'>TITLE OF BOOKS</td><td style='text-align:Left;'>Category</td><td>Language</td>");
                    strTableReport.AppendLine("          " + strReportSchoolNameHead.ToString());
                    strTableReport.AppendLine("     </tr>");
                    strTableReport.AppendLine("          " + strTotReport.ToString());
                    strTableReport.AppendLine("</table>");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    String FileName = "DistrictMis" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
                    Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
                    String HTMLDataToExport = strTableReport.ToString();
                    //System.IO.FileInfo fiCSSInfo = new System.IO.FileInfo(Server.MapPath("/resources/css/ReportFormat.css"));
                    //System.Text.StringBuilder sbCSSInfo = new System.Text.StringBuilder();
                    //System.IO.StreamReader srCSSInfo = fiCSSInfo.OpenText();
                    //while (srCSSInfo.Peek() > 0)
                    //{
                    //    sbCSSInfo.Append(srCSSInfo.ReadLine());
                    //}
                    //srCSSInfo.Close();

                    Response.Write("<html><head><head>" +
                    HTMLDataToExport.Replace("<BR>", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<br>", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<BR >", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<BR />", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<br />", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<Br />", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<Br>", "<br style='mso-data-placement:same-cell;'>")
                                                   .Replace("<br >", "<br style='mso-data-placement:same-cell;'>") + "</html>");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }
            return View();
        }

    }
}

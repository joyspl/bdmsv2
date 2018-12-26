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
    [SessionAuthorize]
    public class MisReportByCircleController : Controller
    {
        BusinessLogicDbTrx objDbTrx = new BusinessLogicDbTrx();
        public ActionResult Index()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult Index(string Command)
        {
            int SchoolCnt = 0, BokCnt = 0;

            string CircleId = "";
            StringBuilder strTableReport = new StringBuilder();
            StringBuilder strReport = new StringBuilder();
            StringBuilder strReportSchoolCodeHead = new StringBuilder();
            StringBuilder strReportSchoolNameHead = new StringBuilder();
            StringBuilder strTotReport = new StringBuilder();
            try { CircleId = Request["CircleID"].ToString(); }
            catch { CircleId = ""; }
            try
            {
                if (CircleId != "")
                {
                    DataSet ds = objDbTrx.GetMisReport(Convert.ToInt64(CircleId), Request["ddlType"].ToString(), Request["ddlClassCategory"].ToString(), Request["ddlLanguageName"].ToString(), Request["ddlBookName"].ToString(), Request["txtStartDate"].ToString(), Request["txtEndDate"].ToString());

                    //filters


                    if (ds != null)
                    {
                        SchoolCnt = ds.Tables[1].Rows.Count;
                        for (int bkCnt = 0; bkCnt < ds.Tables[0].Rows.Count; bkCnt++)
                        {
                            strReport = new StringBuilder();
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["CLASS"].ToString() + "         </td>");
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
                            strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
                            //PrintSchoolDtl = true;
                            for (int schCnt = 0; schCnt < ds.Tables[1].Rows.Count; schCnt++)
                            {
                                if (bkCnt == 0)
                                {
                                    strReportSchoolCodeHead.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[schCnt]["SCHOOL_CODE"].ToString() + "    </td>");
                                    strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + ds.Tables[1].Rows[schCnt]["SCHOOL_NAME"].ToString() + "    </td>");
                                }

                                bool found = false;
                                for (int iCnt = 0; iCnt < ds.Tables[2].Rows.Count; iCnt++)
                                {

                                    if (ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() == ds.Tables[2].Rows[iCnt]["BOOK_CODE"].ToString() && ds.Tables[1].Rows[schCnt]["ID"].ToString() == ds.Tables[2].Rows[iCnt]["ID"].ToString() && ds.Tables[0].Rows[bkCnt]["LANGUAGE"].ToString() == ds.Tables[2].Rows[iCnt]["LANGUAGE"].ToString())
                                    {
                                        found = true;
                                        if (Convert.ToInt64(ds.Tables[2].Rows[iCnt]["Tot"].ToString()) > 0)
                                        {
                                            strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#808080' > " + ds.Tables[2].Rows[iCnt]["Tot"].ToString() + "      </td>");
                                        }
                                        else
                                        {
                                            strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + ds.Tables[2].Rows[iCnt]["Tot"].ToString() + "      </td>");
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
                                }
                            }
                            bool isFound = false;
                            for (int iCnt = 0; iCnt < ds.Tables[3].Rows.Count; iCnt++)
                            {

                                if (ds.Tables[0].Rows[bkCnt]["BOOK_CODE"].ToString() == ds.Tables[3].Rows[iCnt]["BOOK_CODE"].ToString())
                                {
                                    isFound = true;
                                    if (Convert.ToInt64(ds.Tables[3].Rows[iCnt]["StockQty"].ToString()) > 0)
                                    {
                                        strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' bgcolor='#808080'  > " + ds.Tables[3].Rows[iCnt]["StockQty"].ToString() + "      </td>");
                                    }
                                    else
                                    {
                                        strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' > " + ds.Tables[3].Rows[iCnt]["StockQty"].ToString() + "      </td>");
                                    }


                                    if (ds.Tables[3].Rows.Count != 1)
                                    {
                                        ds.Tables[3].Rows.RemoveAt(iCnt);
                                    }
                                }
                                if (isFound == true)
                                {
                                    break;
                                }
                            }
                            if (isFound == false)
                            {
                                strReport.AppendLine("      <td style='text-align:center;vertical-align: middle;' > 0</td>");
                            }
                            strTotReport.AppendLine("<tr>" + strReport.ToString() + "</tr>");
                        }
                      

                        strReportSchoolCodeHead.AppendLine("      <td style='text-align:Left;'> &nbsp; </td>");
                        strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;'> Stock Qty.   </td>");
                        strTableReport.AppendLine("<table border='1' >");
                        strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'> " + "NET REQUISITION OF <B>" + ds.Tables[1].Rows[0]["CIRCLE_NAME"].ToString() + "</B> CIRCLE  DIST- <B>" + ds.Tables[1].Rows[0]["DISTRICT"].ToString() + " </B></td></td></tr>");
                       
                        //filter details

                        // modified 8.12.18 Pomeli

                        //strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'>Language Filter&nbsp; :&nbsp;" + Request["LanguageName"].ToString() + "</td></td></tr>");
                        //strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'>Book Category Filter &nbsp; :&nbsp;" + Request["ClassCategory"].ToString() + "</td></td></tr>");
                        //strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'>Book Name Filter &nbsp; :&nbsp;" + Request["BookName"].ToString() + "</td></td></tr>");
                        //strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'>Type Filter &nbsp; :&nbsp;" + Request["ddlType"].ToString() + "</td></td></tr>");

                        //strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'>Date     From  &nbsp; :&nbsp; " + Request["txtStartDate"].ToString() + "TO  &nbsp; :&nbsp;" + Request["txtEndDate"].ToString() + "</td></td></tr>");
                        
                        
                        strTableReport.AppendLine("     <tr><td colspan='4'>UDISE CODE</td>");
                        strTableReport.AppendLine("          " + strReportSchoolCodeHead.ToString());
                        strTableReport.AppendLine("     </tr>");

                        strTableReport.AppendLine("     <tr>");
                        strTableReport.AppendLine("         <td style='text-align:Left;'>Book Code</td>");
                        strTableReport.AppendLine("         <td style='text-align:Left;'>Class</td>");
                        strTableReport.AppendLine("         <td style='text-align:Left;'>TITLE OF BOOKS</td>");                       
                        strTableReport.AppendLine("         <td>Language</td>");
                        strTableReport.AppendLine("          " + strReportSchoolNameHead.ToString());
                        strTableReport.AppendLine("     </tr>");
                        strTableReport.AppendLine("          " + strTotReport.ToString());
                        strTableReport.AppendLine("</table>");

                        ds.Dispose();


                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.ms-excel";
                        String FileName = "CircleMis" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
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
                        //return Content(HTMLDataToExport);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
            }

            //  return File(fileContents, "application/vnd.ms-excel", fName);
            // TempData["ReportTable"] = strTableReport.ToString();

            return View();
          
        }
        
        //[HttpPost]
        //public ActionResult Index1(string Command)
        //{
        //    int SchoolCnt = 0, BokCnt = 0;

        //    string CircleId = "";
        //    StringBuilder strTableReport = new StringBuilder();
        //    StringBuilder strReport = new StringBuilder();
        //    StringBuilder strReportSchoolCodeHead = new StringBuilder();
        //    StringBuilder strReportSchoolNameHead = new StringBuilder();
        //    StringBuilder strTotReport = new StringBuilder();
        //    try { CircleId = GlobalSettings.oUserData.CircleID; }
        //    catch { CircleId = ""; }
        //    try
        //    {
        //        if (CircleId != "")
        //        {
        //            DataTable dt = objDbTrx.GetMisReport(Convert.ToInt32(CircleId));
        //            if (dt.Rows.Count > 0)
        //            {
        //                DataTable dtBook = new DataTable();
        //                var qryBook = from rows in dt.AsEnumerable()
        //                              orderby rows["BOOK_CODE"] ascending
        //                              group rows by new { BOOK_CODE = rows["BOOK_CODE"], CLASS = rows["CLASS"], BOOK_NAME = rows["BOOK_NAME"], BOOK_CATEGORY = rows["BOOK_CATEGORY"], LANGUAGE = rows["LANGUAGE"] } into grp
        //                              select grp.First();
        //                dtBook = qryBook.CopyToDataTable();
        //                BokCnt = dtBook.Rows.Count;
        //                DataTable dtSchool = new DataTable();

        //                var qrySchool = from rows in dt.AsEnumerable()
        //                                orderby rows["SCHOOL_NAME"] ascending
        //                                group rows by new { SCHOOL_ID = rows["SCHOOL_ID"] } into grp
        //                                select grp.First();
        //                dtSchool = qrySchool.CopyToDataTable();
        //                SchoolCnt = dtSchool.Rows.Count;
        //                for (int bkCnt = 0; bkCnt < dtBook.Rows.Count; bkCnt++)
        //                {
        //                    strReport = new StringBuilder();
        //                    strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["BOOK_CODE"].ToString() + "     </td>");
        //                    strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["CLASS"].ToString() + "         </td>");
        //                    strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["BOOK_NAME"].ToString() + "     </td>");
        //                    strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["BOOK_CATEGORY"].ToString() + " </td>");
        //                    strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["LANGUAGE"].ToString() + "      </td>");
        //                    //PrintSchoolDtl = true;
        //                    for (int schCnt = 0; schCnt < dtSchool.Rows.Count; schCnt++)
        //                    {
        //                        if (bkCnt == 0)
        //                        {
        //                            strReportSchoolCodeHead.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtSchool.Rows[schCnt]["SCHOOL_CODE"].ToString() + "    </td>");
        //                            strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtSchool.Rows[schCnt]["SCHOOL_NAME"].ToString() + "    </td>");
        //                        }

        //                        DataTable dtTotCnt = new DataTable();
        //                        bool found = false;
        //                        for (int iCnt = 0; iCnt < dt.Rows.Count; iCnt++)
        //                        {
                                   
        //                            if (dt.Rows[iCnt]["BOOK_ID"].ToString() == dtBook.Rows[bkCnt]["BOOK_ID"].ToString() && dt.Rows[iCnt]["SCHOOL_ID"].ToString() == dtSchool.Rows[schCnt]["SCHOOL_ID"].ToString())
        //                            {
        //                                found = true;
        //                                strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dt.Rows[iCnt]["Tot"].ToString() + "      </td>");
        //                                if (dt.Rows.Count != 1)
        //                                {
        //                                    dt.Rows.RemoveAt(iCnt);
        //                                }
        //                            }
        //                            if (found == true)
        //                            {
        //                                break;
        //                            }
        //                        }
        //                        dtTotCnt.Dispose();
        //                        if (found == false)
        //                        {
        //                            strReport.AppendLine("      <td style='text-align:Center;vertical-align: middle;'> 0</td>");
        //                        }
        //                    }
        //                    strReport.AppendLine("      <td style='text-align:Left;vertical-align: middle;'> " + dtBook.Rows[bkCnt]["StockQty"].ToString() + "      </td>");
        //                    strTotReport.AppendLine("<tr>" + strReport.ToString() + "</tr>");
        //                }
        //                dt.Dispose();
        //                dtBook.Dispose();
        //                dtSchool.Dispose();
        //                strReportSchoolCodeHead.AppendLine("      <td style='text-align:Left;'> &nbsp; </td>");
        //                strReportSchoolNameHead.AppendLine("      <td style='text-align:Left;'> Stock Qty.   </td>");

        //                strTableReport.AppendLine("<table border='1' width='" + (((SchoolCnt*2) + 5) * 50) + "Px'>");
        //                strTableReport.AppendLine("     <tr><td colspan='" + (SchoolCnt + 5) + "'> " + "NET REQUISITION OF <B>" + dt.Rows[0]["CIRCLE_NAME"].ToString() + "</B> CIRCLE  DIST- <B>" + dt.Rows[0]["DISTRICT"].ToString() + " </B></td></td></tr>");
        //                strTableReport.AppendLine("     <tr><td colspan='5'>UDISE CODE</td>");
        //                strTableReport.AppendLine("          " + strReportSchoolCodeHead.ToString());
        //                strTableReport.AppendLine("     </tr>");
        //                strTableReport.AppendLine("     <tr><td style='text-align:Left;'>Book Code</td><td style='text-align:Left;'>Class</td><td style='text-align:Left;'>TITLE OF BOOKS</td><td style='text-align:Left;'>Category</td><td>Language</td>");
        //                strTableReport.AppendLine("          " + strReportSchoolNameHead.ToString());
        //                strTableReport.AppendLine("     </tr>");
        //                strTableReport.AppendLine("          " + strTotReport.ToString());
        //                strTableReport.AppendLine("</table>");
        //            }
        //        }
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.ContentType = "application/vnd.ms-excel";
        //        String FileName = "MISReport" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls";
        //        Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
        //        String HTMLDataToExport = strTableReport.ToString();
        //        //System.IO.FileInfo fiCSSInfo = new System.IO.FileInfo(Server.MapPath("/resources/css/ReportFormat.css"));
        //        //System.Text.StringBuilder sbCSSInfo = new System.Text.StringBuilder();
        //        //System.IO.StreamReader srCSSInfo = fiCSSInfo.OpenText();
        //        //while (srCSSInfo.Peek() > 0)
        //        //{
        //        //    sbCSSInfo.Append(srCSSInfo.ReadLine());
        //        //}
        //        //srCSSInfo.Close();

        //        Response.Write("<html><head><head>" +                       
        //        HTMLDataToExport.Replace("<BR>", "<br style='mso-data-placement:same-cell;'>")
        //                                       .Replace("<br>", "<br style='mso-data-placement:same-cell;'>")
        //                                       .Replace("<BR >", "<br style='mso-data-placement:same-cell;'>")
        //                                       .Replace("<BR />", "<br style='mso-data-placement:same-cell;'>")
        //                                       .Replace("<br />", "<br style='mso-data-placement:same-cell;'>")
        //                                       .Replace("<Br />", "<br style='mso-data-placement:same-cell;'>")
        //                                       .Replace("<Br>", "<br style='mso-data-placement:same-cell;'>")
        //                                       .Replace("<br >", "<br style='mso-data-placement:same-cell;'>") + "</html>");

        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        objDbTrx.SaveSystemErrorLog(ex, Request.UserHostAddress);
        //    }

        //    //  return File(fileContents, "application/vnd.ms-excel", fName);
        //    // TempData["ReportTable"] = strTableReport.ToString();
        //    return View();
        //}
    }
}

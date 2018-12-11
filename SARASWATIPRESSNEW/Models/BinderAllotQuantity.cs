using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class BinderAllotQuantity
    {
        public int ID { get; set; }
        public string AllotmentDate { get; set; }
        public string AllotmentCode { get; set; }
        public int BinderId { get; set; }
        public int ChallanCategoryId { get; set; }
        public string LanguageId { get; set; }
        public string BookCode { get; set; }
        public string BookId { get; set; }
        public Int32 TotQty { get; set; }
        public Int32 Lot { get; set; }
        public Int32 ReqQty { get; set; }
        public Int32 QtyIssued { get; set; }
        public string BookName { get; set; }
        public string BookCategoryName { get; set; }
        public string LanguageName { get; set; }
        public string BinderName { get; set; }
        public string BinderShortCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int SaveStatus { get; set; }
        public string PrintDtl { get; set; }
        public int AcademicYearID { get; set; }
        public string UserId { get; set; }
        public StringBuilder barcodeDPL { get; set; }


    }
}
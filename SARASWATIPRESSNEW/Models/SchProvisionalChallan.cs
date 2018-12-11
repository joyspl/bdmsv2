using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class SchProvisionalChallan
    {
        public string InvoiceDtlId { get; set; }
        public Int32 ChallanId { get; set; }
        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
        public string DistrictName { get; set; }
        public Int16 DistrictId { get; set; }
        public string CircleName { get; set; }
        public Int32 CircleId { get; set; }
        public string CircleAddress { get; set; }
        public string ConsigneeNo { get; set; }
        public Int32 BinderAllotMentId { get; set; }
        public string BinderAllotMentCode { get; set; }
        public string VehicleNo { get; set; }
        public string CategoryName { get; set; }
        public string LanguageName { get; set; }
        public int LanguageID { get; set; }
        public string CustomerOrderNo { get; set; }
        
        public string TransporterID { get; set; }
        public string Transporter { get; set; }
        public string InRequisitionIds { get; set; }
        public string InBookCodes { get; set; }
        public int SaveStatus { get; set; }
        public string UserId { get; set; }
        public int AcademicYearID { get; set; }
        public Int32 ChallanQty { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public string UpdateCode { get; set; }
        public string UpdateMessage { get; set; }

        public Int32 Qty { get; set; }
        public Int32 AllotedQty { get; set; }
        public Int32 ReqQty { get; set; }
        public Int32 Lot { get; set; }

        public Int32 RequiredQty { get; set; }
        public Int32 IssuedQty { get; set; }
        public Int32 RemainigQty { get; set; } 
        public double RequiredWeight { get; set; }
        public double IssuedWeight { get; set; }
        public double RemainigWeight { get; set; }
        public List<BookRequisitionCalculatedDtl> BookRequisitionCalculatedDtlCollection { get; set; }
        
    }
}
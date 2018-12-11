using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class SchRequisition
    {
        public int CircleID { get; set; }
        public Int64 RequisitionID { get; set; }        
        public string RequisitionDate { get; set; }
        public string RequisitionCode { get; set; }
        public Int64 SchoolID { get; set; }
        public int LanguageID { get; set; }
        public int CategoryID { get; set; }
        public int BookID { get; set; }
        public int StudentEnrolled { get; set; }
        public int RequisitionQuantity { get; set; }
        public int StockQuantity { get; set; }
        public int SaveStatus { get; set; }
        public string UserId { get; set; }
        public int AcademicYearID { get; set; }        
        public MstSchool MstSchool { get; set; }
        public MstLanguage MstLanguage { get; set; }
        public MstCategory MstCategory { get; set; }
        public MstAcademicYear MstAcademicYear { get; set; }
        public List<SchRequisitionDtl> reqTrxCollection { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class SchRequisitionView
    {
        public Int64 RequisitionID { get; set; }
        public string RequisitionDate { get; set; }
        public string RequisitionCode { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string LanguageName { get; set; }
        public Int32 RemainQuantity { get; set; }        
        public string Category { get; set; }       
        public int SaveStatus { get; set; }
        public int ISAPPROVED { get; set; }
        public string APPROVED_BY { get; set; }
        public DateTime? APPROVED_TS { get; set; }

        public int ISAPPROVED_DIST { get; set; }
        public string APPROVED_BY_DIST { get; set; }
        public DateTime? APPROVED_TS_DIST { get; set; }

        public int ISAPPROVED_ADMIN { get; set; }

        public string LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
       
    }
}
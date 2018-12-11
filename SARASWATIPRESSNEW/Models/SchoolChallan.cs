using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class SchoolChallan
    {
        public Int64 SchoolChallanUniqueId { get; set; }
        public Int64 RequisitionId { get; set; }
        public string RequisitionDate { get; set; }
        public string ReqCode { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }

        public bool IsPendingRequire { get; set; }
        public string SchoolChallanCode { get; set; }
        public string SchoolChallanDate { get; set; }
        public string ChallanUpdatedTs { get; set; }
        public string ChallanUpdatedBy { get; set; }
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolContactNo { get; set; }
        public string SchoolEmailId { get; set; }
        public string ChallanYear { get; set; }

        public string UserId { get; set; }
        public int BookRwCnt { get; set; }
        public List<SchoolChallanBookReqDtl> trxSchoolChallanBookReqDtl { get; set; }

        public string CIRCLE_OFFICER_NAME { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string CIRCLE_ADDRESS { get; set; }
        public string CIRCLE_PINCODE { get; set; }
        public string POLICE_STATION { get; set; }
        public string MOBILE_NO { get; set; }
        public string ALTERNATE_MOBILE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string DISTRICT { get; set; }
    }
}
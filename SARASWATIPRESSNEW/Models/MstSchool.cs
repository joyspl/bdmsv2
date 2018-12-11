using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class MstSchool
    {
        public Int64 SchoolID { get; set; }
        public Int32 CircleId { get; set; }
        public int DistrictID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolAdrees { get; set; }
        public string SchoolMobile { get; set; }
        public string SchoolEmailid { get; set; }
        public string SchoolAlternateMobile { get; set; }
        public string DeleivaryAddress { get; set; }
        public string PostalCode { get; set; }
        public string PoliceStation { get; set; }
        public string CircleName { get; set; }
        public string UserId { get; set; }
    }
}
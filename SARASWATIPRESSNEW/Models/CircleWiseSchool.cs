using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class CircleWiseSchool
    {
        public string school_name { get; set; }
        public string school_code{ get; set; }
        public string req_amt { get; set; }
        public List<District> lst_district { get; set; }
        public List<Circle> lst_circle { get; set; }
        public int DistrictID { get; set; }
        public int CircleID { get; set; }

        public List<CircleWiseSchoolReport> CollectionCircleWiseSchoolReport { get; set; }
        public List<CircleWiseSchool> circle_wise_school { get; set; }
        public List<Requisition> req_wise_collection { get; set; }
    }
}
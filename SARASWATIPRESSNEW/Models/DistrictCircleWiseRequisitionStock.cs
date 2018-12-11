using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class DistrictCircleWiseRequisitionStock
    {

        public string district_name { get; set; }
        public string circle_name { get; set; }

        public string no_of_school_cnf { get; set; }

        public string no_of_school_sad { get; set; }

        public List<DistrictCircleWiseRequisitionStock> districtcirclewisecollection { get; set; }

        public string circle_id { get; set; }

        public List<Circle> circlecollectionlist { get; set; }

    }
}
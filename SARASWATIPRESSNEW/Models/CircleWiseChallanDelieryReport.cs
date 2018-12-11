using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class CircleWiseChallanDeliveryReport
    {
        public string district_name { get; set; }
        public string circle_name { get; set; }      

        public List<CircleWiseRequisitionStock> circlewisecollection { get; set; }

        public List<District> districtcollectionlist { get; set; }
        public bool IsDetailsRequire { get; set; }
        public string district_id { get; set; }

        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
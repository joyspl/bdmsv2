using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SARASWATIPRESSNEW.Models
{
    public class CircleWiseRequisitionStock
    {
        public string district_name { get; set; }

        public string circle_name { get; set; }

        public string no_of_school_cnf { get; set; }

        public string no_of_school_sad { get; set; }

        public List<CircleWiseRequisitionStock> circlewisecollection { get; set; }

        public List<District> districtcollectionlist { get; set; }

        public string district_id { get; set; }
       
    }
}
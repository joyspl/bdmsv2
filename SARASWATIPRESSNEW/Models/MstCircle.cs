using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class MstCircle
    {
        public int CircleId { get; set; }
        public string CircleCode { get; set; }
        public string CirclAddress { get; set; }
        public string CirclePinCode { get; set; }
        public string CircleName { get; set; }
        public string DistrictName { get; set; }
        public int DistrictId { get; set; }
    }
}
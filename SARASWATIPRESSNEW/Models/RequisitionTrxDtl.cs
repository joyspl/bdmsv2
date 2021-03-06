﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SARASWATIPRESSNEW.Models
{
    public class RequisitionTrxDtl
    {
        [XmlAttribute]
        public Int64 BookID { get; set; }
        [XmlAttribute]
        public string BookCode { get; set; }
        [XmlAttribute]
        public string BookName { get; set; }
        [XmlAttribute]
        public int StudentEnrolled { get; set; }
        [XmlAttribute]
        public int QtyRequirement { get; set; }
        [XmlAttribute]
        public string classname { get; set; }
        [XmlAttribute]
        public int RequisitionQuantity { get; set; }
        [XmlAttribute]
        public int StockQuantity { get; set; }
        [XmlAttribute]
        public int PreviousYearReqQty { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SARASWATIPRESSNEW.Models
{
    public class SchoolChallanBookReqDtl
    {
        [XmlAttribute]
        public Int64 ReqDtlId { get; set; }
        [XmlAttribute]
        public string Class { get; set; }
        [XmlAttribute]
        public Int64 BookID { get; set; }
        [XmlAttribute]
        public string BookCode { get; set; }
        [XmlAttribute]
        public string BookName { get; set; }        
        [XmlAttribute]
        public Int64 RequisitionQuantity { get; set; }
         [XmlAttribute]
        public Int64 AvailableStockQuantity { get; set; }
        [XmlAttribute]
        public Int64 AlreadyShippedQuantity { get; set; }      
        [XmlAttribute]
        public Int64 QuantityForShipping { get; set; }
        [XmlAttribute]
        public Int64 QtyReceived { get; set; }
        [XmlAttribute]
        public Int64 StockQty { get; set; }
    }
}
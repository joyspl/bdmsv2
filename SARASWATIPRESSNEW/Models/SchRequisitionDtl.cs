using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SARASWATIPRESSNEW.Models
{
    public class SchRequisitionDtl
    {
        [XmlAttribute]
        public Int64 BookID { get; set; }
        [XmlAttribute]
        public string BookCode { get; set; }
        [XmlAttribute]
        public int IsOptional { get; set; }
        [XmlAttribute]
        public string BookName { get; set; }
        [XmlAttribute]
        public string ClassName { get; set; }
        [XmlAttribute]
        public int CLASS_INT { get; set; }
        [XmlAttribute]
        public int PreviousYearRequirement { get; set; }
        [XmlAttribute]
        public int StudentEnrolled { get; set; }
        [XmlAttribute]
        public int StockQuantity { get; set; }
        [XmlAttribute]
        public int RequisitionQuantity { get; set; }
        [XmlAttribute]
        public bool BookLock { get; set; }
        
    }

    
    public class SchRequisitionDtlforreport
    {
        [XmlAttribute]
        public Int64 BookID { get; set; }
        [XmlAttribute]
        public string BookCode { get; set; }
        [XmlAttribute]
        public string BookName { get; set; }
        [XmlAttribute]
        public string ClassName { get; set; }  
         [XmlAttribute]
        public string BookType { get; set; } 
        [XmlAttribute]
        public int PreviousYearRequirement { get; set; }
        [XmlAttribute]
        public int StudentEnrolled { get; set; }
        [XmlAttribute]
        public int StockQuantity { get; set; }
        [XmlAttribute]
        public int RequisitionQuantity { get; set; }
        [XmlAttribute]
        public bool BookLock { get; set; }
        
    }
}

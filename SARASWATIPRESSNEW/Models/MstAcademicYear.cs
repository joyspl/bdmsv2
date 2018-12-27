using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class MstAcademicYear
    {
        public int AcademicYearID { get; set; }
        public string AcademicYear { get; set; }
        public int ISACTIVE { get; set; }
        public string PFX_REQ { get; set; }
        public string PFX_CHALLAN { get; set; }
        public string PFX_SCHCHALLAN { get; set; }
        public string PFX_INVOICE { get; set; }
    }
}
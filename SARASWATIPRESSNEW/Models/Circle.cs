using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class Circle
    {
        [Key]
        public int CircleID { get; set; }

        //[Required(ErrorMessage = "Enter Circle Name")]
        public string Circle_name { get; set; }

        //[Required(ErrorMessage = "Enter Circle Code")]
        public string Circle_code { get; set; }

        
        public int district_id { get; set; }


        

    }

    public class CircleMaster
    {
        private bool? _ActiveStatus = null;
        public int CircleID { get; set; }
        public int DistrictId { get; set; }
        public string circleName { get; set; }

        public string MobileNo { get; set; }
        public string CircleOfficerName { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }

        public string CirclePinCode { get; set; }
        public string PoliceStation { get; set; }
        public string AlternateMobileNo { get; set; }
        public int Active { get; set; }
        public bool ActiveStatus
        {
            get
            {
                return this._ActiveStatus.HasValue ? this._ActiveStatus.Value : Active > default(int) ? true : false;
            }
            set
            {
                this._ActiveStatus = value;
            }
        }
    }
}
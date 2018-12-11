using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class CircleUser
    {
        [Key]
        public int CircleUserID { get; set; }

        //[Required(ErrorMessage = "Enter Book Code")]
        public int CircleID { get; set; }
        public int DistrictId { get; set; }
        //[Required(ErrorMessage = "Enter School")]
        public string circleName { get; set; }

        //[Required(ErrorMessage = "Enter Category")]
        public string MobileNo { get; set; }


        public string CircleOfficerName { get; set; }

        //[Required(ErrorMessage = "Enter Language")]
        public string EmailId { get; set; }

        //[Required(ErrorMessage = "Enter rate of book")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Enter Requisition Quantity")]
        public string Userid { get; set; }

        public string Password { get; set; }

        public List<CircleUser> CircleUsercollection { get; set; }

        public List<Circle> CircleCollection = new List<Circle>();

        public bool active { get; set; }
        public bool flag { get; set; }

        public string CirclePinCode { get; set; }
        public string PoliceStation { get; set; }
        public string AlternateMobileNo { get; set; }
    }
}
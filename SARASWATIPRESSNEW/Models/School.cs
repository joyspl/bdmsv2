using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class School
    {

        [Key]
        public Int64 SchoolID { get; set; }

        public int DistrictId { get; set; }

        public int CircleId { get; set; }

        [Required(ErrorMessage = "Enter School")]
        public string School_name { get; set; }

        [Required(ErrorMessage = "Enter School Code")]
        public string School_Code { get; set; }

        public string School_Adrees { get; set; }

        [Required(ErrorMessage = "Enter School Mobile")]
        public string School_Mobile { get; set; }

        //[Required(ErrorMessage = "Enter School Email Id")]
        public string School_Emailid { get; set; }

        public List<District> DistrictCollection { get; set; }

        public List<Circle> CircleCollection { get; set; }

        public string DistrictName { get; set; }

        public string CircleName { get; set; }

        public string schooldisplaytext { get; set; }

        public List<School> school_collection { get; set; }

        public string School_alt_Mobile { get; set; }
        public string UserId { get; set; }
        
        public bool stat { get; set; }       

    }
}
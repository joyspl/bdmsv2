using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class MstCircleUser
    {
        [Key]
        public int ID { get; set; }
        public int CIRCLE_ID { get; set; }
        public string CIRCLE_OFFICER_NAME { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string CIRCLE_ADDRESS { get; set; }
        public string USER_ID { get; set; }
        public string PASSWORD { get; set; }
        public int ACTIVE { get; set; }
        public int flag { get; set; }
        public string Circle_PinCode { get; set; }
    }
}
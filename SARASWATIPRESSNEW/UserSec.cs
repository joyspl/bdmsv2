using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW
{
    public class UserSec
    {

        public string UserId;
        public string DisplayName;       
        public string UserUniqueId;

        public string CircleID;
        public string DistrictID;
        public string CircleName;
        public string DistrictNname;

        public int AcademicYearId;
        public string AcademicYear;
        public string UserType;

        public int HasReqEditPermission;
        public int HasChallanRevertPermission;

        public UserRole vUserRole;
        public UserSec() { }

    }

    public class TempModelBase
    {
        protected int? _FormatNumberPaddingCount = null;
    }

    public class AcademicYear : TempModelBase
    {
        public int ID { get; set; }
        public string ACAD_YEAR { get; set; }
        public int ISACTIVE { get; set; }
        public string PFX_REQ { get; set; }
        public string PFX_CHALLAN { get; set; }
        public string PFX_SCHCHALLAN { get; set; }
        public string PFX_INVOICE { get; set; }
        public int FormatNumberPaddingCount
        {
            get
            {
                return this._FormatNumberPaddingCount.HasValue ? this._FormatNumberPaddingCount.Value : (System.Configuration.ConfigurationManager.AppSettings["FormatNumberPaddingCount"] != null ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FormatNumberPaddingCount"]) : 7);
            }
            set
            {
                this._FormatNumberPaddingCount = value;
            }
        }
        public string ACAD_YEAR_SHORT { get; set; }
        public string PFX_BINDER { get; set; }
    }

    public enum UserRole
    {
        CIRCLE = 1,
        DISTRICT = 2,
        ADMIN = 3,
        TBLOGIN = 4,
        DIRECTORATE = 5,
        TRANSPORTER = 6,
        CHALLAN = 7,
        LOGISTIC = 11
    }
}
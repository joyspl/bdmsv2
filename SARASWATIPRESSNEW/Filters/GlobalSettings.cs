﻿using SARASWATIPRESSNEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class GlobalSettings
{
    public static UserSec oUserData
    {
        get
        {
            if (HttpContext.Current.Session["oUserData"] != null)
                return HttpContext.Current.Session["oUserData"] as UserSec;
            else
                return new UserSec();
        }
        set
        {
            HttpContext.Current.Session["oUserData"] = value;
        }
    }
    public static AcademicYear oAcademicYear
    {
        get
        {
            if (HttpContext.Current.Session["oAcademicYear"] != null)
                return HttpContext.Current.Session["oAcademicYear"] as AcademicYear;
            else
                return new AcademicYear();
        }
        set
        {
            HttpContext.Current.Session["oAcademicYear"] = value;
        }
    }
}
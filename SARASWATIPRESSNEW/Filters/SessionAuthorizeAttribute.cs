﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class SessionAuthorizeAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return (GlobalSettings.oUserData != null && !string.IsNullOrEmpty(GlobalSettings.oUserData.UserId));
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["commonLoginUrl"], false);
    }
}
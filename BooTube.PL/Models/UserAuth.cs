﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooTube.PL.Models
{
    public class UserAuth : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["user"] != null)
                return true;
            else
            {
                httpContext.Response.Redirect("/Sign/Login");
                return false;
            }
        }
    }
}
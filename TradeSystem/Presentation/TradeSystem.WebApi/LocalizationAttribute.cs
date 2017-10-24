﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace TradeSystem.WebApi
{
    public class LocalizationAttribute : System.Web.Mvc.ActionFilterAttribute
    {
      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
          if (filterContext.RouteData.Values["lang"] != null &&
              !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
          {
              // set the culture from the route data (url)
              var lang = filterContext.RouteData.Values["lang"].ToString();
              Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
          }
          else
          {
              // load the culture info from the cookie
              var cookie = filterContext.HttpContext.Request.Cookies["TradeSystem.WebApi.CurrentUICulture"];
              var langHeader = string.Empty;
              if (cookie != null)
              {
                  // set the culture by the cookie content
                  langHeader = cookie.Value;
                  Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
              }
              else
              {
                  // set the culture by the location if not speicified
                  langHeader = filterContext.HttpContext.Request.UserLanguages[0];
                  Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
              }
              // set the lang value into route data
              filterContext.RouteData.Values["lang"] = langHeader;
          }

          // save the location into cookie
          HttpCookie _cookie = new HttpCookie("TradeSystem.WebApi.CurrentUICulture", Thread.CurrentThread.CurrentUICulture.Name);
          _cookie.Expires = DateTime.Now.AddYears(1);
          filterContext.HttpContext.Response.SetCookie(_cookie);

          base.OnActionExecuting(filterContext);
      }
  }

}
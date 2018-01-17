<%@ WebHandler Language="C#" Class="Tracker" %>

using System;
using System.Web;

public class Tracker : IHttpHandler {

    //This is a handler that tracks page info and records it from a javascript call, this will
    //tie into a class which also will execute from the code behind... if js is disabled, then 
    //I can still get the referrer info...
    public void ProcessRequest (HttpContext context) {
        context.Response.Cache.SetCacheability(HttpCacheability.Private);
        context.Response.Cache.AppendCacheExtension("no-cache, proxy-revalidate, no-cache=\"Set-Cookie\"");
        context.Response.AddHeader("Pragma", "no-cache");
        context.Response.Expires = -1;
        context.Response.ExpiresAbsolute = DateTime.Now.AddMinutes(-1);
        context.Response.Cache.SetNoStore();

        string Page = context.Request.ServerVariables["HTTP_HOST"] + context.Request.ServerVariables["URL"];
        //Request.Path.ToString();
        string userHostAddress = context.Request.UserHostAddress.ToString();
        string userAgent = context.Request.UserAgent.ToString();
        string browser = context.Request.Browser.Browser;
        string crawler = string.Empty;
        if (context.Request.Browser.Crawler != null)
        {
            crawler = context.Request.Browser.Crawler.ToString();
        }
        string MyReferrer = string.Empty;
        if (context.Request.UrlReferrer != null)
        {
            MyReferrer = context.Request.UrlReferrer.ToString();
        }
        XmlManager.InsertPageView(context.Server.MapPath("~/App_Data/PageViews.xml"), Page, userHostAddress, 
            userAgent, browser, crawler, MyReferrer);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
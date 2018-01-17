using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage
{
    string _Page = string.Empty;
    string _userHostAddress = string.Empty;
    string _userAgent = string.Empty;
    string _browser = string.Empty;
    string _crawler = string.Empty;
    string _MyReferrer = string.Empty;

    string Page { 
        get{ return _Page;}
        set{ if(value != null) { _Page = value; } }
    }
    string userHostAddress{
        get { return _userHostAddress; }
        set { if (value != null) { _userHostAddress = value; } }
    }
    string userAgent{
        get { return _userAgent; }
        set { if (value != null) { _userAgent = value; } }
    }
    string browser{
        get { return _browser; }
        set { if (value != null) { _browser = value; } }
    }
    string crawler {
        get { return _crawler; }
        set { if (value != null) { _crawler = value; } }
    }
    string MyReferrer
    {
        get { return _MyReferrer; }
        set { if (value != null) { _MyReferrer = value; } }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page = Request.ServerVariables["HTTP_HOST"] + Request.ServerVariables["URL"];
            //Request.Path.ToString();
            userHostAddress = Request.UserHostAddress.ToString();
            userAgent = Request.UserAgent.ToString();
            browser = Request.Browser.Browser;
            if (Request.Browser.Crawler != null)
            {
                crawler = Request.Browser.Crawler.ToString();
            }
            if (Request.UrlReferrer != null)
            {
                MyReferrer = Request.UrlReferrer.ToString();
            }
            XmlManager.InsertPageView(Server.MapPath("~/App_Data/PageViews.xml"), Page, userHostAddress,
                userAgent, browser, crawler, MyReferrer);
            pnlArchive.SetRenderMethodDelegate(RenderCustom);
            pnlScripts.SetRenderMethodDelegate(RenderScripts);

        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
        }
    }

    protected void RenderCustom(System.Web.UI.HtmlTextWriter writer, System.Web.UI.Control Container)
    {
        Dictionary<string, string> Dates = new Dictionary<string, string>();
        MeaService ms = new MeaService();
        List<BlogPost> Posts = ms.GetBlogPosts();

        foreach(BlogPost Post in Posts)
        {
            DateTime PostDate;
            if (DateTime.TryParse(Post.DatePublished.ToString(), out PostDate))
            {
                if (!Dates.ContainsKey(String.Format("{0:yyyy/MM}", PostDate)))
               {
                   Dates.Add(String.Format("{0:yyyy/MM}", PostDate), String.Format("{0:MMMM yyyy}", PostDate));
               }
            }
        }
        writer.Write("<ul>");
        foreach (string link in Dates.Keys)
        {
            writer.Write("<li><a href=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "archive/" + link  + 
                "\">" + Dates[link] + "</a></li>");
        }
        writer.Write("</ul>");
    }

    protected void RenderScripts(System.Web.UI.HtmlTextWriter writer, System.Web.UI.Control Container)
    {
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/json2.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/jquery-1.3.2.min.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shCore.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushBash.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushCpp.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushCSharp.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushCss.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushDiff.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushJScript.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushPlain.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushSql.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushVb.js\"></script>");
        writer.Write("<script type=\"text/javascript\" src=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/") + "scripts/shBrushXml.js\"></script>");
    }
}

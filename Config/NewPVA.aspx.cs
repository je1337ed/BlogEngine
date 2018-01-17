using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Config_NewPVA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            pnlContentWrapper.SetRenderMethodDelegate(RenderCustom);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
        }
    }

    protected void RenderCustom(System.Web.UI.HtmlTextWriter writer, System.Web.UI.Control Container)
    {
        MeaService ms = new MeaService();
        List<PageView> pageViews = XmlManager.GetPageViews(Server.MapPath("~/App_Data/PageViews.xml"));
        var MatchedPageViews = from pageView1 in pageViews
                               orderby pageView1.DateVisited descending
                               select pageView1 ;

        string TempIpAddy = "";
        foreach (PageView pageView in MatchedPageViews.Take(100))
        {
            string ConcatedInfo = "";
            //if (TempIpAddy.Length == 0)
            //{
            //    ConcatedInfo = "<tab;e>";
            //}
            if(TempIpAddy != pageView.UserHostAddress)
            {
                TempIpAddy = pageView.UserHostAddress;
                ConcatedInfo += "<br />";
                ConcatedInfo += " Host Address: " + pageView.UserHostAddress + "<br />";
                ConcatedInfo += " User Agent: " + pageView.UserAgent + "<br />";
                ConcatedInfo += "  Browser: " + pageView.Browser + "<br />";
                ConcatedInfo += " crawler: " + pageView.Crawler + "<br />";
                ConcatedInfo += "On " + pageView.DateVisited.ToString() + " ";
                ConcatedInfo += pageView.Referrer.Length == 0 ? 
                    " The User Came directly to " + pageView.PageUrl + "<br />" 
                    : " The User Came From " + pageView.Referrer + " To " + pageView.PageUrl + "<br />";
            }
            else //it's repeats for the same ip addy
            {
                ConcatedInfo += "On " + pageView.DateVisited.ToString() + " ";
                ConcatedInfo += pageView.Referrer.Length == 0 ?
                    " The User Came directly to " + pageView.PageUrl + "<br />"
                    : " The User Came From " + pageView.Referrer + " To " + pageView.PageUrl + "<br />";
            }
            writer.Write(ConcatedInfo);
        }
    }

}

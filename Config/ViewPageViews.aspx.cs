using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Config_ViewPageViews : System.Web.UI.Page
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
                               select pageView1;
        foreach (PageView pageView in MatchedPageViews)
        {
            string ConcatedInfo = " Page Url: " + pageView.PageUrl + "<br />";
            ConcatedInfo += " Host Address: " + pageView.UserHostAddress + "<br />";
            ConcatedInfo += " User Agent: " + pageView.UserAgent + "<br />";
            ConcatedInfo += "  Browser: " + pageView.Browser + "<br />";
            ConcatedInfo += " crawler: " + pageView.Crawler + "<br />";
            ConcatedInfo += " Referrer: " + pageView.Referrer + "<br />";
            ConcatedInfo += " Referrer: " + pageView.DateVisited.ToString() + "<br /><br />";
            writer.Write(ConcatedInfo);
        }
    }

}

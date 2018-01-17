using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InProgress : System.Web.UI.Page
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
        List<InProgressPost> PostsFromDB = ms.GetInProgressPosts();
        IEnumerable<InProgressPost> Posts = from post in PostsFromDB
                                            orderby DateTime.Parse(post.DatePublished) descending
                                            select post;
        foreach (InProgressPost Post in Posts)
        {
            writer.Write("<div class=\"ContentTitleCell\">" + String.Format("{0:dddd, MMMM d, yyyy}",
                DateTime.Parse(Post.DatePublished.ToString())) + "</div>");
            writer.Write("<div class=\"ContentCell\">" + "<h3><a href=\"" +
                System.Web.VirtualPathUtility.ToAbsolute("~/") + Post.Link.ToString() + "\" style=\"text-decoration:none;\">" +
                Post.Title.ToString() + "</a></h3>" + Post.Body.ToString() + "</div>");
        }
    }
}

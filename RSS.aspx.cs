using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class RSS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        MeaService ms = new MeaService();
        List<BlogPost> Posts = ms.GetBlogPosts();
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hWriter = new HtmlTextWriter(sw);

        hWriter.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
    "<rss xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
    "xmlns:pingback=\"http://madskills.com/public/xml/rss/module/pingback/\" " +
    "xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\" " +
    "xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\" " +
    "xmlns:slash=\"http://purl.org/rss/1.0/modules/slash/\" " +
    "xmlns:georss=\"http://www.georss.org/georss\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" version=\"2.0\"> " +
    "<channel>" +
    "<title>Jeff Blake's Blog</title>" +
    "<link>http://skynetsoftware.net/</link>" +
    "<description>Lots of awesomeness involving ASP.NET, Javascript, AJAX and other assorted technologies.</description>" +
    "<language>en-us</language>" +
    "<copyright>Skynet Software</copyright>" +
    "<generator>dasGenerator</generator>" +
    "<managingEditor>jeff@skynetsoftware.net</managingEditor>" +
    "<webMaster>jeff@skynetsoftware.net</webMaster> ");

        var MatchedPosts = from Post in Posts
                           orderby DateTime.Parse(Post.DatePublished) descending
                           select Post;
        foreach (BlogPost Post in MatchedPosts)
        {
            hWriter.Write(" <item> " +
        " <title> " + HttpUtility.HtmlEncode(Post.Title.ToString()) + " </title> " +
        " <link>" + System.Web.VirtualPathUtility.ToAbsolute("~/") + Post.Link.ToString() + "</link> " +
        " <pubDate> " + String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Parse(Post.DatePublished.ToString())) + " </pubDate>" +
        " <description> " + HttpUtility.HtmlEncode(Post.Body.ToString()) + " </description>" +
        " </item> ");
        }
        hWriter.Write("</channel></rss>");
        Response.Write(sb.ToString());
    }
}

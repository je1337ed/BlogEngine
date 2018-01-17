using System;
using System.Web.UI;
using System.Data;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;

public partial class InProgressDisplay : Page, IDisplay
{
    public string Year { get; set; }
    public string Month { get; set; }
    public string Day { get; set; }
    public string Topic { get; set; }

    private bool RenderComments = false;
    private long InProgressPostId = 0;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!IsPostBack)
        {
            try
            {
                pnlContentWrapper.SetRenderMethodDelegate(RenderCustom);
                DateTime now = DateTime.Now;
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                int jsDate = (int)(now - unixEpoch).TotalSeconds;
                hfArrivalTime.Value = jsDate.ToString();
            }
            catch (Exception ex)
            {
                string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
                Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            }
        }
        else
        {
            try
            {
                pnlContentWrapper.SetRenderMethodDelegate(RenderCustom);
                pnlComments.Visible = false;
                //lblPostResponse.Text = "Thank you for submitting your comment!";
            }
            catch (Exception ex)
            {
                string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
                Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            }
        }
    }

    protected void RenderCustom(System.Web.UI.HtmlTextWriter writer, System.Web.UI.Control Container)
    {
        MeaService ms = new MeaService();
        List<InProgressPost> InProgressPosts = ms.GetInProgressPosts();
        string filter;
        if (Year != null && Month != null && Day != null)
        {
            filter = "archive/InProgress/" + Year + "/" + Month + "/" + Day + "/" + Topic;
            RenderComments = true;
        }
        else
        {
            filter = "archive/InProgress/" + Year + "/" + Month;
            pnlComments.Visible = false;
        }
        var MatchedPosts = from Post in InProgressPosts where Post.Link.IndexOf(filter) >= 0
                           orderby DateTime.Parse(Post.DatePublished) descending
                           select Post ;
        //MatchedPosts = MatchedPosts.Reverse();
        if (MatchedPosts.Count() == 1)
        {
            InProgressPostId = MatchedPosts.First().Id;
            hfBlogPostId.Value = InProgressPostId.ToString();
            pnlCommentSubmission.Visible = true;
        }
        else
        {
            pnlCommentSubmission.Visible = false;
        }

        foreach (InProgressPost Post in MatchedPosts)
        {
            writer.Write("<div class=\"ContentTitleCell\">" + String.Format("{0:dddd, MMMM d, yyyy}",
                DateTime.Parse(Post.DatePublished.ToString())) + "</div>");
            writer.Write("<div class=\"ContentCell\">" + "<h3><a href=\"" +
                System.Web.VirtualPathUtility.ToAbsolute("~/") + Post.Link.ToString() + "\" style=\"text-decoration:none;\">" +
                Post.Title.ToString() + "</a></h3>" + Post.Body.ToString() + "</div>");
        }
        if (RenderComments)
        {
            pnlCommentWrapper.SetRenderMethodDelegate(RenderCustom2);
        }
    }

    protected void RenderCustom2(System.Web.UI.HtmlTextWriter writer, System.Web.UI.Control Container)
    {
        MeaService ms = new MeaService();
        List<InProgressComment> Comments = ms.GetInProgressCommentsByInProgressPostId(InProgressPostId);
        Comments.Reverse();
        if (Comments != null)
        {
            foreach (InProgressComment Comment in Comments)
            {
                writer.Write("<div class=\"ContentTitleCell\">" + Comment.User.ToString() + "</div>");
                writer.Write("<div class=\"ContentCell\">" + Comment.Comment.ToString() + "</div>");
            }
        }
    }
}

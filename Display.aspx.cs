using System;
using System.Web.UI;
using System.Data;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;

public partial class Display : Page, IDisplay
{
    public string Year { get; set; }
    public string Month { get; set; }
    public string Day { get; set; }
    public string Topic { get; set; }

    private bool RenderComments = false;
    private long BlogPostId = 0;

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

    protected void RenderCustom(System.Web.UI.HtmlTextWriter writer,

                            System.Web.UI.Control Container)
    {
        MeaService ms = new MeaService();
        List<BlogPost> BlogPosts = ms.GetBlogPosts();
        string filter;
        if (Year != null && Month != null && Day != null)
        {
            filter = "archive/" + Year + "/" + Month + "/" + Day + "/" + Topic;
            RenderComments = true;
        }
        else
        {
            filter = "archive/" + Year + "/" + Month;
            pnlComments.Visible = false;
        }
        var MatchedPosts = from Post in BlogPosts where Post.Link.IndexOf(filter) >= 0
                           orderby DateTime.Parse(Post.DatePublished) descending
                           select Post;
        //MatchedPosts = MatchedPosts.Reverse();
        if (MatchedPosts.Count() == 1)
        {
            BlogPostId = MatchedPosts.First().Id;
            hfBlogPostId.Value = BlogPostId.ToString();
            pnlCommentSubmission.Visible = true;
        }
        else
        {
            pnlCommentSubmission.Visible = false;
        }

        foreach(BlogPost Post in MatchedPosts)
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
        List<BlogPostComment> Comments = ms.GetBlogCommentsByBlogPostId(BlogPostId);
        Comments.Reverse();
        if (Comments != null)
        {
        foreach(BlogPostComment Comment in Comments)
        {
            writer.Write("<div class=\"ContentTitleCell\">" + Comment.User.ToString() + "</div>");
            writer.Write("<div class=\"ContentCell\">" + Comment.Comment.ToString() + "</div>");
        }
        }
    }

    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    string link = "archive/" + Year + "/" + Month + "/" + Day + "/" + Topic;
    //    MeaService ms = new MeaService();
    //    if (ms.InsertBlogComment(link, txtName.Text, txtEmail.Text, txtHomePage.Text, txtComment.Text))
    //    {
    //        string Body = txtName.Text + " ( " + txtEmail.Text + " ) added a comment for topic : " + Topic + "." + 
    //            System.Environment.NewLine + txtComment.Text;
    //        SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "Comment Added for topic: " + Topic, Body);
    //    }
    //    else
    //    {
    //        //Response.Write("it worked");
    //    }
    //}

}

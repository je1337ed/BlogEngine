using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for MeaService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MeaService : System.Web.Services.WebService {

    public MeaService () {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //In Progress Post Start
    [WebMethod]
    public List<InProgressPost> GetInProgressPosts()
    {
        try
        {
            return XmlManager.GetInProgressPosts(Server.MapPath("~/App_Data/InProgressPosts.xml"), true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }
    //In Progress Post End

    //In Progress Post Comments Start
    [WebMethod]
    public List<InProgressComment> GetInProgressCommentsByInProgressPostId(long InProgressPostId)
    {
        try
        {
            return XmlManager.GetInProgressCommentsByInProgressPostId(Server.MapPath("~/App_Data/InProgressComments.xml"), 
                InProgressPostId);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    [WebMethod]
    public List<InProgressComment> GetInProgressPostComments()
    {
        try
        {
            return XmlManager.GetInProgressComments(Server.MapPath("~/App_Data/InProgressComments.xml"), true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    [WebMethod]
    public bool InsertInProgressComment(InProgressComment comment)
    {
        try
        {
            if (XmlManager.InsertInProgressPostComment(Server.MapPath("~/App_Data/InProgressComments.xml"),
                comment.InProgressPostId, comment.User, comment.Email, comment.HomePage, comment.Comment))
            {
                string Body = comment.User + " ( " + comment.Email + " ) added a comment for an In Progress Post." +
                    System.Environment.NewLine + comment.Comment;
                Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "Comment Added for In Progress Post: ", 
                    Body);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return false;
        }
    }
    //In Progress Post Comments End

    //Blog Post Start
    [WebMethod]
    public List<BlogPost> GetBlogPosts()
    {
        try
        {
            return XmlManager.GetBlogPosts(Server.MapPath("~/App_Data/BlogPosts.xml"), true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    [WebMethod]
    public string GetBlogPostsStr()
    {
        try
        {
            List<BlogPost> BlogPosts = XmlManager.GetBlogPosts(Server.MapPath("~/App_Data/BlogPosts.xml"));
            return string.Empty;// Utils.SerializeDt(dt);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }
    //Blog Post End

    //Blog Post Comments Start
    [WebMethod]
    public List<BlogPostComment> GetBlogCommentsByBlogPostId(long BlogPostId)
    {
        try
        {
            return XmlManager.GetBlogPostCommentsByBlogPostId(Server.MapPath("~/App_Data/BlogComments.xml"), BlogPostId);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    [WebMethod]
    public List<BlogPostComment> GetBlogComments()
    {
        try
        {
            return XmlManager.GetBlogPostComments(Server.MapPath("~/App_Data/BlogComments.xml"));
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    [WebMethod]
    public bool InsertBlogComment(BlogPostComment comment)
    {
        try
        {
            //long BlogPostId, string User, string Email, string HomePage, string Comment));
            if (XmlManager.InsertBlogPostComment(Server.MapPath("~/App_Data/BlogComments.xml"), comment.BlogPostId,
                comment.User, comment.Email, comment.HomePage, comment.Comment))
            {
                string Body = comment.User + " ( " + comment.Email + " ) added a comment for a Blog Post." +
                    System.Environment.NewLine + comment.Comment;
                Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "Comment Added for Blog Post: ", Body);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return false;
        }
    }
    //Blog Post Comments End
}


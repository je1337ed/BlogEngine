using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;
using System.Xml;
using System.IO;

/// <summary>
/// Summary description for AdminService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AdminService : System.Web.Services.WebService
{
    public AdminService() { }

    #region PageView Data
    [WebMethod(EnableSession = true)]
    public bool ArchivePageViews(string UserName, string PassWord)
    {
        try
        {
            string DecryptedUserName = Utils.AESEncryption.Decrypt(UserName);
            string DecryptedPassWord = Utils.AESEncryption.Decrypt(PassWord);
            if (DecryptedUserName == "jblakey" && DecryptedPassWord == "awesomeo5000")
            {
                return XmlManager.ArchivePageViews(Server.MapPath("~/App_Data/PageViews.xml"));
            }
            else
            {
                string Body = "Someone's attempting to login and failed...: " + Environment.NewLine +
                    " DecryptedUserName: " + DecryptedUserName + " DecryptedPassWord: " + DecryptedPassWord;
                Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
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

    [WebMethod(EnableSession = true)]
    public string DownloadPageViews(string UserName, string PassWord)
    {
        try
        {
            string DecryptedUserName = Utils.AESEncryption.Decrypt(UserName);
            string DecryptedPassWord = Utils.AESEncryption.Decrypt(PassWord);
            if (DecryptedUserName == "jblakey" && DecryptedPassWord == "awesomeo5000")
            {
                string pageViews = XmlManager.DownloadPageViews(Server.MapPath("~/App_Data/PageViews.xml"));
                return pageViews;
            }
            else
            {
                string Body = "Someone's attempting to login and failed...: " + Environment.NewLine +
                    " DecryptedUserName: " + DecryptedUserName + " DecryptedPassWord: " + DecryptedPassWord;
                Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return string.Empty;
        }
    }

    public static List<PageView> GetPageViews(string Path)
    {
        try
        {
            List<PageView> PageViews = new List<PageView>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(PageViews.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                PageViews = (List<PageView>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                return PageViews;
            }
            return null;
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    public static bool InsertPageView(string Path, string PageURL, string userHostAddress, string userAgent, string browser,
        string crawler, string MyReferrer)
    {
        try
        {
            List<PageView> PageViews = new List<PageView>();
            PageView pageView = new PageView();
            pageView.PageUrl = PageURL;
            pageView.UserHostAddress = userHostAddress;
            pageView.UserAgent = userAgent;
            pageView.Browser = browser;
            pageView.Crawler = crawler;
            pageView.Referrer = MyReferrer;
            pageView.DateVisited = DateTime.Now;

            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(PageViews.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                PageViews = (List<PageView>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                PageViews.Reverse();
                PageViews.Add(pageView);
                Serializer.Serialize(objStreamWriter, PageViews);
                objStreamWriter.Close();
            }
            else
            {
                PageViews.Add(pageView);
                Stream PathStream = File.Open(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                Serializer.Serialize(objStreamWriter, PageViews);
                objStreamWriter.Close();
                PathStream.Close();
                PathStream.Dispose();
            }
            return true;
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return false;
        }
    }
    #endregion

    #region InProgressComments
    [WebMethod(EnableSession = true)]
    public bool DeactivateInProgressComment(string InProgressCommentId)
    {
        try
        {
            return XmlManager.ToggleInProgressCommentActive(Server.MapPath("~/App_Data/InProgressComments.xml"),
                InProgressCommentId, false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public bool ActivateInProgressComment(string InProgressCommentId)
    {
        try
        {
            return XmlManager.ToggleInProgressCommentActive(Server.MapPath("~/App_Data/InProgressComments.xml"),
                InProgressCommentId, true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public List<InProgressComment> GetApprovedInProgressComments()
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

    [WebMethod(EnableSession = true)]
    public List<InProgressComment> GetUnApprovedInProgressComments()
    {
        try
        {
            return XmlManager.GetInProgressComments(Server.MapPath("~/App_Data/InProgressComments.xml"), false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return null;
        }
    }
    #endregion

    #region InProgressPosts
    [WebMethod(EnableSession = true)]
    public bool DeactivateInProgressPost(string InProgressPostId)
    {
        try
        {
            return XmlManager.ToggleInProgressPostActive(Server.MapPath("~/App_Data/InProgressPosts.xml"),
                InProgressPostId, false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public bool ActivateInProgressPost(string InProgressPostId)
    {
        try
        {
            return XmlManager.ToggleInProgressPostActive(Server.MapPath("~/App_Data/InProgressPosts.xml"),
                InProgressPostId, true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public List<InProgressPost> GetApprovedInProgressPosts()
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

    [WebMethod]
    public List<InProgressPost> GetInProgressDrafts()
    {
        try
        {
            return XmlManager.GetInProgressPosts(Server.MapPath("~/App_Data/InProgressPosts.xml"), false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return null;
        }
    }

    [WebMethod]
    public bool EditInProgressPost(InProgressPost Post)
    {
        try
        {
            return XmlManager.EditInProgressPost(Server.MapPath("~/App_Data/InProgressPosts.xml"), Post);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod]
    public bool InsertInProgressDraft(InProgressPost Post)
    {
        try
        {
            return XmlManager.InsertInProgressPost(Server.MapPath("~/App_Data/InProgressPosts.xml"), Post, false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }
    #endregion

    #region BlogPosts
    [WebMethod(EnableSession = true)]
    public bool DeactivateBlogPost(string BlogPostId)
    {
        try
        {
            return XmlManager.ToggleBlogPostActive(Server.MapPath("~/App_Data/BlogPosts.xml"), BlogPostId, false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public bool ActivateBlogPost(string BlogPostId)
    {
        try
        {
            return XmlManager.ToggleBlogPostActive(Server.MapPath("~/App_Data/BlogPosts.xml"), BlogPostId, true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod]
    public bool EditBlogPost(BlogPost Post)
    {
        try
        {
            return XmlManager.EditBlogPost(Server.MapPath("~/App_Data/BlogPosts.xml"), Post);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod]
    public bool InsertBlogPostDraft(BlogPost Post)
    {
        try
        {
            //Utils.GetSafeTitleString();
            return XmlManager.InsertBlogPost(Server.MapPath("~/App_Data/BlogPosts.xml"), Post, false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public List<BlogPost> GetApprovedBlogPosts()
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

    [WebMethod(EnableSession = true)]
    public List<BlogPost> GetBlogPostDrafts()
    {
        try
        {
            return XmlManager.GetBlogPosts(Server.MapPath("~/App_Data/BlogPosts.xml"), false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return null;
        }
    }
    #endregion

    #region BlogComments
    [WebMethod(EnableSession = true)]
    public bool DeactivateBlogPostComment(string BlogPostCommentId)
    {
        try
        {
            return XmlManager.ToggleBlogPostCommentActive(Server.MapPath("~/App_Data/BlogComments.xml"),
                BlogPostCommentId, false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public bool ActivateBlogPostComment(string BlogPostCommentId)
    {
        try
        {
            return XmlManager.ToggleBlogPostCommentActive(Server.MapPath("~/App_Data/BlogComments.xml"),
                BlogPostCommentId, true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return false;
        }
    }

    [WebMethod(EnableSession = true)]
    public List<BlogPostComment> GetApprovedBlogComments()
    {
        try
        {
            return XmlManager.GetBlogComments(Server.MapPath("~/App_Data/BlogComments.xml"), true);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
            return null;
        }
    }

    [WebMethod(EnableSession = true)]
    public List<BlogPostComment> GetUnApprovedBlogComments()
    {
        try
        {
            return XmlManager.GetBlogComments(Server.MapPath("~/App_Data/BlogComments.xml"), false);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }
    #endregion

    #region User Functions
    [WebMethod(EnableSession = true)]
    public string ProcessUserLogin(string LoginID, string Password)
    {
        try
        {
            SkynetUser user;
            if (LoginID == "meaflux" && Password == "fluxis8479")
            {
                user = new SkynetUser();
                user.UserName = LoginID;
                user.Password = Password;
                Session["SkynetUser"] = user;
                return "Success";
            }
            else
            {
                return "Unable to log in with that username and password. Please try again.";
            }
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return "An error was encountered while trying to log in. Please try again.";
        }
    }
    #endregion
}


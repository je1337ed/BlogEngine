using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;


/// <summary>
/// Summary description for DataManager
/// </summary>
public static class DataManager
{
    #region New Ideas
    //todo: add create instance stuff to significantly reduce the codebase.
    #endregion

    #region PageView Data

    public static List<PageView> GetPageViews()
    {
        try
        {
            List<PageView> PageViews = new List<PageView>();
            DataTable dt = new DataTable();
            SiteData.DBAccess DBAccess = new SiteData.DBAccess();
            using (SqlCommand dbcmd =
                new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "GetPageViews" })
            {
                dt = DBAccess.OpenDataTable(dbcmd);
                PageViews = PopulateClassListFromDataTable<PageView>(dt);
            }
            return PageViews;
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

            SiteData.DBAccess DBAccess = new SiteData.DBAccess();
            using (SqlCommand dbcmd = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "InsertPageView" })
            {
                //( @id bigint, 
                //@userhostaddress varchar(max), 
                //@pageurl varchar(max), 
                //@useragent varchar(max), 
                //@browser varchar(max), 
                //@crawler varchar(max), 
                //@referrer varchar(max), 
                //@datevisited datetime)

                dbcmd.Parameters.AddWithValue("id", pageView.Id);
                dbcmd.Parameters.AddWithValue("userhostaddress", pageView.UserHostAddress);
                dbcmd.Parameters.AddWithValue("pageurl", pageView.PageUrl);
                dbcmd.Parameters.AddWithValue("useragent", pageView.UserAgent);
                dbcmd.Parameters.AddWithValue("browser", pageView.Browser);
                dbcmd.Parameters.AddWithValue("crawler", pageView.Crawler);
                dbcmd.Parameters.AddWithValue("referrer", pageView.Referrer);
                dbcmd.Parameters.AddWithValue("datevisited", DateTime.Now);

                DBAccess.ExecuteSQL(dbcmd);
            }
            //System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(PageViews.GetType());
            //if (System.IO.File.Exists(Path))
            //{
            //    Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //    StreamReader objStreamReader = new StreamReader(PathStream);
            //    PageViews = (List<PageView>)Serializer.Deserialize(objStreamReader);
            //    objStreamReader.Close();
            //    PathStream.Close();
            //    PathStream.Dispose();
            //    PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
            //    StreamWriter objStreamWriter = new StreamWriter(PathStream);
            //    PageViews.Reverse();
            //    PageViews.Add(pageView);
            //    Serializer.Serialize(objStreamWriter, PageViews);
            //    objStreamWriter.Close();
            //}
            //else
            //{
            //    PageViews.Add(pageView);
            //    Stream PathStream = File.Open(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
            //    StreamWriter objStreamWriter = new StreamWriter(PathStream);
            //    Serializer.Serialize(objStreamWriter, PageViews);
            //    objStreamWriter.Close();
            //    PathStream.Close();
            //    PathStream.Dispose();
            //}
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
    public static bool ToggleInProgressCommentActive(string Path, string InProgressCommentId, bool Active)
    {
        try
        {
            List<InProgressComment> comments = new List<InProgressComment>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                comments = (List<InProgressComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                foreach (InProgressComment comment in comments)
                {
                    if (comment.Id.ToString() == InProgressCommentId)
                    {
                        comment.Active = Active;
                    }
                }
                Serializer.Serialize(objStreamWriter, comments);
                objStreamWriter.Close();
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

    public static List<InProgressComment> GetInProgressComments(string Path, bool Active)
    {
        try
        {
            List<InProgressComment> Comments = new List<InProgressComment>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(Comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                Comments = (List<InProgressComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
            }
            var CommentDrafts = from draft in Comments where draft.Active == Active select draft;
            return CommentDrafts.ToList<InProgressComment>();
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    public static List<InProgressComment> GetInProgressCommentsByInProgressPostId(string Path, long InProgressPostId)
    {
        try
        {
            List<InProgressComment> Comments = new List<InProgressComment>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(Comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                List<InProgressComment> AllComments = (List<InProgressComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                var MatchingComments = from Comment in AllComments
                                       where Comment.InProgressPostId == InProgressPostId && Comment.Active == true
                                       select Comment;
                if (MatchingComments.Count() > 0)
                {
                    Comments = MatchingComments.ToList<InProgressComment>();
                }
            }
            return Comments;
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    public static bool InsertInProgressPostComment(string Path, long InProgressPostId, string User, string Email,
        string HomePage, string Comment)
    {
        try
        {
            List<InProgressComment> comments = new List<InProgressComment>();
            InProgressComment comment = new InProgressComment();
            comment.Id = Generate.NewUniqueID;
            comment.InProgressPostId = InProgressPostId;
            comment.User = User;
            comment.Email = Email;
            comment.HomePage = HomePage;
            comment.Comment = Comment;
            comment.DatePublished = DateTime.Now.ToString();
            comment.Active = false;
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                comments = (List<InProgressComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                comments.Reverse();
                comments.Add(comment);
                Serializer.Serialize(objStreamWriter, comments);
                objStreamWriter.Close();
            }
            else
            {
                comments.Add(comment);
                Stream PathStream = File.Open(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                Serializer.Serialize(objStreamWriter, comments);
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

    #region InProgressPosts
    public static bool EditInProgressPost(string Path, InProgressPost Post)
    {
        try
        {
            List<InProgressPost> posts = new List<InProgressPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<InProgressPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                foreach (InProgressPost PostToEdit in posts)
                {
                    if (PostToEdit.Id == Post.Id)
                    {
                        PostToEdit.Title = Post.Title;
                        PostToEdit.Body = Post.Body;
                        PostToEdit.Link = "archive/InProgress/" + String.Format("{0:yyyy/MM/dd}", DateTime.Now) + "/" +
                            Utils.GetSafeTitleString(Post.Title);
                        PostToEdit.DateModified = DateTime.Now.ToString();
                    }
                }
                Serializer.Serialize(objStreamWriter, posts);
                objStreamWriter.Close();
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

    public static bool InsertInProgressPost(string Path, InProgressPost Post, bool Active)
    {
        try
        {
            List<InProgressPost> posts = new List<InProgressPost>();
            Post.Id = Generate.NewUniqueID;
            Post.Link = "archive/InProgress/" + String.Format("{0:yyyy/MM/dd}", DateTime.Now) + "/" +
                            Utils.GetSafeTitleString(Post.Title);
            Post.DatePublished = DateTime.Now.ToString();
            Post.Active = false;
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<InProgressPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                posts.Reverse();
                posts.Add(Post);
                Serializer.Serialize(objStreamWriter, posts);
                objStreamWriter.Close();
            }
            else
            {
                posts.Add(Post);
                Stream PathStream = File.Open(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                Serializer.Serialize(objStreamWriter, posts);
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

    public static bool ToggleInProgressPostActive(string Path, string InProgressPostId, bool Active)
    {
        try
        {
            List<InProgressPost> posts = new List<InProgressPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {

                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<InProgressPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                foreach (InProgressPost post in posts)
                {
                    if (post.Id.ToString() == InProgressPostId)
                    {
                        post.Active = Active;
                    }
                }
                Serializer.Serialize(objStreamWriter, posts);
                objStreamWriter.Close();
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

    public static List<InProgressPost> GetInProgressPosts(string Path, bool Active)
    {
        try
        {
            List<InProgressPost> posts = new List<InProgressPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<InProgressPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
            }
            var PostDrafts = from draft in posts where draft.Active == Active select draft;

            return PostDrafts.ToList<InProgressPost>();
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }
    #endregion

    #region BlogPosts
    public static bool EditBlogPost(string Path, BlogPost Post)
    {
        try
        {
            List<BlogPost> posts = new List<BlogPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {

                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<BlogPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                foreach (BlogPost PostToEdit in posts)
                {
                    if (PostToEdit.Id == Post.Id)
                    {
                        PostToEdit.Title = Post.Title;
                        PostToEdit.Body = Post.Body;
                        PostToEdit.Link = "archive/" + String.Format("{0:yyyy/MM/dd}", DateTime.Now) + "/" +
                            Utils.GetSafeTitleString(Post.Title);
                        PostToEdit.Description = Post.Description;
                        PostToEdit.DateModified = DateTime.Now.ToString();
                    }
                }
                Serializer.Serialize(objStreamWriter, posts);
                objStreamWriter.Close();
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

    public static bool InsertBlogPost(string Path, BlogPost post, bool Active)
    {
        try
        {
            List<BlogPost> posts = new List<BlogPost>();
            post.Id = Generate.NewUniqueID;
            //post.Title = Title;
            //post.Description = Description;
            //post.Body = Body;
            post.DatePublished = DateTime.Now.ToString();
            post.Link = "archive/" + String.Format("{0:yyyy/MM/dd}", DateTime.Now) + "/" + Utils.GetSafeTitleString(post.Title);
            post.Active = Active;
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {

                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<BlogPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                //posts.Reverse(); 
                posts.Add(post);
                Serializer.Serialize(objStreamWriter, posts);
                objStreamWriter.Close();
            }
            else
            {
                posts.Add(post);
                Stream PathStream = File.Open(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                Serializer.Serialize(objStreamWriter, posts);
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

    public static bool ToggleBlogPostActive(string Path, string BlogPostId, bool Active)
    {
        try
        {
            List<BlogPost> posts = new List<BlogPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {

                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<BlogPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                foreach (BlogPost post in posts)
                {
                    if (post.Id.ToString() == BlogPostId)
                    {
                        post.Active = Active;
                    }
                }
                Serializer.Serialize(objStreamWriter, posts);
                objStreamWriter.Close();
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

    public static List<BlogPost> GetBlogPosts(string Path, bool Active)
    {
        try
        {
            List<BlogPost> posts = new List<BlogPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                //string blah = objStreamReader.ReadToEnd();
                posts = (List<BlogPost>)Serializer.Deserialize(objStreamReader);

                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
            }
            var PostDrafts = from draft in posts where draft.Active == Active select draft;

            return PostDrafts.ToList<BlogPost>();
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    public static BlogPost GetBlogPostsById(string Path, long BlogPostId)
    {
        try
        {
            BlogPost Post = null;
            List<BlogPost> Posts = new List<BlogPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(Posts.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                Posts = (List<BlogPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                var MatchingPosts = from post in Posts
                                    where post.Id == BlogPostId && post.Active == true
                                    select post;
                if (MatchingPosts.Count() > 0)
                {
                    Post = MatchingPosts.First();
                }
            }
            return Post;
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    public static List<BlogPost> GetBlogPosts(string Path)
    {
        try
        {
            List<BlogPost> posts = new List<BlogPost>();
            List<BlogPost> PostsToReturn = new List<BlogPost>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(posts.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                posts = (List<BlogPost>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
            }
            var MatchingPosts = from post in posts
                                where post.Active == true
                                select post;
            if (MatchingPosts.Count() > 0)
            {
                PostsToReturn = MatchingPosts.ToList<BlogPost>();
            }
            return PostsToReturn;
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
    public static bool ToggleBlogPostCommentActive(string Path, string BlogPostCommentId, bool Active)
    {
        try
        {
            List<BlogPostComment> comments = new List<BlogPostComment>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                comments = (List<BlogPostComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                foreach (BlogPostComment comment in comments)
                {
                    if (comment.Id.ToString() == BlogPostCommentId)
                    {
                        comment.Active = Active;
                    }
                }
                Serializer.Serialize(objStreamWriter, comments);
                objStreamWriter.Close();
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

    public static bool InsertBlogPostComment(string Path, long BlogPostId, string User, string Email,
        string HomePage, string Comment)
    {
        try
        {
            List<BlogPostComment> comments = new List<BlogPostComment>();
            BlogPostComment comment = new BlogPostComment();
            comment.Id = Generate.NewUniqueID;
            comment.BlogPostId = BlogPostId;
            comment.User = User;
            comment.Email = Email;
            comment.HomePage = HomePage;
            comment.Comment = Comment;
            comment.DatePublished = DateTime.Now.ToString();
            comment.Active = false;
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                comments = (List<BlogPostComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                PathStream = File.Open(Path, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                comments.Reverse();
                comments.Add(comment);
                Serializer.Serialize(objStreamWriter, comments);
                objStreamWriter.Close();
            }
            else
            {
                comments.Add(comment);
                Stream PathStream = File.Open(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter objStreamWriter = new StreamWriter(PathStream);
                Serializer.Serialize(objStreamWriter, comments);
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

    public static List<BlogPostComment> GetBlogComments(string Path, bool Active)
    {
        try
        {
            List<BlogPostComment> Comments = new List<BlogPostComment>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(Comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                Comments = (List<BlogPostComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
            }
            var CommentDrafts = from draft in Comments where draft.Active == Active select draft;
            return CommentDrafts.ToList<BlogPostComment>();
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    public static List<BlogPostComment> GetBlogPostCommentsByBlogPostId(string Path, long BlogPostId)
    {
        try
        {
            List<BlogPostComment> Comments = new List<BlogPostComment>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(Comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                List<BlogPostComment> AllComments = (List<BlogPostComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
                var MatchingComments = from Comment in AllComments
                                       where Comment.BlogPostId == BlogPostId && Comment.Active == true
                                       select Comment;
                if (MatchingComments.Count() > 0)
                {
                    Comments = MatchingComments.ToList<BlogPostComment>();
                }
            }
            return Comments;
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }

    public static List<BlogPostComment> GetBlogPostComments(string Path)
    {
        try
        {
            List<BlogPostComment> Comments = new List<BlogPostComment>();
            System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(Comments.GetType());
            if (System.IO.File.Exists(Path))
            {
                Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objStreamReader = new StreamReader(PathStream);
                Comments = (List<BlogPostComment>)Serializer.Deserialize(objStreamReader);
                objStreamReader.Close();
                PathStream.Close();
                PathStream.Dispose();
            }
            return Comments;
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }
    }
    #endregion

    #region Assorted


    private class PropertyCache
    {
        public PropertyInfo Property;
        public DataColumn Column;
        public Type ColumnDataType;
    }

    public static List<T> PopulateClassListFromDataTable<T>(DataTable dt)
    {
        try
        {
            List<T> ClassList = new List<T>();
            List<PropertyCache> PopulatedMemberProperties = new List<PropertyCache>();

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (pi.Name.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        PopulatedMemberProperties.Add(new PropertyCache { Property = pi, Column = column, ColumnDataType = column.DataType });
                        break;
                    }
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                T NewT = Activator.CreateInstance<T>();
                foreach (PropertyCache pc in PopulatedMemberProperties)
                {
                    if (pc.Property.PropertyType == typeof(decimal))
                    {
                        try
                        {
                            if (dr[pc.Column] != DBNull.Value)
                                pc.Property.SetValue(NewT, (decimal)dr[pc.Column], null);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            if (dr[pc.Column] != DBNull.Value)
                                pc.Property.SetValue(NewT, dr[pc.Column], null);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                ClassList.Add(NewT);
            }
            return ClassList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string CreateNewXMLStore(string Path, string DBName, string columns)
    {
        string DbPath = string.Empty;
        try
        {
            if (columns.Contains(','))
            {
                DataRow dr = default(DataRow);
                DataSet ds = new DataSet();
                if (System.IO.File.Exists(Path))
                {
                    //ds.ReadXml(Path);
                    return "EA";
                }
                else
                {
                    DataTable dt = new DataTable("Posts");
                    string[] ColumnListing = columns.Split(',');
                    foreach (string Column in ColumnListing)
                    {
                        if (Column.Trim().Length > 0)
                            dt.Columns.Add(Column);
                    }
                    ds.Tables.Add(dt);
                }
                //dr = ds.Tables["Posts"].NewRow();
                //dr["Title"] = Title;
                //dr["Description"] = Description;
                //dr["Body"] = Body;
                //dr["Link"] = Link;
                //dr["PubDate"] = PubDate;
                //ds.Tables["Posts"].Rows.Add(dr);
                //ds.WriteXml(Path, XmlWriteMode.WriteSchema);
                return DbPath;
            }
            else
            {
                return "IA";
            }
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return "EE";
        }
    }
    #endregion

    #region Deprecated
    //public static BlogPost GetBlogPostsByLink(string Path, string Link)
    //{
    //    try
    //    {
    //        BlogPost Post = null;
    //        List<BlogPost> Posts = new List<BlogPost>();
    //        System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(Posts.GetType());
    //        if (System.IO.File.Exists(Path))
    //        {
    //            Stream PathStream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    //            StreamReader objStreamReader = new StreamReader(PathStream);
    //            Posts = (List<BlogPost>)Serializer.Deserialize(objStreamReader);
    //            objStreamReader.Close();
    //            PathStream.Close();
    //            PathStream.Dispose();
    //            var MatchingPosts = from post in Posts
    //                                where post.Link == Link && post.Active == true
    //                                select post;
    //            if (MatchingPosts.Count() > 0)
    //            {
    //                Post = MatchingPosts.First();
    //            }
    //        }
    //        return Post;
    //    }
    //    catch (Exception ex)
    //    {
    //        string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
    //        Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
    //        return null;
    //    }
    //}
    #endregion
}

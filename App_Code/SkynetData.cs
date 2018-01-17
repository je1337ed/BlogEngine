using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class Generate
{
    /// <summary>
    /// Generate a generated ID from the date and time
    /// </summary>
    public static long NewUniqueID
    {
        // create a read only unique ID
        get
        {
            return long.Parse(String.Format("{0}{1}{2}{3}{4}{5}{6}", DateTime.Now.Day, DateTime.Now.Month,
                DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond));
        }
    }
}

public class PageView
{
    public long Id { get; set; }
    public string UserHostAddress { get; set; }
    public string PageUrl { get; set; }
    public string UserAgent { get; set; }
    public string Browser { get; set; }
    public string Crawler { get; set; }
    public string Referrer { get; set; }
    public DateTime DateVisited { get; set; }
}

public class InProgressPost
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public string DatePublished { get; set; }
    public string DateModified { get; set; }
    public bool Active { get; set; }
}

public class InProgressComment
{
    public long Id { get; set; }
    public long InProgressPostId { get; set; }
    public string User { get; set; }
    public string Email { get; set; }
    public string HomePage { get; set; }
    public string Comment { get; set; }
    public string DatePublished { get; set; }
    public string DateModified { get; set; }
    public bool Active { get; set; }
}

public class BlogPost
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public string DatePublished { get; set; }
    public string DateModified { get; set; }
    public bool Active { get; set; }
}

public class BlogPostComment
{
    public long Id { get; set; }
    public long BlogPostId { get; set; }
    public string User { get; set; }
    public string Email { get; set; }
    public string HomePage { get; set; }
    public string Comment { get; set; }
    public string DatePublished { get; set; }
    public string DateModified { get; set; }
    public bool Active { get; set; }
}

public class PostTags
{
    public long PostId { get; set; }
    public long TagId { get; set; }
    public bool Active { get; set; }
}

public class Tags
{
    public long Id { get; set; }
    public string Tag { get; set; }
    public bool Active { get; set; }
}

public class SkynetUser
{
    private string _UserName;
    private string _Password;
    private long _Id;

    public long Id { get { return _Id; } set { _Id = value; } }
    public string UserName { get { return _UserName; } set { _UserName = value; } }
    public string Password { get { return _Password; } set { _Password = value; } }
}

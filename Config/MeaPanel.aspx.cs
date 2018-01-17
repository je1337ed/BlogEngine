using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_MeaPanel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SkynetUser"] == null)
        {
            Response.Redirect("login.aspx");
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        AdminService AdSer = new AdminService();
        BlogPost post = new BlogPost();
        post.Active = false;
        post.Title = tbTitle.Text;
        post.Description = tbDescription.Text;
        post.Body = tbBody.Text;
        post.Link = tbLink.Text;
        AdSer.InsertBlogPostDraft(post);
    }
}

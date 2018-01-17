using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class Contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void submitbutton_Click(object sender, EventArgs e)
    {
        try
        {
            pnl1.Visible = false;
            lblMessage.Text = "Your message has been sent.";
            string Body = tbName.Text + " ( " + tbEmail.Text + " ) sent : " + System.Environment.NewLine + tbMessage.Text;
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", tbSubject.Text, Body);
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
        }
    }
}

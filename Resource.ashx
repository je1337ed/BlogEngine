<%@ WebHandler Language="C#" Class="Resource" %>
using System;
using System.Web;
using System.Data;
using System.Web.SessionState;

public class Resource : IHttpHandler, IReadOnlySessionState, IDisplay2
{
    public string FileName { get; set; }
    
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string file = context.Server.MapPath(FileName);
            string filename = file.Substring(file.LastIndexOf('\\') + 1);
            string extension = file.Substring(file.LastIndexOf('.') + 1);

            //context.Response.Cache.SetExpires(DateTime.Now.AddMonths(5));
            //context.Response.Cache.SetCacheability(HttpCacheability.Public);
            //context.Response.Cache.SetValidUntilExpires(false);

            string FileExtension = FileName.Substring(FileName.LastIndexOf('.') + 1);

            switch (FileExtension)
            {
                case "gif":
                    context.Response.ContentType = "image/gif";
                    context.Response.AddHeader("content-disposition", "inline; filename=" + filename);
                    context.Response.WriteFile(file);
                    break;
                case "jpg":
                    context.Response.ContentType = "image/jpeg";
                    context.Response.AddHeader("content-disposition", "inline; filename=" + filename);
                    context.Response.WriteFile(file);
                    break;
                case "png":
                    context.Response.ContentType = "image/png";
                    context.Response.AddHeader("content-disposition", "inline; filename=" + filename);
                    context.Response.WriteFile(file);
                    break;
                case "css":
                    Utils.GZResponse(context);
                    context.Response.ContentType = "text/css";
                    string OrigionalCSS = System.IO.File.ReadAllText(file);
                    string MinifiedCSS = CSSMinify.CSSMinify.Minify(OrigionalCSS);
                    context.Response.AddHeader("content-disposition", "inline; filename=" + filename);
                    //context.Response.Write(MinifiedCSS);
                    context.Response.Write(OrigionalCSS);
                    break;
                case "js":
                    Utils.GZResponse(context);
                    context.Response.ContentType = "text/js";
                    JavaScriptSupport.JavaScriptMinifier jm = new JavaScriptSupport.JavaScriptMinifier();
                    string OrigionalScript = System.IO.File.ReadAllText(file);
                    string MinifiedScript = jm.MinifyString(OrigionalScript);
                    context.Response.AddHeader("content-disposition", "inline; filename=" + filename);
                    //context.Response.Write(MinifiedScript);
                    context.Response.Write(OrigionalScript);
                    break;
            }
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            context.Response.Write("");
        }
    }
  
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
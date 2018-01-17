<%@ Application Language="C#" %>

<script RunAt="server">
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
    }

    private static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        routes.RouteExistingFiles = true;

        //-----------Blog Posts Start
        routes.Add("Archive", new System.Web.Routing.Route("archive/{year}/{month}/{day}/{topic}", new RouteHandler("~/Display.aspx")));
        routes.Add("ArchiveComments", new System.Web.Routing.Route("archive/{year}/{month}/{day}/{topic}/comments",
            new RouteHandler("~/Display.aspx")));
        routes.Add("ArchiveMonth", new System.Web.Routing.Route("archive/{year}/{month}", new RouteHandler("~/Display.aspx")));
        //-----------Blog Posts End
        
        //-----------In Progress Posts Start
        routes.Add("ArchiveInProgress", new System.Web.Routing.Route("archive/InProgress/{year}/{month}/{day}/{topic}", 
            new RouteHandler("~/DisplayInProgress.aspx")));
        
        routes.Add("ArchiveInProgressComments", 
            new System.Web.Routing.Route("archive/InProgress/{year}/{month}/{day}/{topic}/comments", 
                new RouteHandler("~/DisplayInProgress.aspx")));
        
        routes.Add("ArchiveInProgressMonth", new System.Web.Routing.Route("archive/InProgress/{year}/{month}",
            new RouteHandler("~/DisplayInProgress.aspx")));
        //-----------In Progress Posts End
        
        //-----------Caching Start
        //routes.Add("ResourceHandlerScripts", new System.Web.Routing.Route("Scripts/{FileName}", new RouteHandler2("~/Resource.ashx")));
        //routes.Add("ResourceHandlerStyles", new System.Web.Routing.Route("Styles/{FileName}", new RouteHandler2("~/Resource.ashx")));
        //routes.Add("ResourceHandlerImages", new System.Web.Routing.Route("Images/{FileName}", new RouteHandler2("~/Resource.ashx")));
        //-----------Caching End        
    }
</script>


using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;
using System.Web.Security;
using System.Security;
using System.Net;

    public class RouteHandler : IRouteHandler
    {
        public RouteHandler(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var display = BuildManager.CreateInstanceFromVirtualPath(_virtualPath, typeof(Page)) as IDisplay;
            display.Year = requestContext.RouteData.Values["Year"] as string;
            display.Month = requestContext.RouteData.Values["Month"] as string;
            display.Day = requestContext.RouteData.Values["Day"] as string;
            display.Topic = requestContext.RouteData.Values["Topic"] as string;
            return display;
        }
        string _virtualPath;
    }

    public class RouteHandler2 : IRouteHandler
    {
        public RouteHandler2(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var display = BuildManager.CreateInstanceFromVirtualPath(_virtualPath, typeof(IHttpHandler)) as IDisplay2;
            string Url = ((System.Web.Routing.Route)(requestContext.RouteData.Route)).Url;
            Url = Url.Substring(0, Url.IndexOf('/'));
            display.FileName = requestContext.RouteData.Values["FileName"] as string;

            return display;
        }

        string _virtualPath;
    }

    public interface IDisplay2 : IHttpHandler
    {
        string FileName { get; set; }
    }
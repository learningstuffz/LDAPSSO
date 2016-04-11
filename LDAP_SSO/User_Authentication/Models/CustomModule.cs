using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User_Authentication.Models
{

    public class CustomModule : IHttpModule
    {
        public CustomModule()
        {
        }

        public String ModuleName
        {
            get { return "CustomModule"; }
        }
        string userName;
        
        // In the Init function, register for HttpApplication 
        // events by adding your handlers.
        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
                (new EventHandler(this.Application_BeginRequest));
            application.EndRequest +=
                (new EventHandler(this.Application_EndRequest));
        }

        private void Application_BeginRequest(Object source,
             EventArgs e)
        {
            // Create HttpApplication and HttpContext objects to access
            // request and response properties.
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            bool error = false;
            try
            {
                if (context.Request.IsAuthenticated)
                {
                    userName = context.User.Identity.Name;    
                }
                else
                {
                    userName = "";
                }
                
            }
            catch (Exception ex)
            {

                userName = "";
            }
            if (userName=="")
            {
                string defaulturl="";
                try
                {
                    defaulturl = System.Configuration.ConfigurationManager.AppSettings["defaultUrl"];
                    if (defaulturl != "")
                    {

                        if (Convert.ToString(context.Request.Url).ToUpper().Contains(defaulturl.ToUpper()))
                        {

                        }
                        else
                        {
                            context.Response.Redirect(defaulturl);
                        }
                    }
                   
                }
                catch (Exception ex) 
                {

                    error = true;
                }
                if (error)
                {
                  context.Response.Redirect("http://localhost:29834/");
                }
                
            }
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            //string filePath = context.Request.FilePath;
            //string fileExtension =
            //    VirtualPathUtility.GetExtension(filePath);
            //if (fileExtension.Equals(".aspx"))
            //{
            //    context.Response.Write("<hr><h1><font color=red>" +
            //        "HelloWorldModule: End of Request</font></h1>");
            //}
        }

        public void Dispose() { }
    }
}
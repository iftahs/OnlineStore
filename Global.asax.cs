using OnlneStore.DataAccessLayers;
using OnlneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace OnlneStore
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {             
                        string id = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        int userid = int.Parse(id);

                        DataLayer dal = new DataLayer();
                        User user = (from users in dal.Users where users.UserId == userid select users).ToList<User>()[0];

                        if (user.Admin)
                        {
                            string[] roles = { "admin" };
                            e.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(id, "Forms"), roles);
                        }
                        
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }
                }
            }
        }
    }
}

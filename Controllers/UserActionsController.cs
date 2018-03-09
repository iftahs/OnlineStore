using OnlneStore.DataAccessLayers;
using OnlneStore.Models;
using OnlneStore.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.Security.Principal;
using System.Threading;

namespace OnlneStore.Controllers
{
    public class UserActionsController : ApiController
    {
        //Post method for login
        [System.Web.Mvc.ActionName("Login")]
        public HttpResponseMessage Login(User user)
        {
            DataLayer dal = new DataLayer();
            List<User> users = (from x in dal.Users where x.Email == user.Email select x).ToList();
            Encryption enc = new Encryption();

            bool validUser = false;
            if (users.Count > 0)
                validUser = enc.ValidatePassword(user.Password, users[0].Password);

            if (validUser == false)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid email or password");
            else
                FormsAuthentication.SetAuthCookie(users[0].UserId.ToString(), true);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //Post method for registration
        [System.Web.Mvc.ActionName("Registration")]
        public HttpResponseMessage Registration(User user)
        {
            DataLayer dal = new DataLayer();
            List<User> users = (from x in dal.Users where x.Email == user.Email select x).ToList();
            Encryption enc = new Encryption();

            user.RegistrationDate = DateTime.Now;
            user.Admin = false;

            //Check if email already exist.
            if (users.Count > 0)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Email already exist");

            string hashedPassword = enc.CreateHash(user.Password);
            user.Password = hashedPassword;

            try
            {
                dal.Users.Add(user);
                dal.SaveChanges();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [System.Web.Mvc.ActionName("GetUserFromCookie")]
        public User GETUserFromCookie()
        {
            DataLayer dal = new DataLayer();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                int id = int.Parse(HttpContext.Current.User.Identity.Name);
                User user = (from users in dal.Users where users.UserId == id select users).ToList<User>()[0];
                return user;
            }
            else
                throw new Exception("User is not loged in");
        }

        [System.Web.Mvc.ActionName("Logout")]
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}

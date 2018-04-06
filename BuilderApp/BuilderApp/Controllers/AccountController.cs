using BuilderApp.Models;
using BuilderApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Owin;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace Builder.Controllers

{

    public class AccountController : Controller
     {
        //protected string googleplus_client_id = "994319344086-vm55145t6lkum0lbgise8944r7d320q5.apps.googleusercontent.com";
        //protected string googleplus_client_sceret = "Va1IcgOHZIG8PzM9K9_SeHw";                                                
        //protected string googleplus_redirect_url = "http://localhost:50951";
        //protected string Parameters;

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication(
                 clientId: "000-000.apps.googleusercontent.com",
                 clientSecret: "00000000000");
        }

        [HttpPost]
        [ActionName("RegisterCompany")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCompany(AccountCompanyViewModel _model)
        {
            ViewBag.PageNumber = 1;
            if (ModelState.IsValid)
            {
                   using (BuilderDBEntities db = new BuilderDBEntities())
                    {
                        var compEmail = db.Companies.FirstOrDefault(x => x.email == _model.email);
                        var compLogin = db.Companies.FirstOrDefault(x => x.login == _model.login);
                        var custEmail = db.Customers.FirstOrDefault(x => x.email == _model.email);
                        var custLogin = db.Customers.FirstOrDefault(x => x.login == _model.login);
                        if ((compEmail == null) && (compLogin == null) && (custEmail == null) && (custLogin == null))
                        {
                            Company company = new Company();
                            company.companyName = _model.companyName;
                            company.login = _model.login;
                            company.phoneNumber = _model.phoneNumber;
                            company.email = _model.email;
                            company.password = Security.sha512encrypt(_model.password);
                            company.role_id = 1;
                            company.isDelete = false;
                            db.Companies.Add(company);
                            db.SaveChanges();
                            return RedirectToAction("Login");
                        }
                        else if ((compEmail != null) || (custEmail != null))
                        {
                            ModelState.AddModelError("Email", "Użytkownik o podanym emailu już istnieje");
                        }
                        else if ((compLogin != null) || (custLogin != null))
                        {
                            ModelState.AddModelError("Login", "Użytkownik o podanym loginie już istnieje");
                        }
                    }
            }
            
            return View(_model);
        }


        [HttpGet]
        [ActionName("RegisterCustomer")]
        public ActionResult RegisterCustomer()
        {
            return View();
        }


        [HttpPost]
        [ActionName("RegisterCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCustomer(AccountCustomerViewModel _model)
        {
            ViewBag.PageNumber = 1;
            if (ModelState.IsValid)
            {
               
                    using (BuilderDBEntities db = new BuilderDBEntities())
                    {
                        var compEmail = db.Companies.FirstOrDefault(x => x.email == _model.email);
                        var compLogin = db.Companies.FirstOrDefault(x => x.login == _model.login);
                        var custEmail = db.Customers.FirstOrDefault(x => x.email == _model.email);
                        var custLogin = db.Customers.FirstOrDefault(x => x.login == _model.login);
                        if ((compEmail == null) && (compLogin == null) && (custEmail == null) && (custLogin == null))
                        {
                            Customer customer = new Customer();
                            customer.name = _model.name;
                            customer.surname = _model.surname;
                            customer.login = _model.login;
                            customer.phoneNumber = _model.phoneNumber;
                            customer.email = _model.email;
                            customer.password = Security.sha512encrypt(_model.password);
                            customer.role_id = 1;
                            customer.isDelete = false;
                            db.Customers.Add(customer);
                            db.SaveChanges();
                            return RedirectToAction("Login");
                        }
                        else if ((compEmail != null) || (custEmail != null))
                        {
                            ModelState.AddModelError("Email", "Użytkownik o podanym emailu już istnieje");
                        }
                        else if ((compLogin != null) || (custLogin != null))
                        {
                            ModelState.AddModelError("Login", "Użytkownik o podanym loginie już istnieje");
                        }
                    }
               }
            
            return View(_model);
        }

        [HttpGet]
        [ActionName("LoginCustomer")]
        [Route("Login")]
        public ActionResult LoginCustomer()
        {
            return View();
        }



        [HttpPost]
        [ActionName("Login")]
        public ActionResult LoginCustomer(AccountCustomerViewModel _model)
        {
            using (BuilderDBEntities db = new BuilderDBEntities())
            {
                bool validEmail = db.Customers.Any(x => x.email == _model.email);
                bool validLogin = db.Customers.Any(x => x.login == _model.login);

                if (!(validEmail || validLogin))
                {
                    ModelState.AddModelError("Password", "Niepoprawny login lub hasło");
                    return View(_model);
                }

                _model.password = Security.sha512encrypt(_model.password); 
          
               Customer customer = db.Customers.FirstOrDefault(u => u.login.Equals(_model.password) && u.password.Equals(_model.password));

               string authId = Guid.NewGuid().ToString();
            
                Session["AuthID"] = authId;
                var cookie = new HttpCookie("AuthID");
                cookie.Value = authId;
                Response.Cookies.Add(cookie);                       
         
                    if (customer != null)
                    {
                        FormsAuthentication.SetAuthCookie(customer.login, false);
                        var authTicket = new FormsAuthenticationTicket(1, customer.login , DateTime.UtcNow, DateTime.UtcNow.AddMinutes(60), false, "");
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                        authCookie.Expires = DateTime.UtcNow.AddMinutes(60);
                        Response.SetCookie(authCookie);
                        return RedirectToAction("Home", "Account");
                    }
                return View(_model);
                           
            }

        }


        public ActionResult Index()
        {
            return View();
        }
        
    }
}
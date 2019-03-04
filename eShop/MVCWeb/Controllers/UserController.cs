using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using MVCWeb.Cores;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.Security;
using MVCWeb.Libraries;
using MVCWeb.Models;
using Newtonsoft.Json;

namespace MVCWeb.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var encryptedPassword = (model.Password + Constant.PasswordSuffix).ToMD5();
            var db = new DbAppContext();
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == encryptedPassword);
                if (user != null)
                {
                    var roles = new[] { user.Role };
                    var serializeModel = new CustomPrincipalSerializeModel
                    {
                        UserId = user.Id,
                        DisplayName = user.DisplayName,
                        Roles = roles
                    };
					var timeOut = model.RememberMe ? DateTime.Now.AddMonths(2) : DateTime.Now.AddMinutes(60);
                    var userData = JsonConvert.SerializeObject(serializeModel);
                    var authTicket = new FormsAuthenticationTicket(
                             1,
                            user.Username,
                             DateTime.Now,
                             timeOut,
                             true,
                             userData);
                    
                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    faCookie.Expires = timeOut;
                    Response.Cookies.Add(faCookie);

                    return RedirectToAction("Edit", "Order");
                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }
            return View();
        }
        [CustomAuthorize(Roles = "*")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [CustomAuthorize(Roles = "*")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var db = new DbAppContext();
            var encryptedPassword = (model.OldPassword + Constant.PasswordSuffix).ToMD5();
            var userName = User.Identity.GetUserName();
            var user = db.Users.FirstOrDefault(o => o.Username == userName && o.Password == encryptedPassword);
            if (user == null)
                return Json(new ReturnData { Success = false, Message = "Mật khẩu cũ không đúng!" });
            if (model.NewPassword != model.ConfirmNewPassword)
                return Json(new ReturnData { Success = false, Message = "Xác nhận mật khẩu mới không đúng!" });
            user.Password = (model.NewPassword + Constant.PasswordSuffix).ToMD5();
            db.SaveChanges();
            return Json(new ReturnData { Success = true, Message = "Đã đổi mật khẩu thành công!" });
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
    }
}

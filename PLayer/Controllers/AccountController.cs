using DALayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PLayer.Helper;
using PLayer.Models;

namespace PLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region SIGN UP
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Agree = model.Agree,
                    UserName = model.Email.Split('@')[0],

                };
                var Result = await _userManager.CreateAsync(user, model.Password);
                if (Result is not null)
                {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in Result.Errors)

                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);

        }
        #endregion

        #region SIGN IN
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var Result = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (Result)
                    {
                        var LoginUser = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (LoginUser.Succeeded)
                            return RedirectToAction("Index", "Home");

                    }

                }
                ModelState.AddModelError(" ", "Email Or Password Not Valid ");

            }
            return View(model);
        }
        #endregion

        #region SIGN OUT
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

   
        #region FORGET PASSWORD
        public IActionResult ForgetPassword()
        {
            return View();
        }
        #endregion

        #region Send Email

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //send email
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email, Token = token }, Request.Scheme);
                    var email = new Email()
                    {

                        To = model.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink

                    };
                    EmailSetting.SendEmail(email);

                    return RedirectToAction(nameof(CheckedYourInbox));

                }
                ModelState.AddModelError(string.Empty, "Email is not Exist");
            }

            return View("ForgetPassword", model);
        }

        #endregion

        #region CheckedYourInbox
        public IActionResult CheckedYourInbox()
        {
            return View();
        } 
        #endregion

        #region Reset Password
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var Result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (Result.Succeeded)
                        return RedirectToAction(nameof(Login));

                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }


            }
            return View(model);
        }
        #endregion

        #region AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        } 
        #endregion

    }
}

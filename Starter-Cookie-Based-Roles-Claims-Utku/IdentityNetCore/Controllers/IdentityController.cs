using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityNetCore.Models;
using IdentityNetCore.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Signup()
        {
            var model = new SignupViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(model.Email) == null)
                {
                    var user = new IdentityUser
                    {
                        Email = model.Email,
                        UserName = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    //Kullanıcıyı tekrar çekerek kullanıcının veritabanında yaratıldığından emin oluyoruz.
                    user = await _userManager.FindByEmailAsync(model.Email);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    if (result.Succeeded)
                    {
                        var confirmationLink = Url.ActionLink("ConfirmEmail", "Identity", new {userId = user.Id, @token = token});

                        await _emailSender.SendEmailAsync("utkukaplan06@hotmail.com", user.Email, "Confirm your addess",confirmationLink);

                        return RedirectToAction("Signin");
                    }

                    ModelState.AddModelError("Signup", string.Join("", result.Errors.Select(x => x.Description)));
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Signin");
            }

            return new NotFoundResult();
        }

        public IActionResult Signin()
        {
            return View(new SigninViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Signin(SigninViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                //else
                //{
                //    result.IsLockedOut gibi propertyleri kullanarak kontroller yapabiliriz. 
                //    Ama güvenlik açısından mümkün olduğunca geri dönüş hatalarını açıklayıcı şekilde vermemeliyiz.
                //}


                ModelState.AddModelError("Login", "Cannot login.");
            }

            return View(model);
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}

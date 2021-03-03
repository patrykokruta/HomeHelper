using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeHelper.DTO.Account;
using HomeHelper.DTO.Authentication;
using HomeHelper.Services.Account;
using HomeHelper.UI.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHelper.UI.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login(AuthenticationRequest request, string returnUrl = null)
        {
            var result = await _accountService.AuthenticateAsync(request);
            ViewData["message"] = result.Message;

            if (!result.Succeeded)
            {
                AddErrors(result.Errors);
                return View();
            }

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _accountService.RegisterAsync(request);
            ViewData["message"] = result.Message;

            if (!result.Succeeded)
            {
                AddErrors(result.Errors);
                return View();
            }
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            if (await _accountService.IsEmailTakenAsync(email))
            {
                return Json($"Email {email} is already in use.");
            }
            return Json(true);
        }

        private void AddErrors(List<string> errors)
        {
            if (errors == null) return;

            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}
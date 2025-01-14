﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.API.Data;
using Web.API.Helpers;
using Web.API.Models;


namespace Web.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public AccountController(IUserHelper userHelper, DataContext context)
        {
            _userHelper = userHelper;
            _context = context;

        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

                if (ModelState.IsValid)
                {
                var  result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First());
                        }

                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

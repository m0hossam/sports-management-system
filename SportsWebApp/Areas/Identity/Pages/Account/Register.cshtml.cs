﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SportsWebApp.Data;
using SportsWebApp.Models;

namespace SportsWebApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public readonly SelectList UserTypes;
        public readonly SelectList Clubs;
        public readonly SelectList Stadiums;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;

            UserTypes = new SelectList(_roleManager.Roles.OrderBy(x => x.Name).Select(x => x.Name));
            Clubs = new SelectList(_context.Clubs.OrderBy(x => x.Name), "Id", "Name");
            Stadiums = new SelectList(_context.Stadiums.OrderBy(x => x.Name), "Id", "Name");
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "User Type")]
            public string UserType { get; set; } = "System Admin";

            [Required]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Display(Name = "Represented Club")]
            public int? ClubId { get; set; }

            [Display(Name = "Managed Stadium")]
            public int? StadiumId { get; set; }

            [Display(Name = "National ID")]
            public string NationalId { get; set; }

            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public DateTime? DateOfBirth { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }

        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Extra validation for user types ###
                if (Input.UserType == "Club Representative")
                {
                    if (Input.ClubId == null)
                    {
                        ModelState.AddModelError(string.Empty, "The Club field is required for Club Representatives.");
                        return Page();
                    }

                    var club = _context.Clubs.FirstOrDefault(x => x.Id == Input.ClubId);
                    if (club == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid club.");
                        return Page();
                    }

                    var clubRep = _context.ClubRepresentatives.FirstOrDefault(x => x.Club == club);
                    if (clubRep != null)
                    {
                        ModelState.AddModelError(string.Empty, $"There already is a Club Representative for '{club.Name}'.");
                        return Page();
                    }
                }
                if (Input.UserType == "Stadium Manager")
                {
                    if (Input.StadiumId == null)
                    {
                        ModelState.AddModelError(string.Empty, "The Stadium field is required for Stadium Managers.");
                        return Page();
                    }

                    var stadium = _context.Stadiums.FirstOrDefault(x => x.Id == Input.StadiumId);
                    if (stadium == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid stadium.");
                        return Page();
                    }

                    var stadiumManager = _context.StadiumManagers.FirstOrDefault(x => x.Stadium == stadium);
                    if (stadiumManager != null)
                    {
                        ModelState.AddModelError(string.Empty, $"There already is a Stadium Manager for '{stadium.Name}'");
                        return Page();
                    }
                }
                if (Input.UserType == "Fan")
                {
                    if (Input.NationalId == null || Input.PhoneNumber == null || Input.Address == null || Input.DateOfBirth == null)
                    {
                        ModelState.AddModelError(string.Empty, "The National ID, Phone Number, Address and Date of Birth fields are required for Fans.");
                        return Page();
                    }

                    var fan = _context.Fans.FirstOrDefault(x => x.NationalId == Input.NationalId);
                    if (fan != null)
                    {
                        ModelState.AddModelError(string.Empty, $"A fan with the national ID '{fan.NationalId}' already exists.");
                        return Page();
                    }
                }
                // End of extra validation for user types ###

                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);

                    var roleResult = await _userManager.AddToRoleAsync(user, Input.UserType);

                    string returnController = "Home"; // precaution

                    // Create models for user types ###
                    if (Input.UserType == "System Admin")
                    {
                        var systemAdmin = new SystemAdmin
                        {
                            Name = Input.Name,
                            User = user
                        };

                        _context.SystemAdmins.Add(systemAdmin);
                        _context.SaveChanges();
                        returnController = "SystemAdmins";
                    }
                    if (Input.UserType == "Association Manager")
                    {
                        var associationManager = new AssociationManager
                        {
                            Name = Input.Name,
                            User = user
                        };

                        _context.AssociationManagers.Add(associationManager);
                        _context.SaveChanges();
                        returnController = "AssociationManagers";
                    }
                    if (Input.UserType == "Club Representative")
                    {
                        var club = _context.Clubs.Find(Input.ClubId);
                        var clubRepresentative = new ClubRepresentative
                        {
                            Name = Input.Name,
                            User = user,
                            ClubId = club.Id,
                        };

                        _context.ClubRepresentatives.Add(clubRepresentative);
                        _context.SaveChanges();
                        returnController = "ClubRepresentatives";
                    }
                    if (Input.UserType == "Stadium Manager")
                    {
                        var stadium = _context.Stadiums.Find(Input.StadiumId);
                        var stadiumManager = new StadiumManager
                        {
                            Name = Input.Name,
                            User = user,
                            StadiumId = stadium.Id, 
                        };

                        _context.StadiumManagers.Add(stadiumManager);
                        _context.SaveChanges();
                        returnController = "StadiumManagers";
                    }
                    if (Input.UserType == "Fan")
                    {
                        var fan = new Fan
                        {
                            Name = Input.Name,
                            User = user,
                            NationalId = Input.NationalId,
                            Phone = Input.PhoneNumber,
                            Address = Input.Address,
                            DateOfBirth = Input.DateOfBirth
                        };

                        _context.Fans.Add(fan);
                        _context.SaveChanges();
                        returnController = "Fans";
                    }
                    // End of model creation for user types ###

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", returnController);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OnlineSportShop.Areas.User.Services;

using Proj.Models;

namespace Proj.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Proj.Models.User> _signInManager;
        private readonly UserManager<Proj.Models.User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;
        private readonly ICartList cardList_Service;

        public RegisterModel(
            UserManager<Proj.Models.User> userManager,
            SignInManager<Proj.Models.User> signInManager,
            ILogger<RegisterModel> logger,

            IEmailSender emailSender, ICartList cardList_Service, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.cardList_Service = cardList_Service;
            this._roleManager = roleManager;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage = "please enter a name !!!")]
            [MaxLength(30, ErrorMessage = "too long name !!!!")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Range(10, 100)]
            public int Age { get; set; }

            [DataType(DataType.PhoneNumber)]
            public string PhoneNum { get; set; }
            public Gender Gender { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string street { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {                
                var user = new Proj.Models.User { UserName = Input.UserName, Email = Input.Email, Age = Input.Age, PhoneNumber = Input.PhoneNum, Gender = Input.Gender, Address = new Address(Input.country, Input.city, Input.street) };
   
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var cartList = new ShoppingCart { User_ID = user.Id };
                    cardList_Service.CreatCartList(cartList);
                    _logger.LogInformation("User created a new account with password.");

                    //////Admin Authorized


                    //var roleExist = await _roleManager.RoleExistsAsync("WAdmin");
                    //if (!roleExist)
                    //{
                    //    await _roleManager.CreateAsync(new IdentityRole("WAdmin"));
                    //}
                    //_userManager.AddToRoleAsync(user, "WAdmin").Wait();
                    //bool userInRole = await _userManager.IsInRoleAsync(user, "WAdmin");
                    //if (!userInRole)
                    //{
                    //    _userManager.AddToRoleAsync(user, "WAdmin").Wait();
                    //}


                    var roleExist = await _roleManager.RoleExistsAsync("WUser");
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("WUser"));
                    }

                    bool userInRole = await _userManager.IsInRoleAsync(user, "WUser");
                    if (!userInRole)
                    {
                        _userManager.AddToRoleAsync(user, "WUser").Wait();
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                   
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

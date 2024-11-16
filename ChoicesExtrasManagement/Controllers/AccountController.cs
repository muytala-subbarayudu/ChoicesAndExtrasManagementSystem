using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using ChoicesExtrasManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ChoicesExtrasManagement.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        public readonly ChoicesExtrasManagementDbContext _choicesExtrasManagementDbContext;
        public readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountController
        (
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ChoicesExtrasManagementDbContext choicesExtrasManagementDbContext,
        IEmailService emailService,
        IConfiguration configuration
        ) 
        { 
            _userManager = userManager;
            _signInManager = signInManager;
            _choicesExtrasManagementDbContext = choicesExtrasManagementDbContext;
            _emailService = emailService;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel(); // Hold values in refresh
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.UserEmail);

            if (user != null) 
            {
                //User is found, check password and sign-in
                var passwordCheck = await _userManager.CheckPasswordAsync(user,loginVM.Password);

                //Password correct
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,false, false);
                    if (result.Succeeded)
                    {
                        TempData["Login"] = "User";
                        return RedirectToAction("Index", "Home");
                    }
                }
                //Password In-correct
                TempData["Error"] = "Sorry, Wrong credentials!";
                return View(loginVM);
            }

            //User not found case
            TempData["Error"] = "Sorry, Wrong credentials!";
            return View(loginVM);
        }

        [Authorize(Roles = "home-developer")]
        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel(); // Hold values in refresh
            return View(response);
        }

        [Authorize(Roles = "home-developer")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.UserEmail);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new AppUser()
            {
                Email = registerVM.UserEmail,
                UserName = registerVM.Name,
                PhoneNumber = registerVM.PhoneNumber,
                RegisteredDate = DateTime.Now

            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                var body = "Modern Homes has registered you as a Buyer. Please login now with the email: " + registerVM.UserEmail + " and password: " + registerVM.Password;
                await _emailService.SendEmailAsync(registerVM.UserEmail, "Welcome Message", body);

                // SMS Trigger Code
                var accountSID = _configuration["Twilio-accountSID"];
                var authToken = _configuration["Twilio-authToken"];

                TwilioClient.Init(accountSID, authToken);

                var from = _configuration["Twilio-fromNumber"];
                var to = new PhoneNumber(newUser.PhoneNumber);

                var message = MessageResource.Create(to: to, from: from, 
                    body: "Hello, " + newUser.UserName + ". Registration Success at Modern Homes, Please check email for login credentials. ");
            }
            else
            {
                var errors = newUserResponse.Errors;
                var errorString = "";
                foreach (var item in errors)
                {
                    errorString += item.Description + " \n";
                }
                TempData["Error"] = errorString;
                return View(registerVM);
            }


            return RedirectToAction("ViewBuyers", "Admin");
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}

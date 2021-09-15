using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Models.Services;
using Auth.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    public class AccountController:Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        
        [HttpGet]
        public IActionResult Index()
        {
           
            return View();
        }

        
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var login = new LoginViewModel(); // empty model
            return View(login);
        }
        


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.Login(model);
                if (!result.Success)
                {
                    ModelState.AddModelError("Password", "Password is not correct");
                    return View(model);
                }

                var role = result.Result.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin") return RedirectToAction("index", "admin");
                
                return RedirectToAction("index", "Student");


            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOut();
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            var register = new UserRegisterViewModel();
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.Register(model);
            if (result.Success)
            {
                return RedirectToAction("Index","Home");
            }
            
            ModelState.AddModelError("Password", result.Message);
            return View(model);
        }

    }
}
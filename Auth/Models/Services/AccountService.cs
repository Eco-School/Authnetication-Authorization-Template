using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Models.Data;
using Auth.Models.Data.Entities;
using Auth.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Auth.Models.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(DataContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Response<User>> Register(UserRegisterViewModel model)
        {
            var userExists = _context.Users.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
            if (userExists != null)
                return new Response<User>(false, "Current user is used");

            var user = new User()
            {
                CreatedAt = DateTime.Now,
                UserName = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return new Response<User>(false, "Please try later");
            //sign in
            //   await Login(new LoginViewModel() {PhoneNumber = model.PhoneNumber, Password = model.Password});
            return new Response<User>(true, "");
        }

        public async Task<Response<List<Claim>>> Login(LoginViewModel model)
        {
            //check if user exists
            var user = _context.Users.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
            if (user == null)
                return new Response<List<Claim>>(false, "Password or Phone number is not correct");

            //check password
            var isPasswordCorrect = await CheckPassword(user, model.Password);
            if (!isPasswordCorrect)
                return new Response<List<Claim>>(false, "Password is not correct");

            var userClaims = await AddClaimsAndSignIn(user);
            return new Response<List<Claim>>(true, "", userClaims);
        }

        public async Task SignOut()
        {
         await  _signInManager.SignOutAsync();
        }

        private async Task<List<Claim>> AddClaimsAndSignIn(User user)
        {
            var userClaims = await FillUserClaims(user);// fill user claims
            await _userManager.AddClaimsAsync(user, userClaims);  
            await _signInManager.SignInAsync(user, true); // sign in 
            return userClaims;
        }

        private async Task<List<Claim>> FillUserClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>()
            {
                new("Username", user.UserName),
                new("FullName", user.FullName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
                new("Id", user.Id),
                new Claim(ClaimTypes.Role,"Admin")
            };

            //add roles
            if (roles.Any())
                userClaims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            return userClaims;
        }

        private async Task<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
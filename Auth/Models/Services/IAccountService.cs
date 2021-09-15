using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Models.Data.Entities;
using Auth.Models.ViewModels;

namespace Auth.Models.Services
{
    public interface IAccountService
    {
        Task<Response<User>> Register(UserRegisterViewModel model);
        Task<Response<List<Claim>>> Login(LoginViewModel model);
        Task SignOut();
    }
}
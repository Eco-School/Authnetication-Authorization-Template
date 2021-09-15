using System;
using Microsoft.AspNetCore.Identity;

namespace Auth.Models.Data.Entities
{
    public class User:IdentityUser
    {
        public DateTime CreatedAt  { get; set; }
        public string  FullName { get; set; }
    }
}
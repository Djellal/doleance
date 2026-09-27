using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Doleance.Models;

namespace Doleance
{
    public partial class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(string userName, string password,string nom, string prenom, string qualite, string affiliation)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Redirect("~/Login?error=Invalid user or password");
            }

            var user = new ApplicationUser { UserName = userName, Email = userName,Nom=nom,Prenom=prenom,Qualite=qualite,Affiliation=affiliation};

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("~/");
            }

            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return Redirect($"~/Login?error={message}");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(string nom, string prenom, string qualite, string affiliation)
        {
          

            var id = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await userManager.FindByIdAsync(id);

            user.Nom = nom; 
            user.Prenom = prenom;
            user.Qualite = qualite;
            user.Affiliation = affiliation;

           
            var result2 = await userManager.UpdateAsync(user);
            if (result2.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: true);

                return Redirect("~/");
            }

            var message = string.Join(", ", result2.Errors.Select(error => error.Description));

            return Redirect($"~/Profile?error={message}");
        }
    }
}

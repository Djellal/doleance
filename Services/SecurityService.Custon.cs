using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Doleance.Models;
using Doleance.Data;

namespace Doleance
{
    public partial class SecurityService
    {
        public async Task<IEnumerable<ApplicationUser>> GetUsersInRole(string role)
        {
          
            return await userManager.GetUsersInRoleAsync(role);
            
        }
    }
}

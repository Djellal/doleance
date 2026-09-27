using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;

namespace Doleance.Pages
{
    public partial class ApplicationUsersComponent
    {
        
        protected async System.Threading.Tasks.Task GetUsersInRole(string roleId)
        {
            users =    Security.GetUsersInRole(roleId).Result;
            await grid0.Reload();
        }
    }
}

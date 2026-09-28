using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Doleance.Models;

namespace Doleance.Layouts
{
    public partial class MainLayoutComponent
    {
        /// <summary>Best available human-readable name for the signed-in user.</summary>
        /// <remarks>
        /// SecurityService.User never returns null (it falls back to an "Anonymous"
        /// ApplicationUser), but Nom/Prenom are only populated for records created
        /// through the app, so UserName stays as the fallback.
        /// </remarks>
        protected string DisplayName
        {
            get
            {
                var user = Security?.User;
                if (user == null)
                {
                    return string.Empty;
                }

                var full = $"{user.Prenom} {user.Nom}".Trim();
                if (!string.IsNullOrWhiteSpace(full) && full != " ")
                {
                    return full;
                }

                return user.UserName ?? string.Empty;
            }
        }

        /// <summary>French label for the user's role, or empty when unknown.</summary>
        protected string RoleLabel
        {
            get
            {
                // RoleNames is only set once the user has actually been loaded from
                // the database; it stays null on the Development backdoor path.
                var roles = Security?.User?.RoleNames;
                if (roles == null)
                {
                    return string.Empty;
                }

                var role = roles.FirstOrDefault();

                if (role == Constants.admin)
                {
                    return "Administrateur";
                }

                if (role == Constants.structadmin)
                {
                    return "Administrateur de structure";
                }

                if (role == Constants.user)
                {
                    return "Utilisateur";
                }

                return role;
            }
        }
    }
}

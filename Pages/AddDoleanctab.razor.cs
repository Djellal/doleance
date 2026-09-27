using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Doleance.Models.AppDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Doleance.Models;
using Doleance.Services;
using MimeKit;
using MimeKit.Text;

namespace Doleance.Pages
{
    public partial class AddDoleanctabComponent
    {
        [Inject]
        protected MailService mailSenser { get; set; }


        protected async System.Threading.Tasks.Task SendDoleanceMail()
        {

            var lstadmins = await Security.GetUsersInRole(Constants.admin);

            foreach (var item in lstadmins)
            {
                try
                {
                    string text = "<div style =\"background-color: rgb(4, 38, 82); color: aliceblue;text-align: center;\">"
                                      +"<h1> Université Ferhat Abbas Sétif - 1 </h1>"
                                      + "<h1> Espace des doléances</h1>"
                                  +"</div>"
                                  +"<div>"
                                      + "<h2>Nom et Prénom: " + Security.User.Nom + " " + Security.User.Prenom+"</h2>"
                                      + "<h2>Qualité: " + Security.User.Qualite + "</h2>"
                                      + "<h2>Affiliation: " + Security.User.Affiliation + "</h2>"
                                  + "</div>"
                                  + "<div>"
                                        + doleanctab.paragraph + "<br>"
                                 + "</div>"
                                 + "<a style = \"text-align: center;\" href=\"" + "https://doleances.univ-setif.dz/edit-doleanctab/" + doleanctab.Id + "\"><h2>Voir la doléance</h2></a>";
                  await mailSenser.SendEmail(item.Email, null, "Nouvelle doléance : " + doleanctab.objet, text);

                }
                catch (Exception ex)
                {

                    //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erreur mail", Detail = ex.Message, Duration = 10000 });
                };
            }



        }
    }
}

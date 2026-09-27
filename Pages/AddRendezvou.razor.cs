using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using Doleance.Services;
namespace Doleance.Pages
{
    public partial class AddRendezvouComponent
    {
        [Inject]
        protected MailService mailSenser { get; set; }


        protected async System.Threading.Tasks.Task SendRDVMail()
        {

            var lstadmins = await Security.GetUsersInRole(Constants.admin);

            foreach (var item in lstadmins)
            {
                try
                {
                    string text = "<div style =\"background-color: rgb(4, 38, 82); color: aliceblue;text-align: center;\">"
                                      + "<h1> Université Ferhat Abbas Sétif - 1 </h1>"
                                      + "<h1> Demande de rendez-vous</h1>"
                                  + "</div>"
                                  + "<div>"
                                      + "<h2>Nom et Prénom: " + Security.User.Nom + " " + Security.User.Prenom + "</h2>"
                                      + "<h2>Qualité: " + Security.User.Qualite + "</h2>"
                                      + "<h2>Affiliation: " + Security.User.Affiliation + "</h2>"
                                  + "</div>"
                                  + "<div>"
                                        + rendezvou.objet + "<br>"
                                 + "</div>"
                                 + "<a style = \"text-align: center;\" href=\"" + "https://doleances.univ-setif.dz/edit-rendezvou/" + rendezvou.Id + "\"><h2>Voir la demande</h2></a>";
                    await mailSenser.SendEmail(item.Email, null, "Nouvelle demande de rendez-vous : " + rendezvou.objet, text);

                }
                catch (Exception ex)
                {

                    //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erreur mail", Detail = ex.Message, Duration = 10000 });
                };
            }



        }

    }
}

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
    public partial class EditDoleanctabComponent
    {
        [Inject]
        protected MailService mailSenser { get; set; }


        protected async System.Threading.Tasks.Task SendDoleanceMail()
        {

            try
            {
                string text = "<div style =\"background-color: rgb(4, 38, 82); color: aliceblue;text-align: center;\">"
                                       + "<h1> Université Ferhat Abbas Sétif - 1 </h1>"
                                       + "<h1> Espace des doléances</h1>"
                                   + "</div>"
                                   + "<div>"
                                       + "<h2>Nom et Prénom: " + Security.User.Nom + " " + Security.User.Prenom + "</h2>"
                                       + "<h2>Qualité: " + Security.User.Qualite + "</h2>"
                                       + "<h2>Affiliation: " + Security.User.Affiliation + "</h2>"
                                   + "</div>"
                                   + "<div>"
                                         + doleanctab.paragraph + "<br>"
                                  + "</div>"
                                  + "<a style = \"text-align: center;\" href=\"" + "https://doleances.univ-setif.dz/edit-doleanctab/" + doleanctab.Id + "\"><h2>Voir la doléance</h2></a>";
                await mailSenser.SendEmail(doleanctab.email, null, "Réponse de doléance : " + doleanctab.objet, text);

            }
            catch (Exception ex)
            {

                //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erreur mail", Detail = ex.Message, Duration = 10000 });
            };



        }
        protected async System.Threading.Tasks.Task TransfertDoleance()
        {

               await AppDb.UpdateDoleanctab(doleanctab.Id, doleanctab);

                string text = "<div style =\"background-color: rgb(4, 38, 82); color: aliceblue;text-align: center;\">"
                                  + "<h1> Université Ferhat Abbas Sétif - 1 </h1>"
                                  + "<h2> Espace des doléances</h2>"
                              + "</div>"
                                + "<h2>Nom et Prénom: " + doleanctab.nomPrenom + "</h2>"

                              + "<h1 style =\"background-color: rgb(4, 38, 82); color: aliceblue;text-align: center;\"> Réponse</h1>"
                             + doleanctab.paragraph + "<br>"
                             + "<a style = \"text-align: center;\" href=\"" + "https://doleances.univ-setif.dz/edit-doleanctab/" + doleanctab.Id + "\"><h2>Voir la doléance</h2></a>";

                var stradmins = await Security.GetUsersInRole(Constants.structadmin);
                stradmins = stradmins.Where(a=>a.StructId==doleanctab.structId).ToList();

                foreach (var item in stradmins)
                {
                    await mailSenser.SendEmail(item.Email, null, "Doléance Transférée : " + doleanctab.objet, text);
                }

                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Info, Summary = $"Doléance Transférée", Detail = "Doléance Transférée avec succées", Duration = 10000 });


           
        }
    }
}

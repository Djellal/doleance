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
    public partial class EditRendezvouComponent
    {
        [Inject]
        protected MailService mailSenser { get; set; }

        
        protected async System.Threading.Tasks.Task SendRDVMail()
        {

            try
            {
                string text = "<div style =\"background-color: rgb(4, 38, 82); color: aliceblue;text-align: center;\">"
                                  + "<h1> Université Ferhat Abbas Sétif - 1 </h1>"
                                  + "<h2> Espace des doléances</h2>"
                              + "</div>"
                                + "<h2>Nom et Prénom: " + rendezvou.nomPrenom + "</h2>"

                              + "<h1 style =\"background-color: rgb(4, 38, 82); color: aliceblue;text-align: center;\"> Réponse</h1>"
                             + (rendezvou.acceptee ? "Accépté" : "Réfusé") + "<br>"
                             + (rendezvou.acceptee ? "Date : "+ rendezvou.date.ToString() : "") + "<br>"
                             + "<a style = \"text-align: center;\" href=\"" + "https://doleances.univ-setif.dz/edit-rendezvou/" + rendezvou.Id + "\"><h2>Voir la demande du RDV</h2></a>";


                await mailSenser.SendEmail(rendezvou.email, null, "Réponse de demande de rendez-vous :  : " + rendezvou.objet, text);

            }
            catch (Exception ex)
            {

                //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erreur mail", Detail = ex.Message, Duration = 10000 });
            };



        }
    }
}

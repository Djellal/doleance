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

namespace Doleance.Pages
{
    public partial class EditRendezvouComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected AppDbService AppDb { get; set; }

        [Parameter]
        public dynamic Id { get; set; }

        Doleance.Models.AppDb.Rendezvou _rendezvou;
        protected Doleance.Models.AppDb.Rendezvou rendezvou
        {
            get
            {
                return _rendezvou;
            }
            set
            {
                if (!object.Equals(_rendezvou, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "rendezvou", NewValue = value, OldValue = _rendezvou };
                    _rendezvou = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Doleance.Models.AppDb.Structure> _getStructuresResult;
        protected IEnumerable<Doleance.Models.AppDb.Structure> getStructuresResult
        {
            get
            {
                return _getStructuresResult;
            }
            set
            {
                if (!object.Equals(_getStructuresResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getStructuresResult", NewValue = value, OldValue = _getStructuresResult };
                    _getStructuresResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var appDbGetRendezvouByIdResult = await AppDb.GetRendezvouById(Id);
            rendezvou = appDbGetRendezvouByIdResult;

            var appDbGetStructuresResult = await AppDb.GetStructures();
            getStructuresResult = appDbGetStructuresResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Doleance.Models.AppDb.Rendezvou args)
        {
            try
            {
                var appDbUpdateRendezvouResult = await AppDb.UpdateRendezvou(Id, rendezvou);
                DialogService.Close(rendezvou);
            }
            catch (System.Exception appDbUpdateRendezvouException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Rendezvou" });
            }

            if (rendezvou.acceptee)
            {
                await SendRDVMail();
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}

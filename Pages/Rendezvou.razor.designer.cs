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
    public partial class RendezvouComponent : ComponentBase
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
        protected RadzenDataGrid<Doleance.Models.AppDb.Rendezvou> grid0;

        IEnumerable<Doleance.Models.AppDb.Rendezvou> _getRendezvousResult;
        protected IEnumerable<Doleance.Models.AppDb.Rendezvou> getRendezvousResult
        {
            get
            {
                return _getRendezvousResult;
            }
            set
            {
                if (!object.Equals(_getRendezvousResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getRendezvousResult", NewValue = value, OldValue = _getRendezvousResult };
                    _getRendezvousResult = value;
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
            var appDbGetRendezvousResult = await AppDb.GetRendezvous(new Query() { Expand = "Structure" });
            getRendezvousResult = appDbGetRendezvousResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddRendezvou>("Add Rendezvou", null, new DialogOptions(){ Width = $"{1000}px",CloseDialogOnOverlayClick = true,Resizable = true,Draggable = true });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Doleance.Models.AppDb.Rendezvou args)
        {
            var dialogResult = await DialogService.OpenAsync<EditRendezvou>("Edit Rendezvou", new Dictionary<string, object>() { {"Id", args.Id} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var appDbDeleteRendezvouResult = await AppDb.DeleteRendezvou(data.Id);
                    if (appDbDeleteRendezvouResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception appDbDeleteRendezvouException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Rendezvou" });
            }
        }
    }
}

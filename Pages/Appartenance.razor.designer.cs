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
    public partial class AppartenanceComponent : ComponentBase
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
        protected RadzenDataGrid<Doleance.Models.AppDb.Appartenance> grid0;

        Doleance.Models.AppDb.Appartenance _appartenance;
        protected Doleance.Models.AppDb.Appartenance appartenance
        {
            get
            {
                return _appartenance;
            }
            set
            {
                if (!object.Equals(_appartenance, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "appartenance", NewValue = value, OldValue = _appartenance };
                    _appartenance = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Doleance.Models.AppDb.Appartenance> _getAppartenancesResult;
        protected IEnumerable<Doleance.Models.AppDb.Appartenance> getAppartenancesResult
        {
            get
            {
                return _getAppartenancesResult;
            }
            set
            {
                if (!object.Equals(_getAppartenancesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAppartenancesResult", NewValue = value, OldValue = _getAppartenancesResult };
                    _getAppartenancesResult = value;
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
            appartenance = new Doleance.Models.AppDb.Appartenance(){};

            var appDbGetAppartenancesResult = await AppDb.GetAppartenances();
            getAppartenancesResult = appDbGetAppartenancesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            await this.grid0.InsertRow(new Doleance.Models.AppDb.Appartenance());
        }

        protected async System.Threading.Tasks.Task Grid0RowCreate(dynamic args)
        {
            var appDbCreateAppartenanceResult = await AppDb.CreateAppartenance(args);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowUpdate(dynamic args)
        {
            var appDbUpdateAppartenanceResult = await AppDb.UpdateAppartenance(args.Id, args);
        }

        protected async System.Threading.Tasks.Task EditButtonClick(MouseEventArgs args, dynamic data)
        {
            this.grid0.EditRow(data);
        }

        protected async System.Threading.Tasks.Task SaveButtonClick(MouseEventArgs args, dynamic data)
        {
            this.grid0.UpdateRow(data);
        }

        protected async System.Threading.Tasks.Task CancelButtonClick(MouseEventArgs args, dynamic data)
        {
            this.grid0.CancelEditRow(data);

            var appDbCancelAppartenanceChangesResult = await AppDb.CancelAppartenanceChanges(data);
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var appDbDeleteAppartenanceResult = await AppDb.DeleteAppartenance(data.Id);
                    if (appDbDeleteAppartenanceResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception appDbDeleteAppartenanceException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Appartenance" });
            }
        }
    }
}

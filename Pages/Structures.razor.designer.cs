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
    public partial class StructuresComponent : ComponentBase
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
        protected RadzenDataGrid<Doleance.Models.AppDb.Structure> grid0;

        Doleance.Models.AppDb.Structure _structure;
        protected Doleance.Models.AppDb.Structure structure
        {
            get
            {
                return _structure;
            }
            set
            {
                if (!object.Equals(_structure, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "structure", NewValue = value, OldValue = _structure };
                    _structure = value;
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
            structure = new Doleance.Models.AppDb.Structure(){};

            var appDbGetStructuresResult = await AppDb.GetStructures();
            getStructuresResult = appDbGetStructuresResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            await this.grid0.InsertRow(new Doleance.Models.AppDb.Structure());
        }

        protected async System.Threading.Tasks.Task Grid0RowCreate(dynamic args)
        {
            var appDbCreateStructureResult = await AppDb.CreateStructure(args);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowUpdate(dynamic args)
        {
            var appDbUpdateStructureResult = await AppDb.UpdateStructure(args.Id, args);
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

            var appDbCancelStructureChangesResult = await AppDb.CancelStructureChanges(data);
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var appDbDeleteStructureResult = await AppDb.DeleteStructure(data.Id);
                    if (appDbDeleteStructureResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception appDbDeleteStructureException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Structure" });
            }
        }
    }
}

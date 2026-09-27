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
    public partial class DoleanctabComponent : ComponentBase
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
        protected RadzenDataGrid<Doleance.Models.AppDb.Doleanctab> grid0;

        Doleance.Models.ApplicationUser _CurrentUser;
        protected Doleance.Models.ApplicationUser CurrentUser
        {
            get
            {
                return _CurrentUser;
            }
            set
            {
                if (!object.Equals(_CurrentUser, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "CurrentUser", NewValue = value, OldValue = _CurrentUser };
                    _CurrentUser = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Doleance.Models.AppDb.Doleanctab> _getDoleanctabsResult;
        protected IEnumerable<Doleance.Models.AppDb.Doleanctab> getDoleanctabsResult
        {
            get
            {
                return _getDoleanctabsResult;
            }
            set
            {
                if (!object.Equals(_getDoleanctabsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getDoleanctabsResult", NewValue = value, OldValue = _getDoleanctabsResult };
                    _getDoleanctabsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int? _SelectedStucture;
        protected int? SelectedStucture
        {
            get
            {
                return _SelectedStucture;
            }
            set
            {
                if (!object.Equals(_SelectedStucture, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "SelectedStucture", NewValue = value, OldValue = _SelectedStucture };
                    _SelectedStucture = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int _filter;
        protected int filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (!object.Equals(_filter, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "filter", NewValue = value, OldValue = _filter };
                    _filter = value;
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
            CurrentUser = Security.User;

            getDoleanctabsResult = null;

            SelectedStucture = null;

            if (Security.IsInRole(Constants.admin))
            {
                getDoleanctabsResult = (await AppDb.GetDoleanctabs()).OrderByDescending(d => d.date);
                
            }
            else 
            {
                getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(u => u.userid ==  CurrentUser.Id).OrderByDescending(d => d.date);
               
                
            };

            filter = 2;

            var appDbGetStructuresResult = await AppDb.GetStructures();
            getStructuresResult = appDbGetStructuresResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("add-doleanctab");
        }

        protected async System.Threading.Tasks.Task Selectbar0Change(dynamic args)
        {
            filter = args;

            await FilterDoleance();
        }

        protected async System.Threading.Tasks.Task StucturesdropdownChange(dynamic args)
        {
            await FilterDoleance();
        }

        protected async System.Threading.Tasks.Task ExporterbuttonClick(MouseEventArgs args)
        {
            await AppDb.ExportDoleanctabsToExcel(new Query() { OrderBy = $"d => d.num", Select = "num,date,nomPrenom,objet,repondu" }, $"doleances");
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Doleance.Models.AppDb.Doleanctab args)
        {
            UriHelper.NavigateTo($"edit-doleanctab/{args.Id}");
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Voulez vous vraiment supprimer cette doléance?") == true)
                {
                    var appDbDeleteDoleanctabResult = await AppDb.DeleteDoleanctab(data.Id);
                    if (appDbDeleteDoleanctabResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception appDbDeleteDoleanctabException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Doleanctab" });
            }
        }
    }
}

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
    public partial class MesdoleancesComponent : ComponentBase
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
        protected RadzenDataList<Doleance.Models.AppDb.Doleanctab> datalist0;

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

            getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(u => u.userid ==  CurrentUser.Id).OrderByDescending(d => d.date);
        }

        protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("add-doleanctab");
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args, dynamic data)
        {
            await DialogService.OpenAsync<EditDoleanctab>($"Voir la doléance", new Dictionary<string, object>() { {"Id", data.Id} }, new DialogOptions(){ Width = $"{1000}px",Resizable = true,Draggable = true });
        }
    }
}

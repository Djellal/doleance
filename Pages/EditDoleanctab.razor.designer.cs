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
    public partial class EditDoleanctabComponent : ComponentBase
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

        Doleance.Models.AppDb.Doleanctab _doleanctab;
        protected Doleance.Models.AppDb.Doleanctab doleanctab
        {
            get
            {
                return _doleanctab;
            }
            set
            {
                if (!object.Equals(_doleanctab, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "doleanctab", NewValue = value, OldValue = _doleanctab };
                    _doleanctab = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        ApplicationUser _usr;
        protected ApplicationUser usr
        {
            get
            {
                return _usr;
            }
            set
            {
                if (!object.Equals(_usr, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "usr", NewValue = value, OldValue = _usr };
                    _usr = value;
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

        IEnumerable<int?> _selectedStructures;
        protected IEnumerable<int?> selectedStructures
        {
            get
            {
                return _selectedStructures;
            }
            set
            {
                if (!object.Equals(_selectedStructures, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "selectedStructures", NewValue = value, OldValue = _selectedStructures };
                    _selectedStructures = value;
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
            var appDbGetDoleanctabByIdResult = await AppDb.GetDoleanctabById(Convert.ChangeType(Id, Type.GetTypeCode(typeof(int))));
            doleanctab = appDbGetDoleanctabByIdResult;

            var securityGetUserByIdResult = await Security.GetUserById($"{doleanctab.userid}");
            usr = securityGetUserByIdResult;

            var appDbGetStructuresResult = await AppDb.GetStructures();
            getStructuresResult = appDbGetStructuresResult;

            selectedStructures = new Nullable<int>[]{0};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Doleance.Models.AppDb.Doleanctab args)
        {
            doleanctab.repondu = !string.IsNullOrWhiteSpace(doleanctab.reponse);

            try
            {
                var appDbUpdateDoleanctabResult = await AppDb.UpdateDoleanctab(Convert.ChangeType(Id, Type.GetTypeCode(typeof(int))), doleanctab);
                DialogService.Close();
                await JSRuntime.InvokeAsync<string>("window.history.back");
            }
            catch (System.Exception appDbUpdateDoleanctabException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Erreur" });
            }

            if (doleanctab.repondu)
            {
                await SendDoleanceMail();
            }
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            try
            {
                await TransfertDoleance();
            }
            catch (System.Exception transfertDoleanceException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Erreur",Detail = $"{transfertDoleanceException.Message}" });
            }
        }

        protected async System.Threading.Tasks.Task FermerClick(MouseEventArgs args)
        {
            DialogService.Close();
            await JSRuntime.InvokeAsync<string>("window.history.back");
        }
    }
}

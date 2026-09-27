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
    public partial class AccueilComponent : ComponentBase
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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
        }

        protected async System.Threading.Tasks.Task Login0Register()
        {
            await DialogService.OpenAsync<RegisterApplicationUser>($"S'inscrire", null, new DialogOptions(){ Top = $"{0}px",Width = $"{900}px" });
        }

        protected async System.Threading.Tasks.Task Login0ResetPassword(dynamic args)
        {
            UriHelper.NavigateTo($"Account/ResetPassword?userName={args}", true);
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("add-doleanctab");
        }

        protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("add-rendezvou");
        }
    }
}

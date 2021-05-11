using ApiGateway.Core.AuthenticationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared.RouteView
{
    public class AppRouteView : Microsoft.AspNetCore.Components.RouteView
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IAuthenticationService AuthenticationService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var attribute = (AuthorizeAttribute)Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute));
            var authorize = attribute != null;

            List<string> allRoles = new();
            if (attribute?.Roles != null)
            {
                allRoles.AddRange(attribute.Roles.Split(',').ToList());
            }

            if (AuthenticationService.User != null && authorize)
            {
                if (AuthenticationService.User.Roles.Length <= 0)
                    NavigationManager.NavigateTo($"");
            }


            if (authorize && AuthenticationService.User != null && attribute.Roles != null)
            {
                if (AuthenticationService.User.Roles.ToList().Any(x => allRoles.Contains(x)))
                {
                    base.Render(builder);
                }
                else
                {
                    NavigationManager.NavigateTo($"/unauthorized");
                }
            }
            else
            {
                if (authorize && AuthenticationService.User == null)
                {
                    var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                    NavigationManager.NavigateTo($"/?returnUrl={returnUrl}");
                }
                else
                {
                    base.Render(builder);
                }
            }
        }
    }
}

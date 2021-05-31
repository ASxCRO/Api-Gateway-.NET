using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Pages
{
    public partial class Poruke
    {
        public string CurrentMessage { get; set; } = "Dobrodošao u aplikaciju!";
        public string CurrentUserId { get; set; } = "1";
        public string CurrentUserImageURL { get; set; } = "https://images.pexels.com/photos/1680172/pexels-photo-1680172.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=650&w=940";
        public bool IsConnected { get; set; } = true;

        public class MessageRequest
        {
            public string userName { get; set; }
            public string message { get; set; }
        }

        public MessageRequest model = new MessageRequest();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }

        public void SubmitAsync()
        {
            if (!string.IsNullOrEmpty(CurrentMessage))
            {
                foreach (var message in new List<string> { "dobrodosao", "test", "test2" })
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        public void OnKeyPressInChat(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                SubmitAsync();
            }
        }

        protected override async Task OnInitializedAsync()
        {
        }

        public bool open;
        public Anchor ChatDrawer { get; set; }

        public void OpenDrawer(Anchor anchor)
        {
            ChatDrawer = anchor;
            open = true;
        }
    }
}

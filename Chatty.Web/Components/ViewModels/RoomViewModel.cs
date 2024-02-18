using Chatty.Interfaces.DTOs;
using Chatty.Web.Models;
using Microsoft.AspNetCore.Components;

namespace Chatty.Web.Components.ViewModels
{
    public class RoomViewModel : ComponentBase
    {
        // Fields

        private readonly IRoomService roomService = default!;

        // Constructors

        [Parameter] public Guid? RoomId {  get; set; }

        [Inject] public IRoomService RoomService { get; set; } = default!;

        public List<Message> Messages { get; set; } = [];

        public String? CurrentMessage { get; set; }

        // Methods

        public async Task SendMessage()
        {
            await RoomService?.BroadcastMessage(RoomId ?? Guid.Empty, CurrentMessage);
        }

        protected override async Task OnInitializedAsync()
        {            
            RoomService.OnMessage += RoomService_OnMessage;

            Messages =  await RoomService?.GetMessages(RoomId ?? Guid.Empty) ?? [];
            await RoomService.Subscribe(RoomId ?? Guid.Empty);
            await InvokeAsync(StateHasChanged);
        }

        private void RoomService_OnMessage(Message obj)
        {
            Messages.Add(obj);

            InvokeAsync(StateHasChanged).Wait();
        }
    }
}

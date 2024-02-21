using Chatty.Interfaces.DTOs;
using Chatty.Web.Models;
using Microsoft.AspNetCore.Components;

namespace Chatty.Web.Components.ViewModels
{
    public class RoomViewModel : ComponentBase
    {
        // Fields

        private readonly IMessageService roomService = default!;

        // Constructors

        [Parameter] public Guid? RoomId {  get; set; }

        [Inject] public IMessageService MessageService { get; set; } = default!;

        [Inject] public IRoomService RoomService { get; set; } = default!;

        public List<Message> Messages { get; set; } = [];

        public IEnumerable<Guid> RoomIds { get; set; } = [];

        public String? CurrentMessage { get; set; }

        // Methods

        public async Task SendMessage()
        {
            await MessageService?.BroadcastMessage(RoomId ?? Guid.Empty, CurrentMessage);
        }

        protected override async Task OnInitializedAsync()
        {            
            MessageService.OnMessage += RoomService_OnMessage;

            Messages =  await MessageService?.GetMessages(RoomId ?? Guid.Empty) ?? [];
            RoomIds = await RoomService.GetRoomIds();
            await RoomService.Subscribe();
            RoomService.OnRoomsChanged += async () =>
            {
                RoomIds = await RoomService.GetRoomIds();
                await InvokeAsync(StateHasChanged);
            };
            await MessageService.Subscribe(RoomId ?? Guid.Empty);
            await InvokeAsync(StateHasChanged);
        }
              

        private void RoomService_OnMessage(Message obj)
        {
            Messages.Add(obj);

            InvokeAsync(StateHasChanged).Wait();
        }
    }
}

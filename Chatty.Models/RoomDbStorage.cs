using Newtonsoft.Json;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public class RoomDbStorage : IGrainStorage
    {
        // Fields

        private readonly ChattyContext context;

        // Constructors

        public RoomDbStorage(ChattyContext context) => this.context = context;

        // Methods

        public async Task ClearStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            if (typeof(T) == typeof(RoomState))
            {
                var loadedState = context.RoomStates.SingleOrDefault(x => x.GrainId == grainId.GetGuidKey());
                if (loadedState != null)
                {
                    context.RoomStates.Remove(loadedState);
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task ReadStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            if (typeof(T) == typeof(RoomState))
            {
                var loadedState = context.RoomStates.SingleOrDefault(x => x.GrainId == grainId.GetGuidKey());
                var state = loadedState switch
                {
                    null => Create<T>(),
                    { State: "" } => Create<T>(),
                    _ => JsonConvert.DeserializeObject<T>(loadedState.State)
                };
                grainState.State = state;
            }
        }

        public async Task WriteStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            if (typeof(T) == typeof(RoomState))
            {
                var existingGrainState = context.RoomStates.SingleOrDefault(x => x.GrainId == grainId.GetGuidKey());
                var stateAsJson = JsonConvert.SerializeObject(grainState.State);

                var toSave = existingGrainState switch
                {
                    null => Create(grainId.GetGuidKey(), context),
                    _ => existingGrainState
                };
                toSave.State = stateAsJson;
                await context.SaveChangesAsync();
            }
        }

        private T Create<T>() => (T)Activator.CreateInstance(typeof(T));

        private PersistedRoomState Create(Guid grainId, ChattyContext context)
        {
            var result = new PersistedRoomState { GrainId = grainId };
            context.RoomStates.Add(result);
            return result;
        }
    }
}

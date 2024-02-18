using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Orleans.Runtime;
using Orleans.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatty.Models
{
    public sealed class RoomStateStorage : IGrainStorage
    {
        // Fields

        private readonly DirectoryInfo saveLocation;

        // Constructors

        public RoomStateStorage(DirectoryInfo saveLocation)
        {
            this.saveLocation = saveLocation;
        }

        // Methods

        public Task ClearStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            throw new NotImplementedException();
        }

        public async Task ReadStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            if(typeof(T) == typeof(RoomState))
            {
                var fileName = Path.Combine(saveLocation.FullName, $"{grainId.Key.ToString()}.json");
                if (File.Exists(fileName))
                {
                    var contents = await File.ReadAllTextAsync(fileName);
                    grainState.State = JsonConvert.DeserializeObject<T>(contents);
                }
            }
        }

        public async Task WriteStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
        {
            if (typeof(T) == typeof(RoomState))
            {
                var fileName = Path.Combine(saveLocation.FullName, $"{grainId.Key.ToString()}.json");
                if (File.Exists(fileName))
                {
                   File.Delete(fileName);
                }
                await File.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(grainState.State));
            }
        }
    }
}

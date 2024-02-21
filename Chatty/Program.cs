// See https://aka.ms/new-console-template for more information
using Chatty.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Orleans.Storage;

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, siloBuilder) =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMemoryGrainStorageAsDefault();
        siloBuilder.AddMemoryGrainStorage("messages");
        siloBuilder.Services.AddDbContext<ChattyContext>();

        siloBuilder.AddGrainService<RoomRegistrarService>();
        siloBuilder.Services.AddSingleton<IRoomRegistrarServiceClient, RoomRegistrarServiceClient>();

        //siloBuilder.Services.AddKeyedSingleton<IGrainStorage>("RoomStateStorage", (_, _) =>
        //{
        //    var directory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OrleansTest"));
        //    return new RoomStateStorage(directory);
        //});

        siloBuilder.Services.AddKeyedSingleton<IGrainStorage, RoomDbStorage>("RoomStateStorage");
    }).Build();

await host.StartAsync();

Console.ReadKey();

await host.StopAsync();
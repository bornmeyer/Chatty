using Chatty.Web.Components;
using Chatty.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Host.UseOrleansClient(builder =>
 {
     builder.UseLocalhostClustering();
 })
.ConfigureServices(services =>
{
    //services.AddHostedService<IMessagesService, >();
    services.AddTransient<IMessageService, MessageService>();
    services.AddTransient<IRoomService, RoomService>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

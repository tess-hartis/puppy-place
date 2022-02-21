using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PuppyPlace.Blazor;
using PuppyPlace.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => 
    new HttpClient {BaseAddress = new Uri("https://localhost:7079/api/")});

builder.Services.AddScoped<IPersonService, PersonService>();

await builder.Build().RunAsync();
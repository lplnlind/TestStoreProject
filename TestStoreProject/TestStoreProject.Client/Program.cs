using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TestStoreProject.Client.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddClient();


await builder.Build().RunAsync();

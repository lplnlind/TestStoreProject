using TestStoreProject.Client.Services.Interfaces;
using TestStoreProject.Client.Services;
using MudBlazor.Services;
using Blazored.LocalStorage;
using TestStoreProject.Client.Http;
using TestStoreProject.Client.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace TestStoreProject.Client.DependencyInjection
{
    public static class ClientDependencyInjection
    {
        public static IServiceCollection AddClient(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();

            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            services.AddScoped<AuthHeaderHandler>();

            services.AddHttpClient("AuthorizedClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7207/");
            }).AddHttpMessageHandler<AuthHeaderHandler>();
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));


            services.AddScoped<IProductClient, ProductClient>();
            services.AddScoped<ICategoryClient, CategoryClient>();
            services.AddScoped<ICartClient, CartClient>();
            services.AddScoped<IAuthClient, AuthClient>();
            services.AddScoped<IUserClient, UserClient>();
            services.AddScoped<IOrderClient, OrderClient>();

            services.AddMudServices();

            return services;
        }
    }
}

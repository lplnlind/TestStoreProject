using Application.Interfaces;
using Application.Services;
using Application.Setting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;


namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<UserService, UserService>();

            return services;
        }
    }
}

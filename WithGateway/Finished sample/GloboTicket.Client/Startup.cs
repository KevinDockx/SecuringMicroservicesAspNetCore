using GloboTicket.Web.Models;
using GloboTicket.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace GloboTicket.Web
{
    public class Startup
    {
        private readonly IHostEnvironment environment;
        private readonly IConfiguration config;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            config = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build();

            var builder = services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
            });

            if (environment.IsDevelopment())
                builder.AddRazorRuntimeCompilation();

            services.AddAccessTokenManagement();

            services.AddHttpClient<IEventCatalogService, EventCatalogService>(c => 
                c.BaseAddress = new Uri(config["ApiConfigs:EventCatalog:Uri"]))
                .AddUserAccessTokenHandler();
            services.AddHttpClient<IShoppingBasketService, ShoppingBasketService>(c => 
                c.BaseAddress = new Uri(config["ApiConfigs:ShoppingBasket:Uri"]))
                .AddUserAccessTokenHandler();
            services.AddHttpClient<IOrderService, OrderService>(c =>
                c.BaseAddress = new Uri(config["ApiConfigs:Order:Uri"]))
                .AddUserAccessTokenHandler();
        
            services.AddSingleton<Settings>();

            services.AddHttpContextAccessor();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = "https://localhost:5010/";
                options.ClientId = "globoticket";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.ClientSecret = "ce766e16-df99-411d-8d31-0f5bbc6b8eba";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("shoppingbasket.fullaccess");
             //   options.Scope.Add("eventcatalog.fullaccess");
                options.Scope.Add("globoticketgateway.fullaccess");
                options.Scope.Add("offline_access");
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=EventCatalog}/{action=Index}/{id?}");
            });
        }
    }
}

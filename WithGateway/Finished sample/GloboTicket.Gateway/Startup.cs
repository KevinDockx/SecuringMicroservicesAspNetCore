using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GloboTicket.Gateway.DelegatingHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace GloboTicket.Gateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAccessTokenManagement();

            // sub => http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var authenticationScheme = "GloboTicketGatewayAuthenticationScheme";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(authenticationScheme, options =>
                   {
                       options.Authority = "https://localhost:5010";
                       options.Audience = "globoticketgateway";
                   });

            services.AddHttpClient();

            services.AddScoped<TokenExchangeDelegatingHandler>();

            services.AddOcelot()
                .AddDelegatingHandler<TokenExchangeDelegatingHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            await app.UseOcelot();       
        }
    }
}

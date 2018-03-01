using D2.Common;
using D2.MasterData.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace D2.MasterData
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;

          /*
           *if (ServiceRegistration.registerSelf(logger) == false) {
                logger.LogCritical("failed to register self");
                throw new InvalidOperationException("failed to register self");
            }
          */
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbConnection>(provider => ConnectionFactory.CreateConnection());
            services.AddMvc();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddCors(
                    options => options.AddPolicy(
                                           "default",
                                           policy => policy.AllowAnyOrigin()
                                                           .AllowAnyHeader()
                                                           .AllowAnyMethod()
                                       )
                )
                .AddAuthentication("Bearer")
                .AddOAuth2Introspection(
                    options => {
                        options.Authority = ServiceConfiguration.authority.FullAddress;
                        options.EnableCaching = true;
                        options.ClientId = "api";
                        options.ClientSecret = "78C2A2A1-6167-45E4-A9D7-46C5D921F7D5";
                        options.SaveToken = true;
                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromMinutes(10.0);
                    }
                );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.EnvironmentName != "Development") {
                app.UseForwardedHeaders(
                    new ForwardedHeadersOptions {
                        ForwardedHeaders = ForwardedHeaders.All
                    }
                );
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseMvc()
                .UseCors("default")
                .UseAuthentication();
        }
    }
}

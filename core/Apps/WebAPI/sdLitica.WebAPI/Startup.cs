/* *************************************************************************
 * This file is part of project "Insight Project".
 *
 *  Insight Project is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Insight Project is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
 * *************************************************************************/

using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using sdLitica.Bootstrap.Events;
using sdLitica.Bootstrap.Extensions;
using sdLitica.Filters;
using sdLitica.WebAPI.Models.Security;

namespace sdLitica
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add appsettings file configuration to bootstrap
            Configuration.AddSettings();

            // Add any type of services available
            services.AddServices();

            // Add relational database support
            services.AddRelationalDatabase();

            // Add time series support
            services.AddTimeSeriesDatabase();

            // Add event and messages support
            services.AddEventsAndMessages();

            // Add authentication 
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CustomAuthOptions.DefaultSchema;
                    options.DefaultChallengeScheme = CustomAuthOptions.DefaultSchema;
                })

                // Call custom authentication extension method
                .AddCustomAuth(options =>
                {
                    // Configure single or multiple passwords for authentication
                    options.AuthKey = CustomAuthOptions.DefaultSchema;
                });

            // Adding CORS configuration
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://sdlitica.sdcloud.io", "http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddMvc(
                config =>
                {
                    config.EnableEndpointRouting = false;
                    config.Filters.Add(typeof(ErrorResponseFilter));
                    config.Filters.Add(typeof(ActionValidationFilter));
                    config.Filters.Add(
                        new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "sdLitica Project REST API", Version = "v1"});

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                OpenApiSecurityScheme basicSecurityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'cloudToken' following by space and a token received from /login endpoint",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference {Id = "cloudToken", Type = ReferenceType.SecurityScheme}
                };
                c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {{basicSecurityScheme, ImmutableList<string>.Empty}});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Information about middleware order
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(1)
            };
            
            // WHY???
            // https://github.com/dotnet/AspNetCore.Docs/pull/21695
            app.UseWebSockets(webSocketOptions);

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();

            app.UseMvc();
            app.UseSwagger();
            
            // app.UseMiddleware<WebSocketMiddleware>();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insight Project REST API V1"); });

            //sample subscribe for RabbitMQ
            app.SubscribeEvents();
        }
    }
}
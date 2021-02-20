using System.Reflection;
using ES.API.Requests.Infrastructure;
using Ezley.Events;
using Ezley.EventSourcing;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ezley.ProjectionStore;
using Ezley.SnapshotStore;

namespace ES.API.Requests
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            SetupAuthentication(services);
            SetupCors(services);
            SetupSwagger(services);
            SetupMediatR(services);
            SetupDI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development.");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation("Not Development.");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseRouting();
            
            // useCors must come after UseRouting
            app.UseCors("AppCorsPolicy");
            
            // 2. Enable authentication middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void SetupAuthentication(IServiceCollection services)
        {
            // https://manage.auth0.com/dashboard/us/ezley/apis/5e7a9602020c4e08b509ca56/quickstart
            // 1. Add Authentication Services / Auth0
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Authority"];
                options.Audience = Configuration["Auth0:Audience"];
            });
        }
        private void SetupCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AppCorsPolicy",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        private void SetupMediatR(IServiceCollection services)
        {
            // Setup MediatR;
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatorHandlers(typeof(Startup).GetTypeInfo().Assembly); // DI setup for MediatR
        }

        private void SetupSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen();
        }
        
        private void SetupDI(IServiceCollection services)
        {
            string endpointUri = Configuration["Azure:EndPointUri"]; 
            string database = Configuration["Azure:Database"]; 
            string authKey = Configuration[ "Azure:AuthKey"]; 
            string eventContainer = Configuration["Azure:EventContainer"];
            string viewContainer = Configuration["Azure:ViewContainer"];
            string snapshotContainer = Configuration["Azure:SnapshotContainer"];
            
            // Setup DI
            services.AddTransient<ISnapshotStore, CosmosSnapshotStore>(Tenant =>
                new CosmosSnapshotStore(endpointUri, authKey, database,snapshotContainer));
            
            services.AddTransient<IEventStore, CosmosDBEventStore>(Tenant =>
                new CosmosDBEventStore(
                    new EventTypeResolver(), endpointUri, authKey, database, eventContainer)
            );

            services.AddTransient<IViewRepository, CosmosDBViewRepository>(Tenant =>
                new CosmosDBViewRepository(endpointUri, authKey, database, viewContainer));
             
        }
    }
}
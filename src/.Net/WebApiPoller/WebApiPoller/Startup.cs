using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApiPoller.Data;
using WebApiPoller.Data.Interfaces;
using WebApiPoller.Extensions;
using WebApiPoller.Repositories;
using WebApiPoller.Repositories.Interfaces;
using WebApiPoller.Routing;
using WebApiPoller.Services;
using WebApiPoller.Services.ApiFetcher;
using WebApiPoller.Services.Clients;
using WebApiPoller.Services.Poller;

namespace WebApiPoller
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
            services
                .AddControllers()
                .AddJsonOptions(j =>
                {
                    j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    j.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
            services
                .AddHttpClient();


            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiPoller", Version = "v1" });
                    // c.DescribeAllEnumsAsStrings();
                });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("categoryEnum", typeof(CategoryConstraint));
            });

            //services.AddScoped<IPoller, GoldenApplePoller>();
            //services.AddScoped<IPoller, LetuPoller>();
            // services.AddHttpClient<GoldenAppleClient>();

            // check if this can be anyhow resolved
            services.AddHttpClient<IProductsSourceClient, GoldenAppleClient>();
            services.AddHttpClient<IProductsSourceClient, LetuClient>();

            services.AddScoped<IProductApiFetcher, ProductApiFetcher>();
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiPoller v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebApiPoller.Data;
using WebApiPoller.Data.Interfaces;
using WebApiPoller.Infrastructure.ConfigObjects;
using WebApiPoller.Infrastructure.Routing;
using WebApiPoller.Repositories;
using WebApiPoller.Repositories.Interfaces;
using WebApiPoller.Services.ApiFetcher;
using WebApiPoller.Services.Clients;
using WebApiPoller.Services.Messaging;

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
                .AddHttpClient()
                .Configure<RouteOptions>(options =>
                {
                    options.ConstraintMap.Add("categoryEnum", typeof(CategoryConstraint));
                });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiPoller", Version = "v1" });
                });

            var (databaseConfig, messageProducerConfig) = RegisterConfigs(services);

            services.AddSingleton<IMongoClient>(new MongoClient(databaseConfig.ConnectionString));
            services.AddHostedService<ConfigureMongoDbIndexesService>();

            var producerBuilder = new ProducerBuilder<Null, List<string>>(messageProducerConfig);
            services.AddSingleton(producerBuilder.Build());

            services.AddSingleton<IMessageProducer, MessageProducer>();
            services.AddHttpClient<IProductsSourceClient, GoldenAppleClient>();
            services.AddHttpClient<IProductsSourceClient, LetuClient>();
            services.AddHttpClient<IProductsSourceClient, PodrygkaClient>();

            services.AddScoped<IProductApiFetcher, ProductApiFetcher>();
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        private (DatabaseConfig, ProducerConfig) RegisterConfigs(IServiceCollection services)
        {
            var messageProducerConfig = new ProducerConfig();
            var databaseConfig = new DatabaseConfig();
            Configuration.Bind("MessageProducer", messageProducerConfig);
            Configuration.Bind("DatabaseSettings", databaseConfig);

            services.AddSingleton(messageProducerConfig);
            services.AddSingleton(databaseConfig);

            return (databaseConfig, messageProducerConfig);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogRestApi.Repositories;
using CatalogRestApi.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CatalogRestApi
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
            //serializar el guid y el datetime como string para no tener problemas con mongo
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); 
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String)); 

            //agregando el servicio de mongo... 
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var settings = Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
                return new MongoClient(settings.ConnectionString);

            });

            services.AddSingleton<IItemsRepository, MongoDbItemsRepository>(); 

            //para que el compilador no le quite el sufijo Async a los metodos al momento del runtime
            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatalogRestApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatalogRestApi v1"));
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

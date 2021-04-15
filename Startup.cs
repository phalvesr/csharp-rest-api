using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Application.Services;
using Commands.Domain.Repositories;
using Commands.Domain.Services;
using Commands.Infrastructure.DataProviders.Databases;
using Commands.Infrastructure.DataProviders.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace Commands
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

            services.AddControllers()
            // .AddNewtonsoftJson(serializer => {
            //     serializer.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // });
            .AddNewtonsoftJson();
            
            services.AddDbContext<CommandContext>(
                options => options.UseMySQL(
                    Configuration.GetConnectionString("MariaDBDatabase")
                    // Configuration.GetConnectionString("AWS")
                )
            );

            services.AddTransient<ICommandRepository, CommandRepository>();
            services.AddTransient<ICommandServices, CommandServices>();
            
            services.AddAutoMapper(
                AppDomain.CurrentDomain.GetAssemblies()
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Commands", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Commands v1"));
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

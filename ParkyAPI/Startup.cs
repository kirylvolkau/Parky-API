using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.Mapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI
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
            
            services.AddDbContext<AppDbContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("mssql")));
            
            services.AddScoped<INationalParkRepository, NationalParkRepository>();

            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "ParkyAPI",
                        Version = "1",
                        Description = "Parky project Open API",
                        Contact = new OpenApiContact()
                        {
                            Name = "Kiryl Volkau",
                            Email = "volkaukiryl@gmail.com",
                            Url = new Uri("https://github.com/kirylvolkau")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                        }
                    });
                    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var path = Path.Combine(AppContext.BaseDirectory, xml);
                    options.IncludeXmlComments(path);
                }
            );

            services.AddAutoMapper(typeof(ParkyMappings));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("https://localhost:5001/swagger/ParkyOpenAPISpec/swagger.json", "ParkyAPI");
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
        }
    }
}
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
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.Mapper;
using ParkyAPI.Models.DTO;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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

            services.AddScoped<ITrailRepository, TrailRepository>();
            
            services.AddAutoMapper(typeof(ParkyMappings));

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1,0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer( options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            
            // services.AddSwaggerGen(options =>
            //     {
            //         options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
            //         {
            //             Title = "ParkyAPI National",
            //             Version = "1",
            //             Description = "Parky project Open API : Parks and Trails",
            //             Contact = new OpenApiContact()
            //             {
            //                 Name = "Kiryl Volkau",
            //                 Email = "volkaukiryl@gmail.com",
            //                 Url = new Uri("https://github.com/kirylvolkau")
            //             },
            //             License = new OpenApiLicense()
            //             {
            //                 Name = "MIT License",
            //                 Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //             }
            //         });
            //         
            //         options.SwaggerDoc("ParkyOpenAPISpecTr", new Microsoft.OpenApi.Models.OpenApiInfo()
            //         {
            //             Title = "ParkyAPI Trails",
            //             Version = "1",
            //             Description = "Parky project Open API : Trails",
            //             Contact = new OpenApiContact()
            //             {
            //                 Name = "Kiryl Volkau",
            //                 Email = "volkaukiryl@gmail.com",
            //                 Url = new Uri("https://github.com/kirylvolkau")
            //             },
            //             License = new OpenApiLicense()
            //             {
            //                 Name = "MIT License",
            //                 Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //             }
            //         });
            //         
            //         var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //         var path = Path.Combine(AppContext.BaseDirectory, xml);
            //         options.IncludeXmlComments(path);
            //     }
            // );
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(opt =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    opt.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                }

                opt.RoutePrefix = "";
            });

            // app.UseSwaggerUI(options =>
            //     {
            //         options.SwaggerEndpoint("https://localhost:5001/swagger/ParkyOpenAPISpec/swagger.json", "ParkyAPI");
            //         //options.SwaggerEndpoint("https://localhost:5001/swagger/ParkyOpenAPISpecTr/swagger.json", "ParkyAPI Trails");
            //     });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
        }
    }
}
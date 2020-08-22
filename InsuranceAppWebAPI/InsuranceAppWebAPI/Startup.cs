using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using InsuranceAppWebAPI.Contexts;
using InsuranceAppWebAPI.Repositories;
using InsuranceAppWebAPI.Services;

namespace InsuranceAppWebAPI
{
    [ExcludeFromCodeCoverage]
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
            //Add Automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Add Database Context and Configure it
            // sql server local database
            services.AddDbContext<InsuranceAppContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));

            // Add Repositories and services
            services.AddScoped<CustomerRepository>();
            services.AddScoped<PolicyRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPolicyService, PolicyService>();

            // Configure Authentication service
            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Domain"];
                options.Audience = Configuration["Auth0:Audience"];
            });

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger Documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Insurance App Web API",
                    Version = "v1",
                    Description = "Insurance App Web API Backend",
                    TermsOfService = new Uri("https://insuranceapp.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Julian Largo",
                        Email = "jalargot@gmail.com",
                        Url = new Uri("https://insuranceapp.com/jalargot"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under our License",
                        Url = new Uri("https://insuranceapp.com/license"),
                    }
                });

                // config to be able to add authorization header in the swagger page
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Bearer {token}"
                });
                // config to add the token in each method call
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "bearer",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                        },
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui
            // Specifying the swagger JSON endpoint
            app.UseSwaggerUI(endpoint =>
            {
                endpoint.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance App Web API");
                endpoint.RoutePrefix = string.Empty;
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var insuranceAppContext = scope.ServiceProvider.GetRequiredService<InsuranceAppContext>();
                insuranceAppContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // 2. Enable authentication middleware
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

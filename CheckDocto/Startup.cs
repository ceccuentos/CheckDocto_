using Microsoft.AspNetCore.Authentication.JwtBearer;
using CheckDocto.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace CheckDocto
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
            services.AddCors(options =>
            {
                options.AddPolicy("PermitirApiRequest",
                    builder => builder.WithOrigins("*").WithMethods("GET", "POST").AllowAnyHeader());
            });
            services.AddScoped<CheckDoctoRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                 ClockSkew = TimeSpan.Zero
             });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1.0.0",
                    Title = "Check Documento Flexline",
                    Description = "Chequea reglas de negocio Starfood para documento XML Flexline generado desde otro proveedor",
                    TermsOfService = new Uri("https://www.codevsys.cl"),
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("http://bfy.tw/4nqh")
                    },
                    
                    Contact = new OpenApiContact()
                    {
                        Name = "Codevsys Spa.",
                        Email = "cristian.contreras@codevsys.cl",
                        Url = new Uri("https://www.codevsys.cl/")
                    }
                    
                });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Se usa JWT Authorization header.  
                         Ingrese el token proporcionado.
                         Ejemplo: '12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                    }
                });
                config.IgnoreObsoleteActions();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });


            services.AddControllers(); //.AddXmlDataContractSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "CheckDocto Flexline V1");      
                //config.RoutePrefix = string.Empty;

            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

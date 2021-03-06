using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Roster.Api.ExtensionClasses;
using Roster.Core.DataAccess;
using Roster.Core.Service;
using Roster.Core.Service.Interfaces;
using Roster.Core.Models;

namespace Roster.Api
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
            var jwtSecret = Configuration["JwtSettings:Secret"];
            var connstr = "RosterDB";
            var baseurl = Configuration["BaseFixerUrl"];
            var pwd = Configuration["JwtSettings:Password"];
            // for docker db
            var connectionstr = $@"Server=db,1433;Initial Catalog=walletsystem;User ID=aeon;Password={pwd};";

            //  Repo Service
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            services.AddDbContext<RosterContext>(options => options.UseInMemoryDatabase(databaseName: connstr));
            #region Services

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRosterService, RosterService>();

            #endregion

            #region Auth/Auth Setup

            services.AddAppAuthentication(jwtSecret);
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AuthorizedAdmin", policy =>

                policy.RequireRole(UserRoles.Admin));

                opt.AddPolicy("AuthorizedMember", policy =>

                policy.RequireRole(UserRoles.Member));

            });

            #endregion
            services.AddHttpContextAccessor();
            // Adding CORS allowing all origin for development purposes
            services.AddCors();
            services.AddHttpClient("RosterColors", client =>
            {
                client.BaseAddress = new Uri(baseurl);
            }).AddTransientHttpErrorPolicy(x =>
                x.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            /*.AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            )*/


            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WalletAPI", Version = "v1" });
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    c.IncludeXmlComments(xmlPath);

            //});
            services.AddSwaggerGen(c =>
            {
                // configure SwaggerDoc and others
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // add JWT Authentication to swagger
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "RosterApi v1",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });


            });
        }
        // Method to get swagger xml path


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WalletAPI v1"));
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

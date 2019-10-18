using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VippsCaseAPI.DataAccess;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace VippsCaseAPI
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "admin",
                        ValidAudience = "user",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_key_6060JK"))
                    };
                });
            services.AddDbContext<DBContext>(options => options.UseSqlServer(Configuration["ConnectionString:VippsCaseDev"]));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // CORS enable for stripe from localhost testing through Visual Studio Code
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options
                    .WithOrigins("http://127.0.0.1:5500", "http://localhost:4200")
                    .WithHeaders("content-type", "accept", "origin")
                    .WithMethods("POST"));

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                app.UseDeveloperExceptionPage();
            }
            // CORS enable for stripe from localhost testing through Visual Studio Code
            app.UseCors(options => options
                .WithOrigins("http://127.0.0.1:5500", "http://localhost:4200")
                .WithHeaders("content-type", "accept", "origin")
                .WithMethods("POST"));

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

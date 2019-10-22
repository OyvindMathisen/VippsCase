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

namespace StripeAPI
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // CORS enable for stripe from localhost testing through Visual Studio Code
            app.UseCors(options => options
                .WithOrigins("http://127.0.0.1:5500", "http://localhost:4200")
                .WithHeaders("content-type", "accept", "origin")
                .WithMethods("POST"));


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

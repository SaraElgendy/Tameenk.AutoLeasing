
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Runtime.Loader;
using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tameenk.AutoLeasing.Identity;

namespace Tameenk.AutoLeasing.IdentityAPI
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
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddDbContext<AdminContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
           
            //var resolver = new DefaultContractResolver
            //{
            //    NamingStrategy = new CamelCaseNamingStrategy
            //    {
            //        ProcessDictionaryKeys = true,
            //        OverrideSpecifiedNames = true,
            //        ProcessExtensionDataNames = true
            //    }
            //};
            //services.AddCors(options =>
            //{
            //   // options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://37.224.26.67:7052")
            //   options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://localhost:1234")
            //     .AllowAnyMethod()
            //     .AllowAnyHeader()
            //     .AllowCredentials());
            //});
            //services.AddSignalR();

            #region MyRegion

            services.AddControllers();
            services.AddMvc();
            services.AddAdminIdentityContextConfiguration(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<AuthenticateFilterAttribute>();
           
            services.AddSwaggerGen();
            #endregion

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           

            var routePrefix = "Autolasing";
            app.UseStaticFiles()
                .UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && context.Request.Path.Value == "/")
                {
                    context.Request.Path = "/docs";
                    await next();
                }
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"../{routePrefix}/v1/swagger.json", "Admin panel API");
                c.RoutePrefix = routePrefix;
            });


            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });



        }

        internal class CustomAssemblyLoadContext : AssemblyLoadContext
        {
            public IntPtr LoadUnmanagedLibrary(string absolutePath)
            {
                return LoadUnmanagedDll(absolutePath);
            }
            protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
            {
                return LoadUnmanagedDllFromPath(unmanagedDllName);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                throw new NotImplementedException();
            }
        }

    }
}

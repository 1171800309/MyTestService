using Common.Consul;
using Common.Helper;
using GateWayService.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GateWayService
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
            //Cors����֧��
            services.AddCors(_options => _options.AddPolicy("AllowCors", _builder =>
            {
                _builder.AllowAnyOrigin().AllowAnyMethod();
                _builder.AllowAnyOrigin().AllowAnyHeader();
                //_builder.AllowCredentials().SetPreflightMaxAge(TimeSpan.FromSeconds(600));
            }));
            //ע��appsettings��ȡ��
            services.AddSingleton(new Appsettings(Configuration));
            //ע��MiddleWare
            services.AddSingleton<GlobalApiLoggingMiddleware>();
            // BaseDBConfig.LogConString = Configuration.GetSection("AppSettings:LogConString").Value;
            services.AddControllers();
            services.AddOcelot().AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //����������һ��һ��Ҫ��app.UseRouting �� UseEndpoints ֮��
            app.UseCors("AllowCors");

            app.UseAuthorization();
            app.UseMiddleware<GlobalApiLoggingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var staticFilePath = env.ContentRootPath + "\\wwwroot\\";
            StaticFileOptions staticFileOptions = new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(staticFilePath),
            };
            app.UseStaticFiles(staticFileOptions);
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseFileServer(fileServerOptions);
            ConsulHelper.ConsulRegister();
            app.UseOcelot().Wait();
        }
    }
}

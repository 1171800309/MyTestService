using Autofac;
using Common.Helper;
using Common.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestService.Filter;

namespace TestService
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //Cors֧�ֿ���
            services.AddCors(_options => _options.AddPolicy("AllowCors", _builder =>
            {
                _builder.AllowAnyOrigin().AllowAnyMethod();
                _builder.AllowAnyOrigin().AllowAnyHeader();
            }));


            //ע��appsettings��ȡ��
            services.AddSingleton(new Appsettings(Configuration));

            //jwt��Ȩ��֤
            services.AddAuthorizationSetup();

            //ע��MiddleWare
            services.AddSingleton<GlobalApiLoggingMiddleware>();

            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(GlobalExceptionsFilter));
            });
        }



        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //���ش�����
            app.UseStatusCodePages();

            app.UseRouting();

            //����������һ��һ��Ҫ��app.UseRouting �� UseEndpoints ֮��
            app.UseCors("AllowCors");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<GlobalApiLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });




        }
    }
}

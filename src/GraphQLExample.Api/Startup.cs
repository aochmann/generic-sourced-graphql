using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphiQl;
using GraphQLExample.Api.Extensions;
using GraphQLExample.Api.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GraphQLExample.Infrastructure.InMemory;
using GraphQLExample.Infrastructure;

namespace GraphQLExample.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public IContainer Container { get; private set; }

        private static string CorsPolicyName = "CorsPolicy";
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddDefaultJsonOptions()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "application/json" });
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, cors =>
                   cors.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials());
            });

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Populate(services);

            builder.RegisterModule<RepositoriesModule>();
            builder.RegisterModule<WebInfrastructureModule>();
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseGraphiQl();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseMvc();
            app.UseResponseCompression();
            app.UseCors(CorsPolicyName);

            app.Use((context, next) =>
            {
                context.Response.Headers.Add("Cache-Control", "private, max-age=0, no-cache, no-store");
                return next();
            });

            Container
                .Resolve<IDatabaseInitializer>()
                .InitializeAsync().Wait();

            applicationLifetime.ApplicationStopped.Register(() => Container.Dispose());
        }
    }
}

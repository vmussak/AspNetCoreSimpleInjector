using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjectorCore.Core;

namespace SimpleInjectorCore
{
    public class Startup
    {
        private readonly Container _container = new Container();
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddInstance<IControllerActivator>(
                new SimpleInjectorControllerActivator(_container)
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            InitializeContainer();
            RegisterControllers(app);

            _container.Verify();

            app.Use(async (context, next) => {
                using (_container)
                {
                    await next();
                }
            });

            loggerFactory.AddConsole(LogLevel.Information);
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer()
        {
           _container.Register<IPessoaService, PessoaService>();
        }

        private void RegisterControllers(IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IControllerTypeProvider>();
            var controllerTypes = provider.ControllerTypes.Select(t => t.AsType());
            foreach (var type in controllerTypes)
            {
                var registration = Lifestyle.Transient.CreateRegistration(type, _container);
                _container.AddRegistration(type, registration);
                registration.SuppressDiagnosticWarning(
                    DiagnosticType.DisposableTransientComponent,
                    "ASP.NET disposes controllers."
                );
            }
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

    internal sealed class SimpleInjectorControllerActivator : IControllerActivator
    {
        private readonly Container _container;
        public SimpleInjectorControllerActivator(Container container) { this._container = container; }

        [DebuggerStepThrough]
        public object Create(ActionContext context, Type type) => this._container.GetInstance(type);
    }
}

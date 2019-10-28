using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Hydra.WebApi.Configuration;
using Services.Hydra.WebApi.NotificationStrategies;
using Services.Hydra.WebApi.Services;

namespace Services.Hydra.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<AssistantRelaySettings>(Configuration.GetSection("AssistantRelay"));
            services.Configure<NotificationSettings>(Configuration.GetSection("NotificationSettings"));
            services.Configure<DataStorageOptions>(Configuration.GetSection("DataStorage"));
            services.Configure<SendGridConfiguration>(Configuration.GetSection("SendGrid"));

            services.AddSingleton<INotificationStrategy, GoogleNotificationStrategy>();
            services.AddSingleton<INotificationStrategy, EmailNotificationStrategy>();

            services.AddSingleton<IDocumentStorageService, DocumentStorageService>();
            services.AddSingleton<IFillStateService, FillStateService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

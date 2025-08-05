using AutoMapper;
using CRUDNotes.BL.Repositories;
using CRUDNotes.BL.Services;
using CRUDNotes.Common;
using CRUDNotes.DAL.Entities;
using CRUDNotes.DAL.Repositories;
using CRUDNotes.Site.RabbirMQ;
using Microsoft.EntityFrameworkCore;

namespace CRUDNotes.Site
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICrudNoteRepository, CrudNoteRepository>(provider =>
            {
                var mapper = provider.GetRequiredService<IMapper>();
                return new CrudNoteRepository(Configuration.GetConnectionString("DefaultConnection"), mapper);
            });

            services.AddTransient<INoteService, NoteService>();

            services.AddHostedService<RabbitConsumeService>();
            services.AddSingleton<IRabbitMQProduceService, RabbitMQProduceService>();

            services.AddMvc();

            services.AddAutoMapper(typeof(AutoMapperProfile)); 
            services.AddControllersWithViews();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        { 
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                CreateMap<NoteDTO, Note>().ReverseMap();
            }
        }
    }
}

using AutoMapper;
using CRUDNotes.BL.Repositories;
using CRUDNotes.BL.Services;
using CRUDNotes.Common;
using CRUDNotes.DAL.Entities;
using CRUDNotes.DAL.Repositories;
using CRUDNotes.Site.Models;

namespace CRUDNotes.Site
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddTransient<ICrudNoteRepository>(provider =>
            {
                var mapper = provider.GetRequiredService<IMapper>();
                var conn = Configuration.GetConnectionString("DefaultConnection");
                return new CrudNoteRepository(conn, mapper);
            });

            services.AddTransient<INoteService, NoteService>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                CreateMap<NoteDTO, Note>().ReverseMap();
                CreateMap<NoteModel, NoteDTO>().ReverseMap();
            }
        }
    }
}

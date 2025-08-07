using AutoMapper;
using CRUDNotes.BL.Repositories;
using CRUDNotes.BL.Services;
using CRUDNotes.Common;
using CRUDNotes.DAL.EF;
using CRUDNotes.DAL.Entities;
using CRUDNotes.DAL.Repositories;
using CRUDNotes.Site.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDNotes.Site
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DataBaseContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient<ICrudNoteRepository, CrudNoteRepository>();
            services.AddTransient<INoteService, NoteService>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
                dbContext.Database.EnsureCreated();
            }

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

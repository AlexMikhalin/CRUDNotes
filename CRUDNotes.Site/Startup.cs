using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRUDNotes.BL.Repositories;
using CRUDNotes.BL.Services;
using CRUDNotes.Common;
using CRUDNotes.DAL.EF;
using CRUDNotes.DAL.Entities;
using CRUDNotes.DAL.Repositories;
using CRUDNotes.Site.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CRUDNotes.Site
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
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddTransient<ICrudNoteRepository, CrudNoteRepository>(provider => new CrudNoteRepository(connection));
            services.AddTransient<INoteService, NoteService>();

            services.AddMvc();
            Mapper.Initialize(cfg => cfg.CreateMap<NoteDTO, Note>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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
    }
}

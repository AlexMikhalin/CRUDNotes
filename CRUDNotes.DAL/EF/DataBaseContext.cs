using System;
using System.Collections.Generic;
using System.Text;
using CRUDNotes.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDNotes.DAL.EF
{
    public class DataBaseContext : DbContext
    {

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}

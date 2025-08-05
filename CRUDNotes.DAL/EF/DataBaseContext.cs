using System;
using System.Collections.Generic;
using System.Text;
using CRUDNotes.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDNotes.DAL.EF
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
    {
        public DbSet<Note> Notes { get; set; }
    }
}

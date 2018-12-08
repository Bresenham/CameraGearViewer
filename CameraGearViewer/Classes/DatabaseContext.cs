using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraGearViewer.Classes
{
    public class DatabaseContext : DbContext
    {
        public DbSet<GearComponent> GearComponents{ get; set; }

        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename = database.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GearComponent>().HasKey(m => m.ForumLink);
            base.OnModelCreating(builder);
        }
    }
}

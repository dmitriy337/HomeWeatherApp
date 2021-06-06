using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HomeWeatherApp
{
    public partial class TestDbForHomeContext : DbContext
    {
        public TestDbForHomeContext()
        {
        }

        public TestDbForHomeContext(DbContextOptions<TestDbForHomeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Recording> Recordings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            
                optionsBuilder.UseNpgsql(HomeWeatherApp.Config.ConString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            


            modelBuilder.Entity<Recording>(entity =>
            {
                entity.ToTable("recordings");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Hum).HasColumnName("hum");

                entity.Property(e => e.Temp).HasColumnName("temp");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

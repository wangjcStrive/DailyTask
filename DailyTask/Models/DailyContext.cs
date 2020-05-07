using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DailyTask.Models
{
    public partial class DailyContext : DbContext
    {
        public DailyContext()
        {
        }

        public DailyContext(DbContextOptions<DailyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Daily> Daily { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\MYSQL;Database=Daily;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Daily>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EatTooMuch).HasColumnName(" EatTooMuch");

                entity.Property(e => e.Hz).HasColumnName("HZ");

                entity.Property(e => e.Jl).HasColumnName("JL");

                entity.Property(e => e.Washroom).HasColumnName("washroom");

                entity.Property(e => e.Week).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

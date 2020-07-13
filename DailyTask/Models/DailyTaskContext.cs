using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DailyTask.Models
{
    public partial class DailyTaskContext : DbContext
    {
        public DailyTaskContext()
        {
        }

        public DailyTaskContext(DbContextOptions<DailyTaskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Daily> Daily { get; set; }
        public DbSet<AccountRecord> accountRecords { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\MYSQL;Database=DailyTask;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Daily>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comments).HasMaxLength(255);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EatTooMuch).HasColumnName(" EatTooMuch");

                entity.Property(e => e.Hz).HasColumnName("HZ");

                entity.Property(e => e.Jl).HasColumnName("JL");

                entity.Property(e => e.Washroom).HasColumnName("washroom");

                entity.Property(e => e.Week).HasMaxLength(255);
            });

            modelBuilder.Entity<AccountRecord>().ToTable("AccountManagement");
            //modelBuilder.Entity<AccountRecord>(entity => 
            //{
            //    entity.Property(e => e.ID).HasColumnName("ID").ValueGeneratedNever();

            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

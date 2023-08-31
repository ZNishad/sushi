using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sushi_House.Models
{
    public partial class SushiContext : DbContext
    {
        public SushiContext()
        {
        }

        public SushiContext(DbContextOptions<SushiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Operator> Operators { get; set; } = null!;
        public virtual DbSet<Rayon> Rayons { get; set; } = null!;
        public virtual DbSet<Stat> Stats { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ZNISHAD\\SQLEXPRESS;Database=Sushi;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender");

                entity.Property(e => e.GenderId).HasColumnName("genderId");

                entity.Property(e => e.GenderName)
                    .HasMaxLength(10)
                    .HasColumnName("genderName");
            });

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.ToTable("operator");

                entity.Property(e => e.Operatorid).HasColumnName("operatorid");

                entity.Property(e => e.OperatorList).HasColumnName("operatorList");
            });

            modelBuilder.Entity<Rayon>(entity =>
            {
                entity.ToTable("rayon");

                entity.Property(e => e.RayonId).HasColumnName("rayonId");

                entity.Property(e => e.RayonName)
                    .HasMaxLength(30)
                    .HasColumnName("rayonName");
            });

            modelBuilder.Entity<Stat>(entity =>
            {
                entity.ToTable("stat");

                entity.Property(e => e.StatId).HasColumnName("statId");

                entity.Property(e => e.StatName)
                    .HasMaxLength(20)
                    .HasColumnName("statName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserAdress)
                    .HasMaxLength(120)
                    .HasColumnName("userAdress");

                entity.Property(e => e.UserBirth)
                    .HasColumnType("date")
                    .HasColumnName("userBirth");

                entity.Property(e => e.UserDescription).HasMaxLength(200);

                entity.Property(e => e.UserGenderId).HasColumnName("userGenderId");

                entity.Property(e => e.UserMail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userMail")
                    .IsFixedLength();

                entity.Property(e => e.UserName)
                    .HasMaxLength(24)
                    .HasColumnName("userName");

                entity.Property(e => e.UserNumber)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("userNumber")
                    .IsFixedLength();

                entity.Property(e => e.UserOperId).HasColumnName("userOperId");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userPassword");

                entity.Property(e => e.UserRayonId).HasColumnName("userRayonId");

                entity.Property(e => e.UserStatId).HasColumnName("userStatId");

                entity.Property(e => e.UserSurname)
                    .HasMaxLength(36)
                    .HasColumnName("userSurname");

                entity.HasOne(d => d.UserGender)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserGenderId)
                    .HasConstraintName("FK__users__userGende__37A5467C");

                entity.HasOne(d => d.UserOper)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserOperId)
                    .HasConstraintName("FK__users__userOperI__38996AB5");

                entity.HasOne(d => d.UserRayon)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRayonId)
                    .HasConstraintName("FK__users__userRayon__398D8EEE");

                entity.HasOne(d => d.UserStat)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserStatId)
                    .HasConstraintName("FK__users__userStatu__36B12243");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

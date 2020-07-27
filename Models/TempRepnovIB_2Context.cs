using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PeopleWebApi.Models
{
    public partial class TempRepnovIB_2Context : DbContext
    {
        public TempRepnovIB_2Context()
        {
        }

        public TempRepnovIB_2Context(DbContextOptions<TempRepnovIB_2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AdresType> AdresType { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<People> People { get; set; }
        public virtual DbSet<PeopleAdreses> PeopleAdreses { get; set; }
        public virtual DbSet<PeopleDocuments> PeopleDocuments { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=10.0.1.156;Database=TempRepnovIB_2;User Id=sa;Password=VVal2787;");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdresType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<People>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PeopleAdreses>(entity =>
            {
                entity.HasKey(e => new { e.PeopleId, e.AdresTypeId, e.Adres })
                    .HasName("PK__PeopleAd__0C9E48B5C315BAD2");

                entity.Property(e => e.Adres).HasMaxLength(200);

                entity.HasOne(d => d.AdresType)
                    .WithMany(p => p.PeopleAdreses)
                    .HasForeignKey(d => d.AdresTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PeopleAdr__Adres__1B0907CE");

                entity.HasOne(d => d.People)
                    .WithMany(p => p.PeopleAdreses)
                    .HasForeignKey(d => d.PeopleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PeopleAdr__Peopl__1A14E395");
            });

            modelBuilder.Entity<PeopleDocuments>(entity =>
            {
                entity.HasKey(e => new { e.PeopleId, e.DocumentTypeId, e.Number })
                    .HasName("PK__PeopleDo__BBE6A1B33D4760B3");

                entity.Property(e => e.Number).HasMaxLength(20);

                entity.Property(e => e.Seria).HasMaxLength(20);

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.PeopleDocuments)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PeopleDoc__Docum__173876EA");

                entity.HasOne(d => d.People)
                    .WithMany(p => p.PeopleDocuments)
                    .HasForeignKey(d => d.PeopleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PeopleDoc__Peopl__164452B1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK__UserRole__AF2760AD1F5DBA94");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserRoles__RoleI__22AA2996");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserRoles__UserI__21B6055D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

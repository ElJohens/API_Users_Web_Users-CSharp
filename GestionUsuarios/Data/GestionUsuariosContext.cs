using System;
using System.Collections.Generic;
using GestionUsuarios.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GestionUsuarios.Data
{
    public partial class GestionUsuariosContext : DbContext
    {
        public GestionUsuariosContext()
        {
        }

        public GestionUsuariosContext(DbContextOptions<GestionUsuariosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioRole> UsuarioRoles { get; set; }
        //Esta tabla la creo desde model first, la añadí a pata
        public virtual DbSet<Password> Password { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("GestionUsuariosContext");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleNombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.UsuarioAlias)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioApellidos)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCorreo)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioIdentificacion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioNombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioTelefono)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsuarioRole>(entity =>
            {
                entity.ToTable("Usuario_Role");

                entity.Property(e => e.UsuarioRoleId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Usuario_Role_Id");

                entity.HasOne(d => d.UsuarioRoleNavigation)
                    .WithOne(p => p.UsuarioRole)
                    .HasForeignKey<UsuarioRole>(d => d.UsuarioRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Role_Role");

                entity.HasOne(d => d.UsuarioRole1)
                    .WithOne(p => p.UsuarioRole)
                    .HasForeignKey<UsuarioRole>(d => d.UsuarioRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Role_Usuario");
            });

            

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

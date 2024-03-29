﻿// <auto-generated />
using GestionUsuarios.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestionUsuarios.Migrations
{
    [DbContext(typeof(GestionUsuariosContext))]
    [Migration("20220728032118_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GestionUsuarios.Models.Password", b =>
                {
                    b.Property<int>("PasswordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PasswordId"), 1L, 1);

                    b.Property<string>("PasswordText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserdId")
                        .HasColumnType("int");

                    b.HasKey("PasswordId");

                    b.HasIndex("UserdId");

                    b.ToTable("Password");
                });

            modelBuilder.Entity("GestionUsuarios.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"), 1L, 1);

                    b.Property<string>("RoleNombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("RoleId");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("GestionUsuarios.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioId"), 1L, 1);

                    b.Property<string>("UsuarioAlias")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UsuarioApellidos")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UsuarioCorreo")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("UsuarioIdentificacion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UsuarioNombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UsuarioTelefono")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UsuarioId");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("GestionUsuarios.Models.UsuarioRole", b =>
                {
                    b.Property<int>("UsuarioRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Usuario_Role_Id");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("UsuarioRoleId");

                    b.ToTable("Usuario_Role", (string)null);
                });

            modelBuilder.Entity("GestionUsuarios.Models.Password", b =>
                {
                    b.HasOne("GestionUsuarios.Models.Usuario", "Usuario")
                        .WithMany("Pass")
                        .HasForeignKey("UserdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionUsuarios.Models.UsuarioRole", b =>
                {
                    b.HasOne("GestionUsuarios.Models.Role", "UsuarioRoleNavigation")
                        .WithOne("UsuarioRole")
                        .HasForeignKey("GestionUsuarios.Models.UsuarioRole", "UsuarioRoleId")
                        .IsRequired()
                        .HasConstraintName("FK_Usuario_Role_Role");

                    b.HasOne("GestionUsuarios.Models.Usuario", "UsuarioRole1")
                        .WithOne("UsuarioRole")
                        .HasForeignKey("GestionUsuarios.Models.UsuarioRole", "UsuarioRoleId")
                        .IsRequired()
                        .HasConstraintName("FK_Usuario_Role_Usuario");

                    b.Navigation("UsuarioRole1");

                    b.Navigation("UsuarioRoleNavigation");
                });

            modelBuilder.Entity("GestionUsuarios.Models.Role", b =>
                {
                    b.Navigation("UsuarioRole");
                });

            modelBuilder.Entity("GestionUsuarios.Models.Usuario", b =>
                {
                    b.Navigation("Pass");

                    b.Navigation("UsuarioRole");
                });
#pragma warning restore 612, 618
        }
    }
}

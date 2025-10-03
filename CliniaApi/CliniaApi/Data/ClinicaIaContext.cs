using CliniaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CliniaApi.Data;

public class ClinicaIaContext : DbContext
{
    public ClinicaIaContext(DbContextOptions<ClinicaIaContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Medico> Medicos => Set<Medico>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<ConsultaHistorialItem> ConsultasHistorial => Set<ConsultaHistorialItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.Property(e => e.Correo).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(64);
            entity.Property(e => e.NombreCompleto).HasMaxLength(200);

            entity.HasOne(u => u.Medico)
                .WithMany(m => m.Usuarios)
                .HasForeignKey(u => u.IdMedico)
                .HasConstraintName("FK_Usuarios_Medicos");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.ToTable("Medicos");
            entity.Property(e => e.PrimerNombre).HasMaxLength(100);
            entity.Property(e => e.SegundoNombre).HasMaxLength(100);
            entity.Property(e => e.ApellidoPaterno).HasMaxLength(100);
            entity.Property(e => e.ApellidoMaterno).HasMaxLength(100);
            entity.Property(e => e.Cedula).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(30);
            entity.Property(e => e.Especialidad).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(256);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("Pacientes");
            entity.Property(e => e.PrimerNombre).HasMaxLength(100);
            entity.Property(e => e.SegundoNombre).HasMaxLength(100);
            entity.Property(e => e.ApellidoPaterno).HasMaxLength(100);
            entity.Property(e => e.ApellidoMaterno).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(30);
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.ToTable("Consultas");

            entity.HasOne(c => c.Medico)
                .WithMany(m => m.Consultas)
                .HasForeignKey(c => c.IdMedico)
                .HasConstraintName("FK_Consultas_Medicos");

            entity.HasOne(c => c.Paciente)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.IdPaciente)
                .HasConstraintName("FK_Consultas_Pacientes");
        });

        modelBuilder.Entity<ConsultaHistorialItem>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null);
        });
    }
}

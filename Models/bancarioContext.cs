using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace dss_credito_bancario_backend.Models
{
    public partial class bancarioContext : DbContext
    {
        public bancarioContext()
        {
            
        }

        public bancarioContext(DbContextOptions<bancarioContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<EstadosCivile> EstadosCiviles { get; set; }
        public virtual DbSet<Solicitude> Solicitudes { get; set; }
        public virtual DbSet<TarjetasCredito> TarjetasCreditos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.Curp)
                    .IsRequired()
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.Domicilio)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.IdEstadoCivil).HasColumnName("id_estado_civil");

                entity.Property(e => e.IngresosMensuales)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("Ingresos_mensuales");

                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Nombre_completo");

                entity.Property(e => e.UrlImagen)
                    .IsUnicode(false)
                    .HasColumnName("Url_imagen");

                entity.HasOne(d => d.IdEstadoCivilNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdEstadoCivil)
                    .HasConstraintName("FK__Clientes__id_est__2E1BDC42");
            });

            modelBuilder.Entity<EstadosCivile>(entity =>
            {
                entity.ToTable("Estados_civiles");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<Solicitude>(entity =>
            {
                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.IdTarjetaCredito).HasColumnName("id_tarjeta_credito");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Solicitud__id_cl__2C3393D0");

                entity.HasOne(d => d.IdTarjetaCreditoNavigation)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.IdTarjetaCredito)
                    .HasConstraintName("FK__Solicitud__id_ta__2D27B809");
            });

            modelBuilder.Entity<TarjetasCredito>(entity =>
            {
                entity.ToTable("Tarjetas_credito");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.LimiteCredito)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Limite_credito");

                entity.Property(e => e.MinIngresoAcumulable)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("Min_ingreso_acumulable");

                entity.Property(e => e.TasaInteresAnual)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Tasa_interes_anual");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

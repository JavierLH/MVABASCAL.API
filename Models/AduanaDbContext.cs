using Microsoft.EntityFrameworkCore;
using SistemaAduanero.API.Models.Catalogos;
using System;
using System.Collections.Generic;

namespace SistemaAduanero.API.Models;

public partial class AduanaDbContext : DbContext
{
    public AduanaDbContext()
    {
    }

    public AduanaDbContext(DbContextOptions<AduanaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ClienteCredencialesVucem> ClienteCredencialesVucems { get; set; }

    public virtual DbSet<ManifestacionCove> ManifestacionCoves { get; set; }

    public virtual DbSet<ManifestacionConceptosValor> ManifestacionConceptosValors { get; set; }

    public virtual DbSet<ManifestacionConsultaRfc> ManifestacionConsultaRfcs { get; set; }

    public virtual DbSet<ManifestacionEdocument> ManifestacionEdocuments { get; set; }

    public virtual DbSet<ManifestacionPago> ManifestacionPagos { get; set; }

    public virtual DbSet<ManifestacionesValor> ManifestacionesValors { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    //para los catalogos
    public virtual DbSet<CatIncoterm> CatIncoterms { get; set; }
    public virtual DbSet<CatMetodoValoracion> CatMetodosValoracion { get; set; }
    public virtual DbSet<CatFormaPago> CatFormasPago { get; set; }
    public virtual DbSet<CatIncrementable> CatIncrementables { get; set; }
    public virtual DbSet<CatDecrementable> CatDecrementables { get; set; }
    public virtual DbSet<CatTipoFigura> CatTipoFigura { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0875FF58F51");

            entity.HasIndex(e => e.Rfc, "UQ__Clientes__CAFFA85EBB94081D").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RazonSocial).HasMaxLength(150);
            entity.Property(e => e.Rfc)
                .HasMaxLength(13)
                .HasColumnName("RFC");
        });

        modelBuilder.Entity<ClienteCredencialesVucem>(entity =>
        {
            entity.HasKey(e => e.CredencialId).HasName("PK__ClienteC__9025E6C7C2CB0442");

            entity.ToTable("ClienteCredencialesVUCEM");

            entity.Property(e => e.PasswordKey).HasMaxLength(200);
            entity.Property(e => e.PasswordVucem).HasMaxLength(200);
            entity.Property(e => e.RfcVucem).HasMaxLength(13);
            entity.Property(e => e.VigenciaFin).HasColumnType("datetime");
            entity.Property(e => e.VigenciaInicio).HasColumnType("datetime");

            entity.HasOne(d => d.Cliente).WithMany(p => p.ClienteCredencialesVucems)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClienteCr__Clien__38996AB5");
        });

        modelBuilder.Entity<ManifestacionCove>(entity =>
        {
            entity.HasKey(e => e.CoveId).HasName("PK__Manifest__CFEA5A9949A27F58");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Incoterm).HasMaxLength(20);
            entity.Property(e => e.MetodoValoracion).HasMaxLength(50);
            entity.Property(e => e.MonedaPago).HasMaxLength(10);
            entity.Property(e => e.NumeroCove).HasMaxLength(50);
            entity.Property(e => e.NumeroPedimento).HasMaxLength(20);
            entity.Property(e => e.TipoCambioPago).HasColumnType("decimal(10, 4)");
            entity.Property(e => e.TipoPago).HasMaxLength(20);
            entity.Property(e => e.TotalDecrementables).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalIncrementables).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPago).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrecioPagado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrecioPorPagar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalValorAduana).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Manifestacion).WithMany(p => p.ManifestacionCoves)
                .HasForeignKey(d => d.ManifestacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManifestacionCoves_Padre");
        });

        modelBuilder.Entity<ManifestacionConceptosValor>(entity =>
        {
            entity.HasKey(e => e.ConceptoId).HasName("PK__Manifest__BB30F1353EAD49BE");

            entity.ToTable("ManifestacionConceptosValor");

            entity.Property(e => e.AcargoImportador)
                .HasDefaultValue(false)
                .HasColumnName("ACargoImportador");
            entity.Property(e => e.ClaveConcepto).HasMaxLength(20);
            entity.Property(e => e.Importe).HasColumnType("decimal(19, 3)");
            entity.Property(e => e.TipoCambio)
                .HasDefaultValue(1000m)
                .HasColumnType("decimal(16, 3)");
            entity.Property(e => e.TipoConcepto).HasMaxLength(15);
            entity.Property(e => e.TipoMoneda)
                .HasMaxLength(3)
                .HasDefaultValue("MXN");

            entity.HasOne(d => d.Manifestacion).WithMany(p => p.ManifestacionConceptosValors)
                .HasForeignKey(d => d.ManifestacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manifesta__Manif__5070F446");
        });

        modelBuilder.Entity<ManifestacionConsultaRfc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manifest__3214EC078CD6ED88");

            entity.ToTable("ManifestacionConsultaRFC");

            entity.Property(e => e.RfcConsulta).HasMaxLength(13);
            entity.Property(e => e.TipoFigura).HasMaxLength(15);

            entity.HasOne(d => d.Manifestacion).WithMany(p => p.ManifestacionConsultaRfcs)
                .HasForeignKey(d => d.ManifestacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manifesta__Manif__47DBAE45");
        });

        modelBuilder.Entity<ManifestacionEdocument>(entity =>
        {
            entity.HasKey(e => e.AnexoId).HasName("PK__Manifest__3A1CCBB51AE1F092");

            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.NumeroEdocument)
                .HasMaxLength(20)
                .HasColumnName("NumeroEDocument");

            entity.HasOne(d => d.Manifestacion).WithMany(p => p.ManifestacionEdocuments)
                .HasForeignKey(d => d.ManifestacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manifesta__Manif__534D60F1");
        });

        modelBuilder.Entity<ManifestacionPago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Manifest__F00B61387497F9ED");

            entity.Property(e => e.EspecifiqueFormaPago).HasMaxLength(70);
            entity.Property(e => e.FormaPago).HasMaxLength(20);
            entity.Property(e => e.ImporteTotal).HasColumnType("decimal(19, 3)");
            entity.Property(e => e.MotivoCompenso).HasMaxLength(1000);
            entity.Property(e => e.PrestacionMercancia).HasMaxLength(1000);
            entity.Property(e => e.SituacionNoPago).HasMaxLength(1000);
            entity.Property(e => e.TipoCambio).HasColumnType("decimal(16, 3)");
            entity.Property(e => e.TipoMoneda).HasMaxLength(3);
            entity.Property(e => e.TipoSeccion).HasMaxLength(20);

            entity.HasOne(d => d.Manifestacion).WithMany(p => p.ManifestacionPagos)
                .HasForeignKey(d => d.ManifestacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manifesta__Manif__4AB81AF0");
        });

        modelBuilder.Entity<ManifestacionesValor>(entity =>
        {
            entity.HasKey(e => e.ManifestacionId).HasName("PK__Manifest__4CEA1A0086B7CB5E");

            entity.ToTable("ManifestacionesValor");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Aduana).HasMaxLength(20);
            entity.Property(e => e.Cove).HasMaxLength(20);
            entity.Property(e => e.EstadoEnvio)
                .HasMaxLength(20)
                .HasDefaultValue("BORRADOR");
            entity.Property(e => e.FechaFirma).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Incoterm).HasMaxLength(15);
            entity.Property(e => e.MetodoValoracion).HasMaxLength(20);
            entity.Property(e => e.NumeroOperacionVucem).HasMaxLength(50);
            entity.Property(e => e.NumeroPedimento).HasMaxLength(20);
            entity.Property(e => e.Patente).HasMaxLength(20);
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("I")
                .IsFixedLength();
            entity.Property(e => e.TotalDecrementables)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(19, 3)");
            entity.Property(e => e.TotalIncrementables)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(19, 3)");
            entity.Property(e => e.TotalPrecioPagado)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(19, 3)");
            entity.Property(e => e.TotalPrecioPorPagar)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(19, 3)");
            entity.Property(e => e.TotalValorAduana)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(19, 3)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.ManifestacionesValors)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manifesta__Clien__440B1D61");

            entity.HasOne(d => d.UsuarioCreador).WithMany(p => p.ManifestacionesValors)
                .HasForeignKey(d => d.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manifesta__Usuar__44FF419A");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.PermisoId).HasName("PK__Permisos__96E0C72384836ACC");

            entity.HasIndex(e => e.CodigoPermiso, "UQ__Permisos__C4103855D496083A").IsUnique();

            entity.Property(e => e.CodigoPermiso).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302F1BEB85DE3");

            entity.HasIndex(e => e.NombreRol, "UQ__Roles__4F0B537F3A5DF02B").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.NombreRol).HasMaxLength(50);

            entity.HasMany(d => d.Permisos).WithMany(p => p.Rols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__Permi__300424B4"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__RolId__2F10007B"),
                    j =>
                    {
                        j.HasKey("RolId", "PermisoId").HasName("PK__RolPermi__D04D0E83D1E216B3");
                        j.ToTable("RolPermisos");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B8F11A4EE0");

            entity.HasIndex(e => e.Username, "UQ__Usuarios__A9D1053426175D9C").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.NombreCompleto).HasMaxLength(100);
            entity.Property(e => e.UltimoAcceso).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__Client__34C8D9D1");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__RolId__35BCFE0A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

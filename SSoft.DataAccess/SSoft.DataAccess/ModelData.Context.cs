﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SSoft.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class masterEntities : DbContext
    {
        public masterEntities()
            : base("name=masterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Alimento> Alimento { get; set; }
        public virtual DbSet<Animal> Animal { get; set; }
        public virtual DbSet<BodegaEstablecimiento> BodegaEstablecimiento { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<CategoriaEspecie> CategoriaEspecie { get; set; }
        public virtual DbSet<Componente> Componente { get; set; }
        public virtual DbSet<ConfiguracionPlaca> ConfiguracionPlaca { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<EmpresaEspecie> EmpresaEspecie { get; set; }
        public virtual DbSet<Especie> Especie { get; set; }
        public virtual DbSet<Establecimiento> Establecimiento { get; set; }
        public virtual DbSet<EstadoAnimal> EstadoAnimal { get; set; }
        public virtual DbSet<EstadoLote> EstadoLote { get; set; }
        public virtual DbSet<Lote> Lote { get; set; }
        public virtual DbSet<Medicamento> Medicamento { get; set; }
        public virtual DbSet<Modelo> Modelo { get; set; }
        public virtual DbSet<ModeloComponentes> ModeloComponentes { get; set; }
        public virtual DbSet<MovimientoAlimento> MovimientoAlimento { get; set; }
        public virtual DbSet<MovimientoMedicamento> MovimientoMedicamento { get; set; }
        public virtual DbSet<MovimientosAnimal> MovimientosAnimal { get; set; }
        public virtual DbSet<Recurso> Recurso { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RolRecurso> RolRecurso { get; set; }
        public virtual DbSet<Sistema> Sistema { get; set; }
        public virtual DbSet<TipoMovimiento> TipoMovimiento { get; set; }
        public virtual DbSet<TokenPlaca> TokenPlaca { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioEstablecimiento> UsuarioEstablecimiento { get; set; }
        public virtual DbSet<UsuarioRecurso> UsuarioRecurso { get; set; }
        public virtual DbSet<UsuarioRol> UsuarioRol { get; set; }
        public virtual DbSet<UsuarioSistema> UsuarioSistema { get; set; }
        public virtual DbSet<BitacoraLecturasAcciones> BitacoraLecturasAcciones { get; set; }
        public virtual DbSet<BitacoraLitrosAproxConsumido> BitacoraLitrosAproxConsumido { get; set; }
        public virtual DbSet<ListadoCorreos> ListadoCorreos { get; set; }
        public virtual DbSet<PanelControl> PanelControl { get; set; }
        public virtual DbSet<EjecucionTokenEmail> EjecucionTokenEmail { get; set; }
    }
}
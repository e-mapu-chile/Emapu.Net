//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Empresa
    {
        public Empresa()
        {
            this.EmpresaEspecie = new HashSet<EmpresaEspecie>();
            this.Establecimiento = new HashSet<Establecimiento>();
            this.TokenPlaca = new HashSet<TokenPlaca>();
        }
    
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string NombreFantasia { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public Nullable<bool> Vigente { get; set; }
        public string Rut { get; set; }
        public string NombreResponsable { get; set; }
        public string RutResponsable { get; set; }
        public string CorreoResponsable { get; set; }
    
        public virtual ICollection<EmpresaEspecie> EmpresaEspecie { get; set; }
        public virtual ICollection<Establecimiento> Establecimiento { get; set; }
        public virtual ICollection<TokenPlaca> TokenPlaca { get; set; }
    }
}
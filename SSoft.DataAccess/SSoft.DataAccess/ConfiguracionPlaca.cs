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
    
    public partial class ConfiguracionPlaca
    {
        public int Id { get; set; }
        public Nullable<int> TokenPlacaId { get; set; }
        public string Configuracion { get; set; }
        public Nullable<System.DateTime> FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaFin { get; set; }
        public Nullable<bool> Vigente { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> PorcentajeOptimo { get; set; }
        public Nullable<int> PorcentajeAccion { get; set; }
    
        public virtual TokenPlaca TokenPlaca { get; set; }
    }
}
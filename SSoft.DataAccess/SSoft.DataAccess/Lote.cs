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
    
    public partial class Lote
    {
        public Lote()
        {
            this.Animal = new HashSet<Animal>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Nullable<System.DateTime> FechaTransaccion { get; set; }
        public Nullable<bool> Vigente { get; set; }
        public Nullable<int> EstablecimientoId { get; set; }
        public Nullable<int> EstadoLoteId { get; set; }
    
        public virtual ICollection<Animal> Animal { get; set; }
        public virtual EstadoLote EstadoLote { get; set; }
    }
}

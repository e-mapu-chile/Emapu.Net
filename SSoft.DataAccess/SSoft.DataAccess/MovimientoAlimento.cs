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
    
    public partial class MovimientoAlimento
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> FechaMovimiento { get; set; }
        public Nullable<int> AlimentoId { get; set; }
        public string UnidadMedida { get; set; }
        public Nullable<int> CantidadIngreso { get; set; }
        public Nullable<int> CantidadEgreso { get; set; }
        public string PrecioIngreso { get; set; }
        public string PrecioEgreso { get; set; }
        public Nullable<int> BodegaEstablecimientoId { get; set; }
        public string Observacion { get; set; }
    
        public virtual Alimento Alimento { get; set; }
    }
}

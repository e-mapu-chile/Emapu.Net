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
    
    public partial class Componente
    {
        public Componente()
        {
            this.ModeloComponentes = new HashSet<ModeloComponentes>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> EsAnalogo { get; set; }
        public string Descripcion { get; set; }
    
        public virtual ICollection<ModeloComponentes> ModeloComponentes { get; set; }
    }
}

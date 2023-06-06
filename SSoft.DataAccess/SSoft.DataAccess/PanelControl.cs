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
    
    public partial class PanelControl
    {
        public PanelControl()
        {
            this.ListadoCorreos = new HashSet<ListadoCorreos>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Token { get; set; }
        public Nullable<bool> AguaDadaMas3Min { get; set; }
        public Nullable<bool> AguaDadaMas5Min { get; set; }
        public Nullable<bool> AguaDadaMas10Min { get; set; }
        public Nullable<bool> SinTareaEjecutando { get; set; }
        public Nullable<bool> EnvioCorreo { get; set; }
        public Nullable<bool> Vigente { get; set; }
        public Nullable<bool> EnvioCorreoInforme1Hora { get; set; }
        public Nullable<long> UnixTimeEnvioInforme { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<bool> SinTareaEjecutadoCorreo { get; set; }
    
        public virtual ICollection<ListadoCorreos> ListadoCorreos { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}

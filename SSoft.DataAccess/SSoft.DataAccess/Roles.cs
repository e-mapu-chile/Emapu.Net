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
    
    public partial class Roles
    {
        public Roles()
        {
            this.RolRecurso = new HashSet<RolRecurso>();
            this.UsuarioRol = new HashSet<UsuarioRol>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Vigente { get; set; }
    
        public virtual ICollection<RolRecurso> RolRecurso { get; set; }
        public virtual ICollection<UsuarioRol> UsuarioRol { get; set; }
    }
}

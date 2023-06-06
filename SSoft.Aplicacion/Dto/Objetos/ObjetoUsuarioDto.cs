using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Dto.Objetos
{
    public class ObjetoUsuarioDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombrePersona { get; set; }
        public string Roles { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
    }

    public class ObjetoEstablecimientoDto
    {
        public int Id { get; set; }
        public string Rup { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Titular { get; set; }
        public string Direccion { get; set; }
    }
}

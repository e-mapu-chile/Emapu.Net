using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Dto.Objetos
{
    public class ObjetoRegistroAnimalDto
    {
        public int Id { get; set; }
        public string Identificacion { get; set; }
        public string Categoria { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaCompra { get; set; }
        public string KGIngreso { get; set; }
        public string PrecioCompra { get; set; }
    }

    public class ObjetoEspecieDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class ObjetoCategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}

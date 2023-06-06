using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Dto.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Dto
{
    public class RegistroAnimalDto : MensajeDto
    {
        public List<ObjetoRegistroAnimalDto> RegistroAnimales { get; set; }
    }

    public class EspeciesDto : MensajeDto
    {
        public List<ObjetoEspecieDto> Especies { get; set; }
    }

    public class CategoriaDto : MensajeDto
    {
        public List<ObjetoCategoriaDto> Categorias { get; set; }
        public ObjetoCategoriaDto Categoria { get; set; }
    }

    public class ObjetoAnimalDto
    {
        public string IdentificacionAnimal { get; set; }
        public int EspecieId { get; set; }
        public string NombreLote { get; set; }
        public int EstablecimientoId { get; set; }
        public int CategoriaId { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string FechaCompra { get; set; }
        public string KgIngreso { get; set; }
        public string PrecioCompra { get; set; }
    }
}

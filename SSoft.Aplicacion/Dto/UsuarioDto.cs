using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Dto.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Dto
{
    public class UsuarioDto : MensajeDto
    {
        public ObjetoUsuarioDto Usuario { get; set; }
        public int RolId { get; set; }
        public List<ObjetoSistemaDto> Sistemas { get; set; }
        public List<ObjetoRecursoDto> Recursos { get; set; }
    }

    public class SistemaDto : MensajeDto
    {
        public ObjetoSistemaDto Sistema { get; set; }
        public List<ObjetoSistemaDto> Sistemas { get; set; }
    }

    public class RecursoDto : MensajeDto
    {
        public List<ObjetoRecursoDto> Recursos { get; set; }
    }

    public class EstablecimientoUsuarioDto : MensajeDto
    {
        public List<ObjetoEstablecimientoDto> Establecimientos { get; set; }
        public ObjetoEstablecimientoDto Establecimiento { get; set; }
    }
}

using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Dto.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Dto
{
    public class RegistroAlimentoDto
    {

    }

    public class AlimentosDto : MensajeDto
    {
        public List<ObjetoAlimentoDto> Alimentos { get; set; }
    }
}

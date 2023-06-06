using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Transversal.Enum
{
    public enum CodigoMensaje
    {
        Ok = 0,
        Error = 1,
        EnProceso = 2,
        NoExisteRegistro = 3,
        InsertadoOk = 4,
        ActualizadoOk = 5,
        ErrorInsercion = 6,
        ErrorActualizacion = 7,
        ArchivoSubidoOk = 8,
        ArchivoSubidoError = 9,
        PredioNoValido = 10
    }
}

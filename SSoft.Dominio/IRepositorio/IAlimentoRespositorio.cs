using SSoft.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Dominio.IRepositorio
{
    public interface IAlimentoRespositorio
    {
        IEnumerable<Alimento> ObtenerAlimentos();
        Alimento ObtenerAlimento(int id);
        Alimento ObtenerAlimento(string nombre);
        void Crear(Alimento alimento);
        void CrearBodegaAlimento(BodegaEstablecimiento alimentoBodega);
        void CrearMovimientoAlimento(MovimientoAlimento movimiento);

    }
}

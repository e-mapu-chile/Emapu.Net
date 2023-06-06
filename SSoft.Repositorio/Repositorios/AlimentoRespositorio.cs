using SSoft.DataAccess;
using SSoft.Dominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Repositorio.Repositorios
{
    public class AlimentoRespositorio : IAlimentoRespositorio
    {

        public IEnumerable<Alimento> ObtenerAlimentos()
        {
            masterEntities _db = new masterEntities();
            return _db.Alimento.Where(e => e.Vigente == true);
        }

        public Alimento ObtenerAlimento(int id)
        {
            masterEntities _db = new masterEntities();
            return _db.Alimento.FirstOrDefault(a => a.Id == id);
        }

        public Alimento ObtenerAlimento(string nombre)
        {
            masterEntities _db = new masterEntities();
            return _db.Alimento.FirstOrDefault(a => a.Nombre == nombre);
        }

        public void Crear(Alimento alimento)
        {
            var dato = new masterEntities();
            dato.Alimento.Add(alimento);
            dato.SaveChanges();
        }

        public void CrearBodegaAlimento(BodegaEstablecimiento alimentoBodega)
        {
            var dato = new masterEntities();
            dato.BodegaEstablecimiento.Add(alimentoBodega);
            dato.SaveChanges();
        }

        public void CrearMovimientoAlimento(MovimientoAlimento movimiento)
        {
            var dato = new masterEntities();
            dato.MovimientoAlimento.Add(movimiento);
            dato.SaveChanges();
        }
    }
}

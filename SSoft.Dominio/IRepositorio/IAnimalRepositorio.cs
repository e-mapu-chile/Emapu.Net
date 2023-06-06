using SSoft.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Dominio.IRepositorio
{
    public interface IAnimalRepositorio
    {
        IEnumerable<MovimientosAnimal> ObtenerMovimientosAnimal(int establecimientoId, int especieId, DateTime fechaIngreso,string lote);
        IEnumerable<Especie> ObtenerEspecies();
        IEnumerable<Categoria> ObtenerCategoriaEspecie(int idEspecie);
        Categoria ObtenerCategoria(string categoria);
        Animal ObtenerAnimal(string identificacionAnimal);
        Lote ObtenerLote(string lote, int idEstablecimiento);

        void InsertarLote(Lote lote);
        void InsertarAnimal(Animal animal);
        void InsertarMovimientoAnimal(MovimientosAnimal movimientoAnimal);
        void EliminarMovimientoXAnimal(int idAnimal);

        void ModificarEstadoLote(string lote, int idEstadoLote);
    }
}

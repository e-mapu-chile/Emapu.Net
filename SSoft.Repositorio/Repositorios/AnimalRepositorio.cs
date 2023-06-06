using SSoft.DataAccess;
using SSoft.Dominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Repositorio.Repositorios
{
    public class AnimalRepositorio : IAnimalRepositorio
    {

        public IEnumerable<MovimientosAnimal> ObtenerMovimientosAnimal(int establecimientoId, int especieId, DateTime fechaIngreso, string lote)
        {
            masterEntities _db = new masterEntities();

            var loteObj = _db.Lote.FirstOrDefault(l => l.Nombre == lote && l.Vigente == true && l.EstablecimientoId == establecimientoId);

            if (lote != null)
            {
                return _db.MovimientosAnimal.Where(m => m.EstablecimientoId == establecimientoId && m.EspecieId == especieId && m.Animal.LoteId == loteObj.Id);
            }

            else
                return null;
        }


        public IEnumerable<Especie> ObtenerEspecies()
        {
            masterEntities _db = new masterEntities();
            return _db.Especie.Where(e => e.Vigente == true);
        }

        public IEnumerable<Categoria> ObtenerCategoriaEspecie(int idEspecie)
        {
            masterEntities _db = new masterEntities();
            List<Categoria> categoriaList = new List<Categoria>();
            var data = _db.CategoriaEspecie.Where(c => c.EspecieId == idEspecie);
            foreach (var d in data)
            {
                Categoria obj = new Categoria();
                var cat = _db.Categoria.FirstOrDefault(c => c.Id == d.CategoriaId);
                obj.Id = cat.Id;
                obj.Nombre = cat.Nombre;
                categoriaList.Add(obj);
            }

            return categoriaList;
        }

        public Categoria ObtenerCategoria(string categoria)
        {
            masterEntities _db = new masterEntities();

            return _db.Categoria.FirstOrDefault(c => c.Nombre == categoria);
        }

        public void InsertarLote(Lote lote)
        {
            var dato = new masterEntities();
            dato.Lote.Add(lote);
            dato.SaveChanges();
        }


        public void InsertarAnimal(Animal animal)
        {
            var dato = new masterEntities();
            dato.Animal.Add(animal);
            dato.SaveChanges();
        }

        public void InsertarMovimientoAnimal(MovimientosAnimal movimientoAnimal)
        {
            var dato = new masterEntities();
            dato.MovimientosAnimal.Add(movimientoAnimal);
            dato.SaveChanges();
        }


        public Animal ObtenerAnimal(string identificacionAnimal)
        {
            masterEntities _db = new masterEntities();

            return _db.Animal.FirstOrDefault(f => f.IdentificacionAnimal == identificacionAnimal);
        }


        public Lote ObtenerLote(string lote, int idEstablecimiento)
        {
            masterEntities _db = new masterEntities();

            return _db.Lote.FirstOrDefault(f => f.Nombre == lote && f.Vigente == true && f.EstablecimientoId == idEstablecimiento);
        }


        public void EliminarMovimientoXAnimal(int idAnimal)
        {
            masterEntities _db = new masterEntities();
            var ctx = new masterEntities();
            var ctx2 = new masterEntities();
            var detalle = _db.MovimientosAnimal.Where(d => d.AnimalId == idAnimal);
            foreach (var d in detalle)
            {
                var obj = ctx.MovimientosAnimal.FirstOrDefault(o => o.Id == d.Id);
                ctx.MovimientosAnimal.Remove(obj);
                ctx.SaveChanges();
            }


            var obj2 = ctx2.Animal.FirstOrDefault(o => o.Id == idAnimal);
            ctx2.Animal.Remove(obj2);
            ctx2.SaveChanges();
        }


        public void ModificarEstadoLote(string lote, int idEstadoLote)
        {
            masterEntities ctx = new masterEntities();
            var result = from r in ctx.Lote
                         where r.Nombre == lote
                         select r;

            Lote loteBD = result.First();

            loteBD.EstadoLoteId = idEstadoLote;

            ctx.SaveChanges();
        }
    }
}

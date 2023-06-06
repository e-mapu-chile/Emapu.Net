using SSoft.Aplicacion.Dto;
using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Dto.Objetos;
using SSoft.DataAccess;
using SSoft.Dominio.IRepositorio;
using SSoft.Repositorio.Repositorios;
using SSoft.Transversal.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Servicios
{
    public class ServicioAnimal
    {
        #region Instanacia
        IUsuarioRepositorio _usuario;
        IAnimalRepositorio _animal;
        #endregion

        #region Constructor
        public ServicioAnimal()
            : this(new UsuarioRepositorio(), new AnimalRepositorio())
        {
        }

        public ServicioAnimal(IUsuarioRepositorio usuario, IAnimalRepositorio animal)
        {
            _usuario = usuario;
            _animal = animal;
        }
        #endregion

        #region Metodos
        #region LoteDisponible
        public bool LoteDisponible(int establecimientoId, string lote)
        {
            var data = _animal.ObtenerLote(lote, establecimientoId);

            if (data != null)
            {
                if (data.EstadoLoteId == 2)
                    return false;
            }
            return true;
        }
        #endregion
        #region ObtenerRegistroAnimal
        public RegistroAnimalDto ObtenerRegistroAnimal(int establecimientoId, int especieId, string lote)
        {
            RegistroAnimalDto dto = new RegistroAnimalDto();
            List<ObjetoRegistroAnimalDto> obj = new List<ObjetoRegistroAnimalDto>();
            DateTime fechaIngresoServer = DateTime.Now;
            try
            {

                //if (fechaIngreso.Length > 0)
                //{
                //    fechaIngresoServer = DateTime.ParseExact(fechaIngreso, "dd/MM/yyyy", null);  //Convert.ToDateTime(fechaIngreso);
                //}

                var animalesRegistrados = _animal.ObtenerMovimientosAnimal(establecimientoId, especieId, fechaIngresoServer, lote.ToUpper());

                if (animalesRegistrados.Count() > 0)
                {
                    dto.CodigoMensaje = CodigoMensaje.Ok;
                    dto.Mensaje = "OK";
                    foreach (var a in animalesRegistrados)
                    {
                        ObjetoRegistroAnimalDto o = new ObjetoRegistroAnimalDto();

                        o.Categoria = a.Animal.Categoria.Nombre;// Especie.CategoriaEspecie.First().Categoria.Nombre; Animal.CategoriaEspecie.Categoria.Nombre;
                        o.FechaCompra = a.FechaCompra.Value.ToShortDateString(); ;
                        o.FechaNacimiento = a.Animal.FechaNacimiento.Value.ToShortDateString();
                        o.Id = a.Animal.Id;
                        o.Identificacion = a.Animal.IdentificacionAnimal;
                        o.KGIngreso = a.KGIngreso;
                        o.PrecioCompra = a.PrecioCompra;

                        obj.Add(o);
                    }
                    dto.RegistroAnimales = obj;
                }
                else
                {
                    dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                    dto.Mensaje = "No existen registros";
                    dto.RegistroAnimales = obj;
                }
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Error en el sistema: " + ex.Message + ". Consulte a su ejecutivo.";
                dto.RegistroAnimales = obj;
            }
            return dto;
        }
        #endregion
        #region ObtenerEspecies
        public EspeciesDto ObtenerEspecies()
        {
            EspeciesDto dto = new EspeciesDto();
            List<ObjetoEspecieDto> obj = new List<ObjetoEspecieDto>();

            var especies = _animal.ObtenerEspecies();

            if (especies.Count() > 0)
            {
                foreach (var e in especies)
                {
                    ObjetoEspecieDto o = new ObjetoEspecieDto();

                    o.Id = e.Id;
                    o.Nombre = e.Nombre;

                    obj.Add(o);
                }
                dto.Especies = obj;
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";
            }
            else
            {
                dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                dto.Mensaje = "No existen registros";
            }
            return dto;
        }
        #endregion
        #region ObtenerCategoria
        public CategoriaDto ObtenerCategorias(int idEspecie)
        {
            CategoriaDto dto = new CategoriaDto();
            List<ObjetoCategoriaDto> obj = new List<ObjetoCategoriaDto>();

            var categoria = _animal.ObtenerCategoriaEspecie(idEspecie);

            if (categoria.Count() > 0)
            {
                foreach (var e in categoria)
                {
                    ObjetoCategoriaDto o = new ObjetoCategoriaDto();

                    o.Id = e.Id;
                    o.Nombre = e.Nombre;

                    obj.Add(o);
                }
                dto.Categorias = obj;
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";
            }
            else
            {
                dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                dto.Mensaje = "No existen registros";
            }
            return dto;
        }
        #endregion
        #region ObtenerCategoria
        public CategoriaDto ObtenerCategoria(string categoria)
        {
            CategoriaDto dto = new CategoriaDto();

            var categoriaDb = _animal.ObtenerCategoria(categoria);

            if (categoriaDb != null)
            {

                ObjetoCategoriaDto o = new ObjetoCategoriaDto();

                o.Id = categoriaDb.Id;
                o.Nombre = categoriaDb.Nombre;

                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";

                dto.Categoria = o;
            }
            else
            {
                dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                dto.Mensaje = "No existen registros";
            }
            return dto;
        }
        #endregion
        #region InsertarAnimal
        public TransaccionDto InsertarAnimal(ObjetoAnimalDto dto)
        {
            TransaccionDto transaccionDto = new TransaccionDto();
            Lote loteObj = new Lote();
            Animal animalObj = new Animal();
            MovimientosAnimal movimientoObj = new MovimientosAnimal();
            try
            {
                var datoAnimal = _animal.ObtenerAnimal(dto.IdentificacionAnimal);
                var loteDato = _animal.ObtenerLote(dto.NombreLote, dto.EstablecimientoId);
                if (datoAnimal == null)
                {
                    #region Lote
                    if (loteDato == null)
                    {
                        loteObj.Nombre = dto.NombreLote;
                        loteObj.Vigente = true;
                        loteObj.FechaTransaccion = DateTime.Now;
                        loteObj.EstablecimientoId = dto.EstablecimientoId;
                        _animal.InsertarLote(loteObj);
                    }
                    else
                    {
                        loteObj.Id = loteDato.Id;
                    }
                    #endregion
                    #region Animal
                    animalObj.CategoriaEspecieId = dto.CategoriaId;
                    animalObj.EstablecimientoId = dto.EstablecimientoId;
                    animalObj.EstadoAnimalId = 1;
                    try
                    {
                        animalObj.FechaNacimiento = DateTime.ParseExact(dto.FechaNacimiento, "dd/MM/yyyy", null); //Convert.ToDateTime(dto.FechaNacimiento);
                    }
                    catch
                    {
                        transaccionDto.CodigoMensaje = CodigoMensaje.Error;
                        transaccionDto.Mensaje = "Fecha con Errores, tiene que dejar el formato de su celda excel como texto y no como fecha.";
                        transaccionDto.IdNuevo = -2;
                        return transaccionDto;
                    }

                    animalObj.IdentificacionAnimal = dto.IdentificacionAnimal;
                    animalObj.Sexo = dto.Sexo;
                    animalObj.Vigente = true;
                    animalObj.LoteId = loteObj.Id;

                    _animal.InsertarAnimal(animalObj);
                    #endregion
                    if (animalObj.Id > 0)
                    {
                        #region Movimiento
                        movimientoObj.AnimalId = animalObj.Id;
                        movimientoObj.EstablecimientoId = dto.EstablecimientoId;

                        try
                        {
                            movimientoObj.FechaCompra = DateTime.ParseExact(dto.FechaCompra, "dd/MM/yyyy", null); // Convert.ToDateTime(dto.FechaCompra);(dto.FechaNacimiento);
                        }
                        catch
                        {
                            transaccionDto.CodigoMensaje = CodigoMensaje.Error;
                            transaccionDto.Mensaje = "Fecha con Errores, tiene que dejar el formato de su celda excel como texto y no como fecha.";
                            transaccionDto.IdNuevo = -2;
                            return transaccionDto;
                        }


                        movimientoObj.FechaMovimiento = DateTime.Now;
                        movimientoObj.KGIngreso = dto.KgIngreso;
                        movimientoObj.PrecioCompra = dto.PrecioCompra;
                        movimientoObj.TipoMovimientoId = 1;
                        movimientoObj.EspecieId = dto.EspecieId;

                        _animal.InsertarMovimientoAnimal(movimientoObj);
                        #endregion

                        transaccionDto.CodigoMensaje = CodigoMensaje.Ok;
                        transaccionDto.Mensaje = "OK";
                        transaccionDto.IdNuevo = animalObj.Id;
                    }
                    else
                    {
                        transaccionDto.CodigoMensaje = CodigoMensaje.ErrorInsercion;
                        transaccionDto.Mensaje = "Error al insertar el animal, favor contacte a su ejecutivo.";
                        transaccionDto.IdNuevo = 0;
                    }
                }
                else
                {
                    transaccionDto.CodigoMensaje = CodigoMensaje.ErrorInsercion;
                    transaccionDto.Mensaje = "El Animal ya existe.";
                    transaccionDto.IdNuevo = -3;
                }


            }
            catch (Exception ex)
            {
                transaccionDto.CodigoMensaje = CodigoMensaje.Error;
                transaccionDto.Mensaje = ex.Message;
                transaccionDto.IdNuevo = -1;
            }
            return transaccionDto;
        }
        #endregion
        #region EliminarAnimal
        public TransaccionDto EliminarAnimal(int id)
        {
            TransaccionDto transaccionDto = new TransaccionDto();

            try
            {
                _animal.EliminarMovimientoXAnimal(id);
                transaccionDto.CodigoMensaje = CodigoMensaje.Ok;
                transaccionDto.Mensaje = "OK";
                transaccionDto.IdNuevo = 1;
            }
            catch (Exception ex)
            {
                transaccionDto.CodigoMensaje = CodigoMensaje.Error;
                transaccionDto.Mensaje = ex.Message;
                transaccionDto.IdNuevo = -1;
            }
            return transaccionDto;
        }
        #endregion
        #region FinalizarLote
        public TransaccionDto FinalizarLote(int establecimientoId, string lote)
        {
            TransaccionDto transaccionDto = new TransaccionDto();

            try
            {
                var loteDato = _animal.ObtenerLote(lote, establecimientoId);
                if (loteDato != null)
                {
                    _animal.ModificarEstadoLote(loteDato.Nombre, 2);
                    transaccionDto.CodigoMensaje = CodigoMensaje.Ok;
                    transaccionDto.Mensaje = "OK";
                    transaccionDto.IdNuevo = 1;
                }
                else
                {
                    transaccionDto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                    transaccionDto.Mensaje = "El lote no existe";
                    transaccionDto.IdNuevo = 0;
                }
            }
            catch (Exception ex)
            {
                transaccionDto.CodigoMensaje = CodigoMensaje.Error;
                transaccionDto.Mensaje = ex.Message;
                transaccionDto.IdNuevo = -1;
            }
            return transaccionDto;
        }
        #endregion
        #endregion
    }
}

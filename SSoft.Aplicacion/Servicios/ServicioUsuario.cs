using SSoft.Aplicacion.Dto;
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
    public class ServicioUsuario
    {
        #region Instanacia
        IUsuarioRepositorio _usuario;
        #endregion

        #region Constructor
        public ServicioUsuario()
            : this(new UsuarioRepositorio())
        {
        }

        public ServicioUsuario(IUsuarioRepositorio usuario)
        {
            _usuario = usuario;
        }
        #endregion

        #region Metodos

        #region LoginUsuario
        public UsuarioDto LoginUsuario(string cuenta, string password,int sistemaId)
        {
            UsuarioDto dto = new UsuarioDto();
            ObjetoUsuarioDto usuarioObj = new ObjetoUsuarioDto();
            List<ObjetoRecursoDto> recursos = new List<ObjetoRecursoDto>();

            try
            {
                Usuario usuario = _usuario.ObtenerUsuario(cuenta, password);

                if (usuario != null)
                {
                    var recursosBD = _usuario.ObtenerRecursos(usuario.Id, sistemaId);

                    foreach (var s in recursosBD)
                    {
                        ObjetoRecursoDto re = new ObjetoRecursoDto();

                        re.Color = s.Color;
                        re.ColorLetra = s.ColorLetra;
                        re.Descripcion = s.Descripcion;
                        re.Id = s.Id;
                        re.Modulo = s.Modulo;
                        re.Nombre = s.Nombre;
                        re.Url = s.Url;

                        recursos.Add(re);
                    }

                    dto.CodigoMensaje = CodigoMensaje.Ok;
                    dto.Mensaje = "OK";

                    usuarioObj.IdUsuario = usuario.Id;
                    usuarioObj.NombreUsuario = usuario.UserName;
                    usuarioObj.NombrePersona = usuario.NombrePersona;
                    dto.Usuario = usuarioObj;
                    dto.Recursos = recursos;
                }
                else
                {
                    dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                    dto.Mensaje = "Usuario no Existe";
                }
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Error: " + ex.Message + "Favor contacte con su administrador";
            }
            return dto;
        }
        #endregion
        #region ObtenerSistemas
        public SistemaDto ObtenerSistemas(int usuarioId)
        {
            SistemaDto dto = new SistemaDto();
            List<ObjetoSistemaDto> sistemas = new List<ObjetoSistemaDto>();

            try
            {
                var sistemasBD = _usuario.ObtenerSistemas(usuarioId);

                foreach (var s in sistemasBD)
                {
                    ObjetoSistemaDto sis = new ObjetoSistemaDto();

                    sis.Id = s.Id;
                    sis.Nombre = s.Nombre;
                    sis.Descripcion = s.Descripcion;
                    sis.Color = s.Color;
                    sis.ColorLetra = s.ColorLetra;

                    sistemas.Add(sis);
                }

                if (sistemasBD.Count() > 0)
                {

                    dto.CodigoMensaje = CodigoMensaje.Ok;
                    dto.Mensaje = "OK";
                    dto.Sistemas = sistemas;
                }
                else
                {
                    dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                    dto.Mensaje = "No tiene sistemas asignados";
                }

            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Error: " + ex.Message + "Favor contacte con su administrador";
            }
            return dto;
        }
        #endregion
        #region ObtenerSistema
        public SistemaDto ObtenerSistema(int sistemaId)
        {
            SistemaDto dto = new SistemaDto();
            ObjetoSistemaDto sis = new ObjetoSistemaDto();
            try
            {
                var sistemasBD = _usuario.ObtenerSistema(sistemaId);

                if (sistemasBD != null)
                {
                    sis.Id = sistemasBD.Id;
                    sis.Nombre = sistemasBD.Nombre;
                    sis.Descripcion = sistemasBD.Descripcion;
                    sis.Area = sistemasBD.Area;
                    sis.Controller = sistemasBD.Controller;
                    sis.Action = sistemasBD.Action;
                    sis.Color = sistemasBD.Color;
                    sis.ColorLetra = sistemasBD.ColorLetra;

                    dto.Sistema = sis;
                }
                else
                {
                    dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                    dto.Mensaje = "No existe el sistema";
                }

            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Error: " + ex.Message + "Favor contacte con su administrador";
            }
            return dto;
        }
        #endregion
        #region ObtenerRecursos
        public RecursoDto ObtenerRecursos(int usuarioId, int sistemaId)
        {
            RecursoDto dto = new RecursoDto();
            List<ObjetoRecursoDto> recursos = new List<ObjetoRecursoDto>();

            try
            {
                var recursosBD = _usuario.ObtenerRecursos(usuarioId, sistemaId);

                foreach (var s in recursosBD)
                {
                    ObjetoRecursoDto sis = new ObjetoRecursoDto();

                    sis.Id = s.Id;
                    sis.Nombre = s.Nombre;
                    sis.Descripcion = s.Descripcion;
                    sis.Url = s.Url;
                    sis.Color = s.Color;
                    sis.ColorLetra = s.ColorLetra;
                    sis.Modulo = s.Modulo;

                    recursos.Add(sis);
                }

                if (recursosBD.Count() > 0)
                {
                    dto.CodigoMensaje = CodigoMensaje.Ok;
                    dto.Mensaje = "OK";
                    dto.Recursos = recursos;
                }
                else
                {
                    dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                    dto.Mensaje = "No tiene opciones asignadas";
                }
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Error: " + ex.Message + "Favor contacte con su administrador";
            }
            return dto;
        }
        #endregion
        #region ObtenerEstablecimientosUsuario
        public EstablecimientoUsuarioDto ObtenerEstablecimientosUsuario(int idUsuario)
        {
            EstablecimientoUsuarioDto dto = new EstablecimientoUsuarioDto();
            List<ObjetoEstablecimientoDto> obj = new List<ObjetoEstablecimientoDto>();

            var establecimientos = _usuario.ObtenerEstablecimientosUsuario(idUsuario);

            if (establecimientos.Count() > 0)
            {
                foreach (var s in establecimientos)
                {
                    ObjetoEstablecimientoDto o = new ObjetoEstablecimientoDto();

                    var estab = s.Establecimiento;
                    o.Id = estab.Id;
                    o.Nombre = estab.Nombre;
                    o.Descripcion = estab.RUP + "-" + estab.Nombre;
                    obj.Add(o);
                }
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";
                dto.Establecimientos = obj;
            }
            else
            {
                dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                dto.Mensaje = "No existen registros";
            }

            return dto;
        }
        #endregion
        #region ObtenerEstablecimiento
        public EstablecimientoUsuarioDto ObtenerEstablecimiento(int id)
        {
            EstablecimientoUsuarioDto dto = new EstablecimientoUsuarioDto();
            ObjetoEstablecimientoDto obj = new ObjetoEstablecimientoDto();

            var establecimiento = _usuario.ObtenerEstablecimiento(id);

            if (establecimiento != null)
            {
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";

                obj.Direccion = establecimiento.Direccion;
                obj.Id = id;
                obj.Nombre = establecimiento.Nombre;
                obj.Titular = establecimiento.Empresa.NombreResponsable;

                dto.Establecimiento = obj;
            }
            else
            {
                dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                dto.Mensaje = "No existen registros";
            }

            return dto;
        }
        #endregion
        #endregion
    }
}

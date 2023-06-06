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
    public class ServicioAlimento
    {
        #region Instanacia
        IUsuarioRepositorio _usuario;
        IAlimentoRespositorio _alimento;
        #endregion

        #region Constructor
        public ServicioAlimento()
            : this(new UsuarioRepositorio(), new AlimentoRespositorio())
        {
        }

        public ServicioAlimento(IUsuarioRepositorio usuario, IAlimentoRespositorio alimento)
        {
            _usuario = usuario;
            _alimento = alimento;
        }
        #endregion

        #region Metodos
        #region ObtenerAlimentos
        public AlimentosDto ObtenerAlimentos()
        {
            AlimentosDto dto = new AlimentosDto();
            List<ObjetoAlimentoDto> obj = new List<ObjetoAlimentoDto>();

            var alimentos = _alimento.ObtenerAlimentos();

            if (alimentos.Count() > 0)
            {
                foreach (var e in alimentos)
                {
                    ObjetoAlimentoDto o = new ObjetoAlimentoDto();

                    o.Id = e.Id;
                    o.Nombre = e.Nombre;

                    obj.Add(o);
                }
                dto.Alimentos = obj;
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
        #endregion
    }
}

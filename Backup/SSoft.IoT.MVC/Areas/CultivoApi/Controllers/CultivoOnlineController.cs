using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSoft.IoT.MVC.Areas.CultivoApi.Controllers
{
    public class CultivoOnlineController : ApiController
    {
        ServicioCultivoOnline _servicioCultivo = new ServicioCultivoOnline();

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //[Route("api/protocolo/ObtenerProtocolo")]
        public string ObtenerConfiguracion(string token)
        {
            MensajeDto dto = new MensajeDto();

            dto = _servicioCultivo.ObtenerConfiguracion(token, 0);

            return dto.Mensaje;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //[Route("api/protocolo/ObtenerProtocolo")]
        public string GuardarLecturaAccion(string data)
        {
            MensajeDto dto = new MensajeDto();
            try
            {
                _servicioCultivo.CrearBitacoraLecturaAccion(data);

                return "1";
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
    }
}

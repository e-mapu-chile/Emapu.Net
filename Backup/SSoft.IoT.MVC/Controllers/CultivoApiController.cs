using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Servicios;
using SSoft.Transversal.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace SSoft.IoT.MVC.Controllers
{
    public class CultivoApiController : ApiController
    {
        ServicioCultivoOnline _servicioCultivo = new ServicioCultivoOnline();

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //[Route("api/protocolo/ObtenerProtocolo")]
        public string ObtenerConfiguracion(string token)
        {
            MensajeDto dto = new MensajeDto();

            dto = _servicioCultivo.ObtenerConfiguracion(token,0);

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

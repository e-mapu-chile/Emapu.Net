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
        public string ObtenerConfiguracion(string token)
        {
            //MensajeDto dto = new MensajeDto();

            //dto = _servicioCultivo.ObtenerConfiguracion(token, 0);

            //string mensaje = "e["+dto.Mensaje+"]";

            string sUrlRequest = "https://pssoft.cl/emapu/getPlan?tokenAparato=" + token + "";
            var json = new WebClient().DownloadString(sUrlRequest);
            return json;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public string GuardarLecturaAccion(string data)
        {
            MensajeDto dto = new MensajeDto();
            try
            {
                _servicioCultivo.CrearBitacoraLecturaAccion(data);

                return "e[1]";
            }
            catch (Exception ex)
            {
                return "e[-1]";
            }
        }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public string ObtenerConfiguracionV200(string token)
        {
            MensajeDto dto = new MensajeDto();

            dto = _servicioCultivo.ObtenerConfiguracion(token, 0);

            string mensaje = "e[" + dto.Mensaje + "]";
            return mensaje;
        }
    }
}

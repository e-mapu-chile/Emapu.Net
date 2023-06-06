using SSoft.Aplicacion.Servicios;
using SSoft.MVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSoft.MVC.Controllers
{
    public class AlimentoController : Controller
    {
        //
        // GET: /Alimento/
        #region instancias
        ServicioAlimento _servicioAlimento = new ServicioAlimento();
        ServicioUsuario _servicioUsuario = new ServicioUsuario();
        CultureInfo cl = new CultureInfo("es-CL");
        #endregion

        public ActionResult Index()
        {
            return View();
        }
        #region RegistroAlimento
        public ActionResult RegistroAlimento()
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index");
            }
            int usuarioId = 0;
            int sistemaId = 0;
            try
            {
                int.TryParse(Session["IdUser"].ToString(), out usuarioId);
                int.TryParse(Session["SistemaId"].ToString(), out sistemaId);
            }
            catch
            {

            }
            if (usuarioId == 0)
            {
                Response.Redirect("../Index");
            }

            var establecimientos = from s in _servicioUsuario.ObtenerEstablecimientosUsuario(usuarioId).Establecimientos
                                   orderby s.Descripcion ascending
                                   select new ObjetoEstablecimientoModel
                                   {
                                       Id = s.Id,
                                       Descripcion = s.Descripcion
                                   };
            var alimentos = from e in _servicioAlimento.ObtenerAlimentos().Alimentos
                           orderby e.Nombre ascending
                           select new ObjetoAlimentoModel
                           {
                               Id = e.Id,
                               Descripcion = e.Nombre
                           };

            var model = new AlimentoModel()
            {
                Establecimientos = establecimientos.ToList(),
                Alimentos = alimentos.ToList()
            };
            return View(model);
        }
        #endregion
    }
}

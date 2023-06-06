using SSoft.Aplicacion.Servicios;
using SSoft.MVC.Models;
using SSoft.Transversal.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSoft.IoT.MVC.Controllers
{
    public class PerfilController : Controller
    {
        ServicioUsuario _servicioUsuario = new ServicioUsuario();

        #region Index
        public ActionResult Index()
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("Index");
            }

            int usuarioId = 0;
            int sistemaId = 0;
            try
            {
                int.TryParse(Session["IdUser"].ToString(), out usuarioId);
                int.TryParse(Session["SistemaId"].ToString(), out sistemaId);
            }
            catch { }

            List<ObjetoRecursoModel> sis = new List<ObjetoRecursoModel>();

            var recursos = _servicioUsuario.ObtenerRecursos(usuarioId, 4);

            if (recursos.CodigoMensaje == CodigoMensaje.Ok)
            {
                foreach (var s in recursos.Recursos)
                {
                    ObjetoRecursoModel mo = new ObjetoRecursoModel();

                    mo.Id = s.Id;
                    mo.Nombre = s.Nombre;
                    mo.Descripcion = s.Descripcion;
                    mo.Url = s.Url;
                    mo.Color = s.Color;
                    mo.ColorLetra = s.ColorLetra;
                    mo.Modulo = s.Modulo;

                    sis.Add(mo);
                }
            }

            var model = new RecursosModel()
            {
                recursos = sis
            };
            return View(model);
        }
        #endregion

        #region DireccionarSistema
        public ActionResult DireccionarSistema(int id)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            try
            {
                var sistema = _servicioUsuario.ObtenerSistema(id);
                Session["SistemaId"] = id.ToString();
                if (sistema.CodigoMensaje == CodigoMensaje.Ok)
                {
                    //RedirectToAction(sistema.Sistema.Action, sistema.Sistema.Controller, new { Area = sistema.Sistema.Area });
                    string url = "../" + sistema.Sistema.Area + "/" + sistema.Sistema.Controller + "/" + sistema.Sistema.Action;
                    return Json(new { url = url, mensaje = "OK", id = 1 });
                }
                else
                {
                    return Json(new { mensaje = sistema.Mensaje, id = 0 });
                }
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = "Error en el Sistema.", id = 0 });
            }
        }
        #endregion

    }
}

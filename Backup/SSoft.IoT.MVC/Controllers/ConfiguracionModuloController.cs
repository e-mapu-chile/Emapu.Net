using SSoft.Aplicacion.Dto;
using SSoft.Aplicacion.Servicios;
using SSoft.IoT.MVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SSoft.IoT.MVC.Controllers
{
    public class ConfiguracionModuloController : Controller
    {
        //
        // GET: /ConfiguracionModulo/
        ServicioCultivoOnline _servicioCultivoOnline = new ServicioCultivoOnline();
        CultureInfo cl = new CultureInfo("es-CL");

        public ActionResult Index()
        {
            return View();
        }
        #region Configuracion
        public ActionResult Configuracion()
        {

            return View();
        }
        #endregion
        #region ObtenerModelos
        [HttpPost]
        public ActionResult ObtenerModelos(string sidx, string sord, int page, int rows,
                bool _search, string searchField, string searchOper, string searchString)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
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
            IEnumerable<ObjetoTokenPlacaDto> gridRows;

            gridRows = _servicioCultivoOnline.ObtenerModelosUsuario(usuarioId);
            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredgridRows = gridRows;
            if (_search)
            {
                filteredgridRows = gridRows.Where(s =>
                    (typeof(ObjetoTokenPlacaDto).GetProperty(searchField).GetValue
                    (s, null) == null) ? false :
                        typeof(ObjetoTokenPlacaDto).GetProperty(searchField).GetValue(s, null)
                        .ToString().Contains(searchString));
            }

            // Sort the student list
            var sortedgridRows = SortIQueryable<ObjetoTokenPlacaDto>(filteredgridRows.AsQueryable(), sidx, sord);

            // Calculate the total number of pages
            var totalRecords = filteredgridRows.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid
            var data = (from s in sortedgridRows
                        orderby s.Id descending
                        select new
                        {
                            id = s.Id,
                            cell = new object[] { s.Id, s.Identificacion, s.Modelo, s.IdModelo }
                        }).ToArray();

            // Send the data to the jQGrid
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }
        #endregion
        #region ObtenerConfiguraciones
        [HttpPost]
        public ActionResult ObtenerConfiguraciones(string sidx, string sord, int page, int rows,
                bool _search, string searchField, string searchOper, string searchString, int tokenPlacaId)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
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
            IEnumerable<ObjetoConfiguracionModeloDto> gridRows;

            gridRows = _servicioCultivoOnline.ObtenerConfiguraciones(tokenPlacaId);
            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredgridRows = gridRows;
            if (_search)
            {
                filteredgridRows = gridRows.Where(s =>
                    (typeof(ObjetoConfiguracionModeloDto).GetProperty(searchField).GetValue
                    (s, null) == null) ? false :
                        typeof(ObjetoConfiguracionModeloDto).GetProperty(searchField).GetValue(s, null)
                        .ToString().Contains(searchString));
            }

            // Sort the student list
            var sortedgridRows = SortIQueryable<ObjetoConfiguracionModeloDto>(filteredgridRows.AsQueryable(), sidx, sord);

            // Calculate the total number of pages
            var totalRecords = filteredgridRows.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid
            var data = (from s in sortedgridRows
                        orderby s.Id ascending
                        select new
                        {
                            id = s.Id,
                            cell = new object[] { s.Id, s.Nombre, s.FechaInicio, s.FechaFin, s.Vigente }
                        }).ToArray();

            // Send the data to the jQGrid
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }
        #endregion
        #region ObtenerModeloComponenteFrm
        public ActionResult ObtenerModeloComponenteFrm(int idConfig, int modeloId)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
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
            var data = _servicioCultivoOnline.ObtenerConfiguracionFrm(idConfig, modeloId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CalibrarModulo
        public ActionResult CalibrarModulo()
        {
            return View();
        }
        #endregion
        #region GuardarTarea
        public ActionResult GuardarTarea(int tokenplacaId, int idConfig, string nombre, string fechaInicio, string fechaFin, string configuracion, int porcentajeOptimo, int porcentajeAccion)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            int usuarioId = 0;
            try
            {
                int.TryParse(Session["IdUser"].ToString(), out usuarioId);
            }
            catch { }

            var data = _servicioCultivoOnline.CrearEditarTarea(tokenplacaId, idConfig, nombre, fechaInicio, fechaFin, configuracion, porcentajeOptimo, porcentajeAccion);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region EliminarTarea
        public ActionResult EliminarTarea(int id)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            int usuarioId = 0;
            try
            {
                int.TryParse(Session["IdUser"].ToString(), out usuarioId);
            }
            catch { }

            var data = _servicioCultivoOnline.EliminarTarea(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GuardarCalibracion
        public ActionResult GuardarCalibracion(int tokenplacaId, int porcentaje, int valorSensor)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            int usuarioId = 0;
            try
            {
                int.TryParse(Session["IdUser"].ToString(), out usuarioId);
            }
            catch { }

            var data = _servicioCultivoOnline.EditarTokenPlacaCalibracion(tokenplacaId, porcentaje, valorSensor);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ObtenerValorSensor
        public ActionResult ObtenerValorSensor(int tokenplacaId)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            int usuarioId = 0;
            try
            {
                int.TryParse(Session["IdUser"].ToString(), out usuarioId);
            }
            catch { }

            var data = _servicioCultivoOnline.ObtenerValorSensorCalibracion(tokenplacaId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ConfigurarConexion
        public ActionResult ConfigurarConexion()
        {
            return View();
        }
        #endregion
        #region ValidarToken
        public ActionResult ValidarToken(string token)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
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
            var data = _servicioCultivoOnline.ObtenerConfiguracion(token, usuarioId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DescargarConexion
        public FileStreamResult DescargarConexion(string nombreRed, string claveRed, string token)
        {
            //todo: add some data from your database into that string:
            //movistar_C5745E&F2518C6A97CC51CAD29E&123&http://ssoft.cl
            var string_with_your_data = nombreRed + "&" + claveRed + "&" + token + "&ssoft.cl";

            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "conexion.dat");
        }
        #endregion
        #region PanelControl
        public ActionResult PanelControl()
        {
            return View();
        }
        #endregion
        #region ObtenerPanelControl
        public ActionResult ObtenerPanelControl(int tokenId)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
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
            var data = _servicioCultivoOnline.ObtenerPanelControl(tokenId, usuarioId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GuardarPanel
        public ActionResult GuardarPanel(int tokenId,int panelId, bool correo, bool tarea, bool minutos, bool informe)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
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
            var data = _servicioCultivoOnline.CrearEditarPanelControl(usuarioId,tokenId, panelId, correo, tarea, minutos, informe);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SortIQueryable
        // Utility method to sort IQueryable given a field name as "string"
        // May consider to put in a central place to be shared
        private IQueryable<T> SortIQueryable<T>(IQueryable<T> data,
            string fieldName, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return data;
            if (string.IsNullOrWhiteSpace(sortOrder)) return data;

            var param = Expression.Parameter(typeof(T), "i");
            Expression conversion = Expression.Convert
        (Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = Expression.Lambda<Func<T, object>>(conversion, param);

            return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression)
                : data.OrderBy(mySortExpression);
        }
        #endregion

    }
}

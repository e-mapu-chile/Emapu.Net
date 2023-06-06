using SSoft.Aplicacion.Dto;
using SSoft.Aplicacion.Servicios;
using SSoft.IoT.MVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace SSoft.IoT.MVC.Controllers
{
    public class GestionController : Controller
    {
        //
        // GET: /Gestion/
        ServicioCultivoOnline _servicioCultivoOnline = new ServicioCultivoOnline();
        CultureInfo cl = new CultureInfo("es-CL");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndicadorLecturaAccion()
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            return View();
        }

        #region ObtenerIndicadorLectura()
        //[CLSCompliant(false)]
        //[Authorize(Roles = "Usuario")]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult ObtenerIndicadorLectura()
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
            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;
            int dia = DateTime.Now.Day;
            //if (year != 1900)
            //{
            //    anio = year;
            //    mes = month;
            //    dia = day;
            //}
            var re = _servicioCultivoOnline.ObtenerIndicadorLectura(usuarioId);
            GraficoModelsAndBruto graphics = new GraficoModelsAndBruto();
            graphics.NacionalSubida = re;
            //List<Muestra> se = mB.MuestrasPromedio(idCpe, dia, mes, anio);
            string[] nS = new string[10000];
            string[] nB = new string[10000];
            string[] iS = new string[10000];
            string[] iB = new string[10000];

            //for (var i = 0; i < se.Count; i++)
            //{
            //    nS[i] = se[i].BytesIn;
            //    nB[i] = se[i].BytesOut;
            //    iS[i] = se[i].BytesInInternacional;
            //    iB[i] = se[i].BytesOutInternacional;

            //}

            graphics.BytesIn = nS;
            graphics.BytesOut = nB;
            graphics.BytesInInternacional = iS;
            graphics.BytesOutInternacional = iB;
            var jsonData = graphics;
            //IGraficoService cpeServices = IoC.Resolve<IGraficoService>();


            //var jsonData = cpeServices.FindGraficoByIdCpeIpAnoMesDia(idCpe, anio, mes, dia);
            ////GraficoModels poke = new GraficoModels();
            ////JsonHelper jsonHelper = new JsonHelper();
            ////poke = jsonHelper.Deserialize<GraficoModels>("{name: 'Nacional Subida',data: [7.0, 6.9, 9.5, 14.5, -1, 21.5, 25.2, 26.5, 23.3, 8.3, 13.9, 9.6]}");//(jsonData.Datos);

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ObtenerConfiguraciones
        [HttpPost]
        public ActionResult ObtenerConfiguraciones(string sidx, string sord, int page, int rows,
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
            IEnumerable<ObjetoTareasModeloLecturaDto> gridRows;

            gridRows = _servicioCultivoOnline.ObtenerConfiguracionesVigentes(usuarioId);
            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredgridRows = gridRows;
            if (_search)
            {
                filteredgridRows = gridRows.Where(s =>
                    (typeof(ObjetoTareasModeloLecturaDto).GetProperty(searchField).GetValue
                    (s, null) == null) ? false :
                        typeof(ObjetoTareasModeloLecturaDto).GetProperty(searchField).GetValue(s, null)
                        .ToString().Contains(searchString));
            }

            // Sort the student list
            var sortedgridRows = SortIQueryable<ObjetoTareasModeloLecturaDto>(filteredgridRows.AsQueryable(), sidx, sord);

            // Calculate the total number of pages
            var totalRecords = filteredgridRows.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid
            var data = (from s in sortedgridRows
                        orderby s.Id ascending
                        select new
                        {
                            id = s.Id,
                            cell = new object[] { s.Id, s.NombreEquipo, s.Nombre, s.Semaforo, s.PorcentajeActual, s.Porcentajes, s.Encendido, s.Nivel }
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
        #region ObtenerAccionesEjecutadasMaquinas
        [HttpPost]
        public ActionResult ObtenerAccionesEjecutadasMaquinas(string sidx, string sord, int page, int rows,
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
            IEnumerable<ObjetoEjecucionesMaquinaDto> gridRows;

            gridRows = _servicioCultivoOnline.ObtenerAccionesEjecutadasMaquinas(usuarioId);
            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredgridRows = gridRows;
            if (_search)
            {
                filteredgridRows = gridRows.Where(s =>
                    (typeof(ObjetoEjecucionesMaquinaDto).GetProperty(searchField).GetValue
                    (s, null) == null) ? false :
                        typeof(ObjetoEjecucionesMaquinaDto).GetProperty(searchField).GetValue(s, null)
                        .ToString().Contains(searchString));
            }

            // Sort the student list
            var sortedgridRows = SortIQueryable<ObjetoEjecucionesMaquinaDto>(filteredgridRows.AsQueryable(), sidx, sord);

            // Calculate the total number of pages
            var totalRecords = filteredgridRows.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid
            var data = (from s in sortedgridRows
                        orderby s.Id descending
                        select new
                        {
                            id = s.Id,
                            cell = new object[] { s.Id, s.Nombre, s.NombreTarea, s.FechaAccion, s.Accion }
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
        #region ObtenerSalud
        [HttpPost]
        public ActionResult ObtenerSalud(string sidx, string sord, int page, int rows,
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
            IEnumerable<ObjetoEstadoSaludMaquinaDto> gridRows;

            gridRows = _servicioCultivoOnline.ObtenerEstadoSaludMaquinas(usuarioId);
            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredgridRows = gridRows;
            if (_search)
            {
                filteredgridRows = gridRows.Where(s =>
                    (typeof(ObjetoEstadoSaludMaquinaDto).GetProperty(searchField).GetValue
                    (s, null) == null) ? false :
                        typeof(ObjetoEstadoSaludMaquinaDto).GetProperty(searchField).GetValue(s, null)
                        .ToString().Contains(searchString));
            }

            // Sort the student list
            var sortedgridRows = SortIQueryable<ObjetoEstadoSaludMaquinaDto>(filteredgridRows.AsQueryable(), sidx, sord);

            // Calculate the total number of pages
            var totalRecords = filteredgridRows.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid
            var data = (from s in sortedgridRows
                        orderby s.Id ascending
                        select new
                        {
                            id = s.Id,
                            cell = new object[] { s.Id, s.Nombre, s.EstadoTarea, s.MensajeEstadoSalud, s.EstadoSalud }
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

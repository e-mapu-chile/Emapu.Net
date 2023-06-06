using ClosedXML.Excel;
using SSoft.Aplicacion.Dto;
using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Dto.Objetos;
using SSoft.Aplicacion.Servicios;
using SSoft.MVC.Models;
using SSoft.MVC.Models.Base;
using SSoft.Transversal.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace SSoft.MVC.Controllers
{
    public class AnimalController : Controller
    {
        //
        // GET: /Animal/

        #region instancias
        ServicioAnimal _servicioAnimal = new ServicioAnimal();
        ServicioUsuario _servicioUsuario = new ServicioUsuario();
        CultureInfo cl = new CultureInfo("es-CL");
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        #region RegistroAnimal
        public ActionResult RegistroAnimal()
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
            var especies = from e in _servicioAnimal.ObtenerEspecies().Especies
                           orderby e.Nombre ascending
                           select new ObjetoEspecieModel
                           {
                               Id = e.Id,
                               Descripcion = e.Nombre
                           };

            var model = new AnimalModel()
            {
                Establecimientos = establecimientos.ToList(),
                Especies = especies.ToList()
            };
            return View(model);
        }
        #endregion
        #region ObtenerEstablecimiento
        /// <summary>
        /// Obtiene las enfermedades Tuberculosis
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ObtenerEstablecimiento(int id)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            var data = _servicioUsuario.ObtenerEstablecimiento(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ObtenerCategoria
        /// <summary>
        /// Obtiene las enfermedades Tuberculosis
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ObtenerCategoria(int idEspecie)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            var data = _servicioAnimal.ObtenerCategorias(idEspecie);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region LoteDisponible
        public ActionResult LoteDisponible(int establecimientoId,string lote)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            try
            {
                 if(_servicioAnimal.LoteDisponible(establecimientoId,lote))
                     return Json(new { mensaje = "OK", id = 1 }, JsonRequestBehavior.AllowGet);
                 else
                     return Json(new { mensaje = "El lote que quiere ingresar ya tiene animales y esta en proceso", id = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = ex.Message, id = -1 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region FinalizarLote
        public ActionResult FinalizarLote(int establecimientoId, string lote)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            try
            {
                var loteS = _servicioAnimal.FinalizarLote(establecimientoId,lote);

                if(loteS.CodigoMensaje == CodigoMensaje.Ok)
                    return Json(new { mensaje = "OK", id = 1 }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { mensaje = "No se puede finalizar el lote", id = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = ex.Message, id = -1 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region ObtenerRegistroAnimalesTable
        [HttpPost]
        public ActionResult ObtenerRegistroAnimalesTable(string sidx, string sord, int page, int rows,
                bool _search, string searchField, string searchOper, string searchString, int establecimientoId, int especieId,
            string lote)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }

            IEnumerable<ObjetoRegistroAnimalDto> gridRows;

            gridRows = _servicioAnimal.ObtenerRegistroAnimal(establecimientoId, especieId, lote).RegistroAnimales;
            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredgridRows = gridRows;
            if (_search)
            {
                filteredgridRows = gridRows.Where(s =>
                    (typeof(ObjetoRegistroAnimalDto).GetProperty(searchField).GetValue
                    (s, null) == null) ? false :
                        typeof(ObjetoRegistroAnimalDto).GetProperty(searchField).GetValue(s, null)
                        .ToString().Contains(searchString));
            }

            // Sort the student list
            var sortedgridRows = SortIQueryable<ObjetoRegistroAnimalDto>(filteredgridRows.AsQueryable(), sidx, sord);

            // Calculate the total number of pages
            var totalRecords = filteredgridRows.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid
            var data = (from s in sortedgridRows
                        orderby s.Id descending
                        select new
                        {
                            id = s.Id,
                            cell = new object[] { s.Id, s.Identificacion, s.Categoria, s.FechaNacimiento, s.FechaCompra, s.KGIngreso, s.PrecioCompra }
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
        #region AgregarAnimal
        [HttpPost]
        public ActionResult AgregarAnimal(ObjetoAnimalModel model)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            try
            {
                ObjetoAnimalDto dto = new ObjetoAnimalDto();
                dto.CategoriaId = model.CategoriaId;
                dto.EspecieId = model.EspecieId;
                dto.EstablecimientoId = model.EstablecimientoId;
                dto.FechaCompra = model.FechaCompra;
                dto.NombreLote = model.NombreLote.ToUpper();
                dto.FechaNacimiento = model.FechaNacimiento;
                dto.IdentificacionAnimal = model.IdentificacionAnimal;
                dto.KgIngreso = model.KgIngreso + " KG";
                dto.PrecioCompra = "$" + model.PrecioCompra;
                dto.Sexo = model.Sexo;

                TransaccionDto transaccion = new TransaccionDto();

                transaccion = _servicioAnimal.InsertarAnimal(dto);


                if (transaccion.IdNuevo > 0)
                {
                    return Json(new { mensaje = transaccion.Mensaje, id = 1 });
                }
                else
                {
                    return Json(new { mensaje = transaccion.Mensaje, id = 0 });
                }

            }
            catch (Exception ex)
            {
                return Json(new { mensaje = ex.Message, id = -1 });
            }
        }
        #endregion
        #region AdjuntarExcel
        /// <summary>
        /// Metodo que adjunta un archivo en binario en la BD
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileUploadJsonResult AdjuntarExcel(HttpPostedFileBase file, ObjetoAnimalMasivoModel model)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            try
            {


                if (file != null)
                {
                    //Save the uploaded Excel file.

                    string filePath = Server.MapPath("~/FilesTemp/") + Path.GetFileName(file.FileName);
                    file.SaveAs(filePath);

                    //Open the Excel file using ClosedXML.
                    using (XLWorkbook workBook = new XLWorkbook(filePath))
                    {
                        //Read the first Sheet from Excel file.
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        //Create a new DataTable.
                        DataTable dt = new DataTable();

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in workSheet.Rows())
                        {
                            //Use the first row to add columns to DataTable.
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                //Add rows to DataTable.
                                dt.Rows.Add();
                                int i = 0;
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    i++;
                                }
                            }
                        }
                        var da = dt;
                        int cantidadErrores = 0;
                        int cantidadCargados = 0;
                        int cantidadErroresFecha = 0;
                        int cantidadAnimalesExistentes = 0;
                        foreach (DataRow row in da.Rows) // Loop over the rows.
                        {
                            string identificacionAnimal = row.ItemArray[0].ToString();
                            string categoria = row.ItemArray[1].ToString();
                            string fechaNacimiento = row.ItemArray[2].ToString();
                            string fechaCompra = row.ItemArray[3].ToString();
                            string kgIngreso = row.ItemArray[4].ToString();
                            string precioCompra = row.ItemArray[5].ToString();
                            var categoriaDb = _servicioAnimal.ObtenerCategoria(categoria).Categoria;

                            if (ValidarLineaExcelCarga(row))
                            {
                                //Nuevo animal

                                ObjetoAnimalDto objIn = new ObjetoAnimalDto();

                                objIn.CategoriaId = categoriaDb.Id;
                                objIn.EspecieId = model.EspecieIdFile;
                                objIn.EstablecimientoId = model.EstablecimientoIdFile;
                                objIn.FechaCompra = fechaCompra;
                                objIn.FechaNacimiento = fechaNacimiento;
                                objIn.IdentificacionAnimal = identificacionAnimal;
                                objIn.KgIngreso = kgIngreso + " KG";
                                objIn.NombreLote = model.NombreLoteFile;
                                int precio = Convert.ToInt32(precioCompra);
                                objIn.PrecioCompra = precio.ToString("c", cl);

                                objIn.Sexo = "";
                                var nuevoAnimal = _servicioAnimal.InsertarAnimal(objIn);
                                if (nuevoAnimal.IdNuevo == -3)
                                {
                                    cantidadAnimalesExistentes++;
                                }
                                if (nuevoAnimal.IdNuevo == -2)
                                {
                                    cantidadErroresFecha++;
                                }
                                if (nuevoAnimal.IdNuevo < 1)//ALT + 124 es el or
                                {
                                    cantidadErrores++;
                                }
                                else
                                {
                                    cantidadCargados++;
                                }
                            }
                            else
                            {
                                cantidadErrores++;
                            }
                        }
                        if (cantidadErroresFecha > 0)
                            return new FileUploadJsonResult { Data = new { id = 0, mensaje = "Error en la Carga, las celdas de su planilla que tengan fecha deben ser con formato texto, favor modifique el formato de celda y vuelva a cargar.", estado = 0 } };
                        if (cantidadCargados == 0 && cantidadAnimalesExistentes > 0)
                            return new FileUploadJsonResult { Data = new { id = 0, mensaje = "Error en la Carga, los animales que estan en su planilla ya estan en su predio.", estado = 0 } };
                        if (cantidadCargados > 0 && cantidadAnimalesExistentes > 0)
                            return new FileUploadJsonResult { Data = new { id = 1, mensaje = "Animales Cargados Correctamente, pero en su planilla existen animales que estan en su predio.", estado = 1 } };
                        if (cantidadCargados == 0)
                            return new FileUploadJsonResult { Data = new { id = 0, mensaje = "Error en la Carga, la cantidad de filas cargadas estan en su totalidad con errores, favor contacte con su ejecutivo.", estado = 0 } };
                        else
                            return new FileUploadJsonResult { Data = new { id = 1, cantidad = cantidadCargados, erroes = cantidadErrores, mensaje = "Animales Cargados Correctamente.", estado = 1, jsons = "" } };

                    }
                }
                else
                {
                    return new FileUploadJsonResult { Data = new { id = 0, mensaje = "Debe cargar un archivo", estado = 0 } };
                }
            }
            catch (Exception ex)
            {
                return new FileUploadJsonResult { Data = new { id = 0, mensaje = "Error en la Carga: " + ex.Message, estado = 0 } };
            }
        }

        #endregion
        #region ValidarLineaExcelCarga
        public bool ValidarLineaExcelCarga(DataRow row)
        {
            string identificacionAnimal = row.ItemArray[0].ToString();
            string categoria = row.ItemArray[1].ToString();
            string fechaNacimiento = row.ItemArray[2].ToString();
            string fechaCompra = row.ItemArray[3].ToString();
            string kgIngreso = row.ItemArray[4].ToString();
            string precioCompra = row.ItemArray[5].ToString();
            //VALIDAR CATEGORIA
            var categoriaDb = _servicioAnimal.ObtenerCategoria(categoria).Categoria;
            if (categoriaDb == null)
                return false;

            //VALIDAMOS FECHA
            try
            {
                var dat = Convert.ToDateTime(fechaNacimiento);
                var dat2 = Convert.ToDateTime(fechaCompra);
            }
            catch
            {
                return false;
            }
            //VALIDAMOS NUMERO COMA FLOTANTE
            try
            {
                var dou = Convert.ToDouble(kgIngreso);
            }
            catch
            {
                return false;
            }
            //VALIDAMOS NUMERO INT
            try
            {
                var i = Convert.ToInt32(precioCompra);
            }
            catch
            {
                return false;
            }

            return true;

        }
        #endregion
        #region DescargarPlantilla
        public ActionResult DescargarPlantilla(int idEspecie)
        {
            try
            {
                string nombreArchivo = "PlantillaCarga_" + DateTime.Now.ToString();

                var categorias = _servicioAnimal.ObtenerCategorias(idEspecie);

                if (categorias.CodigoMensaje == CodigoMensaje.Ok)
                {

                    List<ObjetoEspecieModel> categoria = new List<ObjetoEspecieModel>();
                    List<ObjetoPlanillaCargaModel> planilla = new List<ObjetoPlanillaCargaModel>();



                    ObjetoPlanillaCargaModel obj = new ObjetoPlanillaCargaModel();
                    obj.Categoria = "";
                    obj.Fecha_Compra = "";
                    obj.Identificacion_Animal = "";
                    obj.Fecha_Nacimiento = "";
                    obj.Kg_Ingreso = "";
                    obj.Precio_Compra = "";

                    planilla.Add(obj);

                    DataTable tb1 = new DataTable();
                    tb1 = Extenders.ToDataTable(planilla);

                    foreach (var c in categorias.Categorias)
                    {
                        ObjetoEspecieModel ca = new ObjetoEspecieModel();
                        ca.Descripcion = c.Nombre;
                        categoria.Add(ca);
                    }
                    DataTable tb2 = new DataTable();
                    tb2 = Extenders.ToDataTable(categoria);




                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        wb.Worksheets.Add(tb1, "Información Animal");
                        wb.Worksheets.Add(tb2, "Categorias");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                }
                else
                {
                    throw new HttpException(404, "Couldn't find " + "PlanillaCarga");
                }


                return View();
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + "PlanillaCarga");
            }
        }
        #endregion
        #region EliminarAnimal
        public ActionResult EliminarAnimal(int id)
        {
            if ((string)Session["CodigoMensaje"] != "Ok" || Session["CodigoMensaje"] == null)
            {
                Response.Redirect("../Index/Index");
            }
            try
            {

                TransaccionDto transaccion = new TransaccionDto();

                transaccion = _servicioAnimal.EliminarAnimal(id);


                if (transaccion.IdNuevo > 0)
                {
                    return Json(new { mensaje = transaccion.Mensaje, id = 1 });
                }
                else
                {
                    return Json(new { mensaje = transaccion.Mensaje, id = 0 });
                }

            }
            catch (Exception ex)
            {
                return Json(new { mensaje = ex.Message, id = -1 });
            }
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

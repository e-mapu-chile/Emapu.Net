using SSoft.Aplicacion.Servicios;
using SSoft.MVC.Models;
using SSoft.Transversal.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSoft.MVC.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        ServicioUsuario _servicioUsuario = new ServicioUsuario();

        public ActionResult Index()
        {
            return View();
        }
        #region Login
        /// <summary>
        /// Accion al momento de logear un usuario; en donde obtiene
        /// su informacion y sus paginas que puede visualizar
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(UsuarioModel usuario)
        {
            List<ObjetoSistemasModel> sistemasModel = new List<ObjetoSistemasModel>();

            var usuarioDto = _servicioUsuario.LoginUsuario(usuario.Login, usuario.Password,1);

            if (usuarioDto.CodigoMensaje == CodigoMensaje.Ok)
            {
                Session["CodigoMensaje"] = CodigoMensaje.Ok.ToString();
                Session["IdUser"] = usuarioDto.Usuario.IdUsuario.ToString();
                Session["Nombre"] = usuarioDto.Usuario.NombrePersona;
                Session["usuario"] = usuarioDto.Usuario.NombreUsuario;
                //Session["roles"] = usuarioDto.Usuario.Roles;
                //Session["rolId"] = usuarioDto.RolId.ToString();
                Session["Recursos"] = usuarioDto.Recursos;
                //foreach (var r in usuarioDto.Sistemas)
                //{
                //    ObjetoSistemasModel t = new ObjetoSistemasModel();
                //    t.Id = r.Id;
                //    t.Nombre = r.Nombre;
                //    t.Descripcion = r.Descripcion;
                //    sistemasModel.Add(t);
                //}
            }
            else
            {
                Session["CodigoMensaje"] = usuarioDto.CodigoMensaje.ToString();
            }
            return Json(new { success = usuarioDto.CodigoMensaje, message = usuarioDto.Mensaje, sistemas = sistemasModel });
        }
        #endregion
        #region CerrarSesion
        public ActionResult CerrarSesion()
        {
            Session["CodigoMensaje"] = "";
            Session["IdUser"] = "";
            Session["Nombre"] = "";
            Session["usuario"] = "";
            Session["Recursos"] = "";
            Response.Redirect("Index");
            return View();
        }
        #endregion
    }
}

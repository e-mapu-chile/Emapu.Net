using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSoft.MVC.Models
{
    public class UsuarioModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class SistemasModel
    {
        public List<ObjetoSistemasModel> sistemas { get; set; }
    }
    public class ObjetoSistemasModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
        public string ColorLetra { get; set; }
    }
    public class RecursoModel
    {
        public int Id { get; set; }
        public string Ruta { get; set; }
        public string Nombre { get; set; }
    }

    public class UsuarioFormModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Clave { get; set; }
        public string Email { get; set; }
        public string NombrePersona { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public string TipoCuenta { get; set; }
        public int LaboratorioId { get; set; }
        public int UnidadId { get; set; }
    }

    public class ModificiarClaveModel
    {
        public string Clave5 { get; set; }
        public string Clave6 { get; set; }
    }
    public class RecursosModel
    {
        public List<ObjetoRecursoModel> recursos { get; set; }
    }
    public class ObjetoRecursoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public string Modulo { get; set; }
        public string Color { get; set; }
        public string ColorLetra { get; set; }
    }

    
   
}
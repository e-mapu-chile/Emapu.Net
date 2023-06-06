using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSoft.MVC.Models
{
    public class AnimalModel
    {
        public List<ObjetoEstablecimientoModel> Establecimientos { get; set; }
        public List<ObjetoEspecieModel> Especies { get; set; }
    }
    public class ObjetoEstablecimientoModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class ObjetoEspecieModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class ObjetoAnimalModel
    {
        public string IdentificacionAnimal { get; set; }
        public int EspecieId { get; set; }
        public string NombreLote { get; set; }
        public int EstablecimientoId { get; set; }
        public int CategoriaId { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string FechaCompra { get; set; }
        public string KgIngreso { get; set; }
        public string PrecioCompra { get; set; }
    }
    public class ObjetoAnimalMasivoModel
    {
        public int EspecieIdFile { get; set; }
        public string NombreLoteFile { get; set; }
        public int EstablecimientoIdFile { get; set; }
    }

    public class ObjetoPlanillaCargaModel
    {
        public string Identificacion_Animal { get; set; }
        public string Categoria { get; set; }
        public string Fecha_Nacimiento { get; set; }
        public string Fecha_Compra { get; set; }
        public string Kg_Ingreso { get; set; }
        public string Precio_Compra { get; set; }
    }
}
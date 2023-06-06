using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSoft.MVC.Models
{
    public class AlimentoModel
    {
        public List<ObjetoEstablecimientoModel> Establecimientos { get; set; }
        public List<ObjetoAlimentoModel> Alimentos { get; set; }
    }

   
    public class ObjetoAlimentoModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
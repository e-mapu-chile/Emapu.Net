using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSoft.IoT.MVC.Models
{
    public class CultivoOnLineModel
    {
    }

    public class ConexionConfigModel
    {
        public string NombreRed { get; set; }
        public string ClaveRed { get; set; }
        public string Token { get; set; }
    }
    public class GraficoModelsAndBruto
    {
        public string NacionalSubida { get; set; }
        public string NacionalBajada { get; set; }
        public string InternacionalSubida { get; set; }
        public string InternacionalBajada { get; set; }
        public string[] BytesIn { get; set; }
        public string[] BytesOut { get; set; }
        public string[] BytesInInternacional { get; set; }
        public string[] BytesOutInternacional { get; set; }

    }
}
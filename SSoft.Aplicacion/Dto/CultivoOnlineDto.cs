using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Dto
{
    public class CultivoOnlineDto
    {
    }

    public class ObjetoTokenPlacaDto
    {
        public int Id { get; set; }
        public int IdModelo { get; set; }
        public string Modelo { get; set; }
        public string Identificacion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }

    public class ObjetoConfiguracionModeloDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public bool Vigente { get; set; }
        public int TokenPlacaId { get; set; }
    }
    public class ObjetoTareasModeloLecturaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Configuracion { get; set; }
        public string PinAnalogo { get; set; }
        public string ValorLectura { get; set; }
        public string Porcentajes { get; set; }
        public int Semaforo { get; set; }//1 rojo, 2 amarillo, 3 verde
        public string PorcentajeActual { get; set; }
        public string Encendido { get; set; }
        public string NombreEquipo { get; set; }
        public int CienPorcientoValor { get; set; }
        public int Nivel { get; set; }//nivel de humedad colores
    }
    public class ObjetoEstadoSaludMaquinaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int EstadoSalud { get; set; }
        public string MensajeEstadoSalud { get; set; }
        public string EstadoTarea { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }

    public class ObjetoEjecucionesMaquinaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreTarea { get; set; }
        public string FechaAccion { get; set; }
        public string Accion { get; set; }
        public string LitrosMin { get; set; }
    }
    public class ObjetoComponentesFormDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int Valor1 { get; set; }
        public int Valor2 { get; set; }
        public int Valor3 { get; set; }
        public int Valor4 { get; set; }
        public int Iteracciones { get; set; }
        public int EjecutandoTarea { get; set; }
        public int SegundosAgua { get; set; }

    }

    public class ObjetoPanelControlDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool AguaDadaMas10Min { get; set; }
        public bool AguaDadaMas3Min { get; set; }
        public bool AguaDadaMas5Min { get; set; }
        public bool EnvioCorreo { get; set; }
        public bool EnvioCorreoInforme1Hora { get; set; }
        public bool SinTareaEjecutando { get; set; }
        public bool Vigente { get; set; }
        public string Descripcion { get; set; }
    }
}

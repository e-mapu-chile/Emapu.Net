using SSoft.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Dominio.IRepositorio
{
    public interface IModeloRepositorio
    {
        IEnumerable<TokenPlaca> ObtenerTokensPlaca(int usuarioId);
        IEnumerable<TokenPlaca> ObtenerTokensPlacaEmpresa(int empresaId);
        TokenPlaca ObtenerTokenPlaca(string token);
        TokenPlaca ObtenerTokenPlaca(int id);
        IEnumerable<ConfiguracionPlaca> ObtenerConfiguracionesPlaca(int tokenPlacaId);
        ConfiguracionPlaca ObtenerConfiguracionPlaca(int id);
        IEnumerable<ModeloComponentes> ObtenerModeloComponentes(int idModelo);

        void DejarONoVigenteConfiguracion(int id, bool vigente);
        void CrearTarea(ConfiguracionPlaca configuracion);
        void ModificarTarea(ConfiguracionPlaca configuracion);
        void ModificarConfiguracion(string configuracion, int tokenPlacaId);
        void EliminarTarea(int id);

        void CrearBitacoraLecturaAccion(BitacoraLecturasAcciones lecuras);
        IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLectura(int idEmpresa);
        IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaToken(string token);
        IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaToken(string token, int pin);
        IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaTokenTiempo(string token, int pin);
        int ObtenerSegundosEjecutados(string token, int pin);
        int ObtenerSegundosEjecutados(string token, int pin, string tarea);
        IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaTokenTarea(string token, string tarea, int dia, int mes, int anio);
        BitacoraLecturasAcciones ObtenerBitacoraLectura(string token, int pin);
        BitacoraLecturasAcciones ObtenerBitacoraLectura(string token, int pin, string tarea);

        void CrearBitacoraConsumoAgua(BitacoraLitrosAproxConsumido lecuras);
        //void ActualizarBitacoraConsumoAgua(BitacoraLitrosAproxConsumido lecturas);
        IEnumerable<BitacoraLitrosAproxConsumido> ObtenerBitacoraConsumoAgua(int idEmpresa);

        void ModificarCalibrarTokenPlaca(int idTokenPlaca, int porcentaje, int valorSensor, int valorBajo);

        PanelControl ObtenerPanelControl(int id);
        PanelControl ObtenerPanelControl(string token, int usuarioId);
        IEnumerable<PanelControl> ObtenerPanelesControl();
        void CrearPanel(PanelControl panel);
        void ModificarPanel(PanelControl panel);
        void ModificarPanel(int id, long unixFechaInforme);
        IEnumerable<ListadoCorreos> ObtenerCorreos(int idPanel);
        void EliminarCorreo(int idCorreo);

        EjecucionTokenEmail ObtenerEjecucionTokenEmail(string token);
        void CrearEjecucionToken(EjecucionTokenEmail token);
        void ModificarEjecucionToken(EjecucionTokenEmail token);
    }
}

using SSoft.DataAccess;
using SSoft.Dominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Repositorio.Repositorios
{
    public class ModeloRepositorio : IModeloRepositorio
    {
        public IEnumerable<TokenPlaca> ObtenerTokensPlaca(int usuarioId)
        {
            masterEntities _db = new masterEntities();
            return _db.TokenPlaca.Where(e => e.UsuarioId == usuarioId && e.Vigente == true);
        }
        public IEnumerable<TokenPlaca> ObtenerTokensPlacaEmpresa(int empresaId)
        {
            masterEntities _db = new masterEntities();
            return _db.TokenPlaca.Where(e => e.EmpresaId == empresaId && e.Vigente == true);
        }
        public TokenPlaca ObtenerTokenPlaca(int id)
        {
            masterEntities _db = new masterEntities();

            return _db.TokenPlaca.FirstOrDefault(f => f.Id == id);
        }
        public TokenPlaca ObtenerTokenPlaca(string token)
        {
            masterEntities _db = new masterEntities();

            return _db.TokenPlaca.FirstOrDefault(f => f.Token == token);
        }

        public IEnumerable<ConfiguracionPlaca> ObtenerConfiguracionesPlaca(int tokenPlacaId)
        {
            masterEntities _db = new masterEntities();
            return _db.ConfiguracionPlaca.Where(e => e.TokenPlacaId == tokenPlacaId);
        }

        public ConfiguracionPlaca ObtenerConfiguracionPlaca(int id)
        {
            masterEntities _db = new masterEntities();

            return _db.ConfiguracionPlaca.FirstOrDefault(f => f.Id == id);
        }
        public IEnumerable<ModeloComponentes> ObtenerModeloComponentes(int idModelo)
        {
            masterEntities _db = new masterEntities();
            return _db.ModeloComponentes.Where(e => e.ModeloId == idModelo);
        }

        public void DejarONoVigenteConfiguracion(int id, bool vigente)
        {
            masterEntities ctx = new masterEntities();
            var result = from r in ctx.ConfiguracionPlaca
                         where r.Id == id
                         select r;

            ConfiguracionPlaca configBD = result.First();

            configBD.Vigente = vigente;

            ctx.SaveChanges();
        }


        public void CrearTarea(ConfiguracionPlaca configuracion)
        {
            var dato = new masterEntities();
            dato.ConfiguracionPlaca.Add(configuracion);
            dato.SaveChanges();
        }

        public void ModificarTarea(ConfiguracionPlaca configuracion)
        {
            masterEntities ctx = new masterEntities();
            var result = from r in ctx.ConfiguracionPlaca
                         where r.Id == configuracion.Id
                         select r;

            ConfiguracionPlaca confPlacaBD = result.First();

            confPlacaBD.Nombre = configuracion.Nombre;
            confPlacaBD.Configuracion = configuracion.Configuracion;
            confPlacaBD.FechaFin = configuracion.FechaFin;
            confPlacaBD.FechaInicio = configuracion.FechaInicio;
            confPlacaBD.PorcentajeAccion = configuracion.PorcentajeAccion;
            confPlacaBD.PorcentajeOptimo = configuracion.PorcentajeOptimo;
            confPlacaBD.Vigente = true;

            ctx.SaveChanges();
        }
        public void ModificarConfiguracion(string configuracion, int tokenPlacaId)
        {
            masterEntities ctx = new masterEntities();



            var result = from r in ctx.ConfiguracionPlaca
                         where r.TokenPlacaId == tokenPlacaId && DateTime.Now >= r.FechaInicio && DateTime.Now <= r.FechaFin
                         select r;

            if (result.Count() > 0)
            {
                ConfiguracionPlaca confPlacaBD = result.First();

                confPlacaBD.Configuracion = configuracion;
                confPlacaBD.Vigente = true;

                ctx.SaveChanges();
            }
            else
            {
                var result2 = from r in ctx.ConfiguracionPlaca
                              where r.TokenPlacaId == tokenPlacaId
                              select r;
                ConfiguracionPlaca confPlacaBD = result2.First();

                confPlacaBD.Configuracion = configuracion;
                confPlacaBD.Vigente = false;

                ctx.SaveChanges();
            }
        }

        public void EliminarTarea(int id)
        {
            masterEntities _db = new masterEntities();
            var ctx2 = new masterEntities();

            var obj2 = ctx2.ConfiguracionPlaca.FirstOrDefault(o => o.Id == id);
            ctx2.ConfiguracionPlaca.Remove(obj2);
            ctx2.SaveChanges();
        }


        public void CrearBitacoraLecturaAccion(BitacoraLecturasAcciones lecuras)
        {
            var dato = new masterEntities();
            dato.BitacoraLecturasAcciones.Add(lecuras);
            dato.SaveChanges();
        }

        public IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLectura(int idEmpresa)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLecturasAcciones.Where(e => e.EmpresaId == idEmpresa);
        }
        public IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaTokenTarea(string token, string tarea, int dia, int mes, int anio)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLecturasAcciones.Where(e => e.Token == token && e.TareaEjecutada == tarea && e.FechaRegistro.Value.Day == dia && e.FechaRegistro.Value.Month == mes && e.FechaRegistro.Value.Year == anio);
        }
        public IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaToken(string token)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLecturasAcciones.Where(e => e.Token == token);
        }
        public IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaToken(string token, int pin)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLecturasAcciones.Where(e => e.Token == token && e.PinAnalogo == pin);
        }
        public IEnumerable<BitacoraLecturasAcciones> ObtenerBitacoraLecturaTokenTiempo(string token, int pin)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLecturasAcciones.Where(e => e.Token == token && e.PinAnalogo == pin && e.PinDigital > 0);
        }

        public int ObtenerSegundosEjecutados(string token, int pin)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLecturasAcciones.Where(e => e.Token == token && e.PinAnalogo == pin).Select(p => p.PinDigital ?? 0).Sum(p => p);
        }
        public int ObtenerSegundosEjecutados(string token, int pin, string tarea)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLecturasAcciones.Where(e => e.Token == token && e.PinAnalogo == pin && e.TareaEjecutada == tarea).Select(p => p.PinDigital ?? 0).Sum(p => p);
        }
        public BitacoraLecturasAcciones ObtenerBitacoraLectura(string token, int pin)
        {
            masterEntities _db = new masterEntities();
            var data = _db.BitacoraLecturasAcciones.Where(e => e.Token == token && e.PinAnalogo == pin).OrderByDescending(e => e.Id);

            return data.FirstOrDefault();
        }
        public BitacoraLecturasAcciones ObtenerBitacoraLectura(string token, int pin, string tarea)
        {
            masterEntities _db = new masterEntities();
            var data = _db.BitacoraLecturasAcciones.Where(e => e.Token == token && e.PinAnalogo == pin && e.TareaEjecutada == tarea).OrderByDescending(e => e.Id);

            return data.FirstOrDefault();
        }

        public void CrearBitacoraConsumoAgua(BitacoraLitrosAproxConsumido lecuras)
        {
            var dato = new masterEntities();
            dato.BitacoraLitrosAproxConsumido.Add(lecuras);
            dato.SaveChanges();
        }

        //public void ActualizarBitacoraConsumoAgua(BitacoraLitrosAproxConsumido lecturas)
        //{
        //    masterEntities ctx = new masterEntities();
        //    var result = from r in ctx.BitacoraLitrosAproxConsumido
        //                 where r.Id == lecturas.Id
        //                 select r;

        //    BitacoraLitrosAproxConsumido confPlacaBD = result.First();

        //    confPlacaBD.Nombre = configuracion.Nombre;
        //    confPlacaBD.Configuracion = configuracion.Configuracion;
        //    confPlacaBD.FechaFin = configuracion.FechaFin;
        //    confPlacaBD.FechaInicio = configuracion.FechaInicio;
        //    confPlacaBD.Vigente = false;

        //    ctx.SaveChanges();
        //}

        public IEnumerable<BitacoraLitrosAproxConsumido> ObtenerBitacoraConsumoAgua(int idEmpresa)
        {
            masterEntities _db = new masterEntities();
            return _db.BitacoraLitrosAproxConsumido.Where(e => e.EmpresaId == idEmpresa);
        }

        public void ModificarCalibrarTokenPlaca(int idTokenPlaca, int porcentaje, int valorSensor, int valorBajo)
        {
            masterEntities ctx = new masterEntities();
            var result = from r in ctx.TokenPlaca
                         where r.Id == idTokenPlaca
                         select r;

            TokenPlaca confPlacaBD = result.First();

            confPlacaBD.ValorSensor = valorSensor;
            confPlacaBD.PorcentajeCorrespondiente = porcentaje;
            confPlacaBD.ValorBajo = valorBajo;

            ctx.SaveChanges();
        }




        public PanelControl ObtenerPanelControl(int id)
        {
            masterEntities _db = new masterEntities();
            return _db.PanelControl.FirstOrDefault(p => p.Id == id);
        }

        public PanelControl ObtenerPanelControl(string token, int usuarioId)
        {
            masterEntities _db = new masterEntities();
            return _db.PanelControl.FirstOrDefault(p => p.Token == token && p.UsuarioId == usuarioId);
        }
        public IEnumerable<PanelControl> ObtenerPanelesControl()
        {
            masterEntities _db = new masterEntities();
            return _db.PanelControl.Where(p => p.Vigente == true && p.EnvioCorreo == true);
        }
        public void CrearPanel(PanelControl panel)
        {
            var dato = new masterEntities();
            dato.PanelControl.Add(panel);
            dato.SaveChanges();
        }

        public void ModificarPanel(PanelControl panel)
        {
            masterEntities ctx = new masterEntities();
            var result = from r in ctx.PanelControl
                         where r.Id == panel.Id
                         select r;

            PanelControl confPlacaBD = result.First();

            confPlacaBD.Nombre = panel.Nombre;
            confPlacaBD.AguaDadaMas10Min = panel.AguaDadaMas10Min;
            confPlacaBD.AguaDadaMas3Min = panel.AguaDadaMas3Min;
            confPlacaBD.AguaDadaMas5Min = panel.AguaDadaMas5Min;
            confPlacaBD.Descripcion = panel.Descripcion;
            confPlacaBD.EnvioCorreo = panel.EnvioCorreo;
            confPlacaBD.EnvioCorreoInforme1Hora = panel.EnvioCorreoInforme1Hora;
            confPlacaBD.SinTareaEjecutando = panel.SinTareaEjecutando;
            confPlacaBD.Vigente = panel.Vigente;

            ctx.SaveChanges();
        }

        public void ModificarPanel(int id, long unixFechaInforme)
        {
            masterEntities ctx = new masterEntities();
            var result = from r in ctx.PanelControl
                         where r.Id == id
                         select r;

            PanelControl confPlacaBD = result.First();

            confPlacaBD.UnixTimeEnvioInforme = unixFechaInforme;

            ctx.SaveChanges();
        }
        public IEnumerable<ListadoCorreos> ObtenerCorreos(int idPanel)
        {
            throw new NotImplementedException();
        }

        public void EliminarCorreo(int idCorreo)
        {
            throw new NotImplementedException();
        }


        public EjecucionTokenEmail ObtenerEjecucionTokenEmail(string token)
        {
            masterEntities _db = new masterEntities();
            return _db.EjecucionTokenEmail.FirstOrDefault(p => p.Token == token);
        }

        public void CrearEjecucionToken(EjecucionTokenEmail token)
        {
            var dato = new masterEntities();
            dato.EjecucionTokenEmail.Add(token);
            dato.SaveChanges();
        }

        public void ModificarEjecucionToken(EjecucionTokenEmail token)
        {
            masterEntities ctx = new masterEntities();
            var result = from r in ctx.EjecucionTokenEmail
                         where r.Token == token.Token
                         select r;

            EjecucionTokenEmail confPlacaBD = result.First();

            confPlacaBD.UnixTimeEjecucionEmail = token.UnixTimeEjecucionEmail;

            ctx.SaveChanges();
        }
    }
}

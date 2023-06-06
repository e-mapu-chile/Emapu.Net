using SSoft.Aplicacion.Dto;
using SSoft.Aplicacion.Dto.Base;
using SSoft.DataAccess;
using SSoft.Dominio.IRepositorio;
using SSoft.Repositorio.Repositorios;
using SSoft.Transversal.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Aplicacion.Servicios
{
    public class ServicioCultivoOnline
    {
        #region Instanacia
        IUsuarioRepositorio _usuario;
        IModeloRepositorio _modelo;
        #endregion

        #region Constructor
        public ServicioCultivoOnline()
            : this(new UsuarioRepositorio(), new ModeloRepositorio())
        {
        }

        public ServicioCultivoOnline(IUsuarioRepositorio usuario, IModeloRepositorio modelo)
        {
            _usuario = usuario;
            _modelo = modelo;
        }
        #endregion

        #region Metodos
        #region ObtenerConfiguracion
        public MensajeDto ObtenerConfiguracion(string token, int idUsuario)
        {
            MensajeDto dto = new MensajeDto();

            try
            {
                var tokenPlaca = _modelo.ObtenerTokenPlaca(token);

                if (tokenPlaca != null)
                {
                    var usuario = _usuario.ObtenerUsuario(tokenPlaca.UsuarioId.Value);

                    if (tokenPlaca.UsuarioId != idUsuario && idUsuario != 0)
                    {
                        dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                        dto.Mensaje = "La Key ingresada no existe";
                    }
                    if (tokenPlaca.ValorSensor == null || tokenPlaca.PorcentajeCorrespondiente == null || tokenPlaca.ValorSensor == -1)
                    {
                        //0&500&>&150&6;1&1000&>&500&7
                        dto.CodigoMensaje = CodigoMensaje.Ok;
                        dto.Mensaje = "0&1024&>&1024&4&-1";
                        return dto;
                    }
                    var configs = _modelo.ObtenerConfiguracionesPlaca(tokenPlaca.Id);


                    DateTime fechaActual = DateTime.Now;
                    var fechaActSinHora = fechaActual.ToShortDateString();
                    DateTime fechaAValidar = Convert.ToDateTime(fechaActSinHora);

                    EjecutarPanelControl();


                    int cantidadNoVigente = 0;
                    foreach (var c in configs)
                    {
                        if (c.FechaInicio <= fechaAValidar && fechaAValidar <= c.FechaFin)
                        {
                            _modelo.DejarONoVigenteConfiguracion(c.Id, true);
                            //RETORNAR LA CONFIGURACION
                            dto.CodigoMensaje = CodigoMensaje.Ok;
                            dto.Mensaje = c.Configuracion;
                            return dto;
                        }
                        else
                        {
                            //DEJAR NO VIGENTE LA CONFIGURACION
                            _modelo.DejarONoVigenteConfiguracion(c.Id, false);
                            cantidadNoVigente++;
                        }
                    }
                    if (cantidadNoVigente > 0)
                    {
                        dto.CodigoMensaje = CodigoMensaje.Ok;
                        dto.Mensaje = "noHay";
                        return dto;
                    }
                }
                else
                {
                    dto.CodigoMensaje = CodigoMensaje.NoExisteRegistro;
                    dto.Mensaje = "La Key ingresada no existe";
                }
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "-1";
            }
            return dto;
        }
        #endregion
        #region ObtenerModelosUsuario
        public List<ObjetoTokenPlacaDto> ObtenerModelosUsuario(int idUsuario)
        {
            List<ObjetoTokenPlacaDto> dto = new List<ObjetoTokenPlacaDto>();

            var tokenPlacas = _modelo.ObtenerTokensPlaca(idUsuario);

            foreach (var t in tokenPlacas)
            {
                ObjetoTokenPlacaDto o = new ObjetoTokenPlacaDto();
                o.Id = t.Id;
                o.IdModelo = t.ModeloId.Value;
                o.Identificacion = t.Identificacion;
                o.Latitud = t.Latitud;
                o.Longitud = t.Longitud;
                o.Modelo = t.Modelo.Nombre;

                dto.Add(o);
            }

            return dto;
        }
        #endregion
        #region ObtenerConfiguraciones
        public List<ObjetoConfiguracionModeloDto> ObtenerConfiguraciones(int tokenPlacaId)
        {
            List<ObjetoConfiguracionModeloDto> dto = new List<ObjetoConfiguracionModeloDto>();

            var configuraciones = _modelo.ObtenerConfiguracionesPlaca(tokenPlacaId);

            foreach (var t in configuraciones)
            {
                ObjetoConfiguracionModeloDto o = new ObjetoConfiguracionModeloDto();
                o.Id = t.Id;
                o.Nombre = t.Nombre;
                o.FechaInicio = t.FechaInicio.Value.ToString();
                o.FechaFin = t.FechaFin.Value.ToString();
                o.Vigente = t.Vigente.Value;
                dto.Add(o);
            }

            return dto;
        }
        #endregion
        #region ObtenerConfiguracionesVigentes
        public List<ObjetoTareasModeloLecturaDto> ObtenerConfiguracionesVigentes(int idMaquina)
        {
            List<ObjetoTareasModeloLecturaDto> dto = new List<ObjetoTareasModeloLecturaDto>();
            //int tokenTon = Int32.Parse(ton.Token);
            var configuraciones = _modelo.ObtenerConfiguracionesPlaca(idMaquina);
            foreach (var t in configuraciones)
            {
                if (t.Vigente.Value)
                {
                    if (t.Configuracion.Length > 0)
                    {
                        var configVal = t.Configuracion.Split('|');

                        for (int ia = 0; ia < configVal.Length; ia++)
                        {
                            //e|700|760|800|810|5|0|0|0
                            if (ia > 0 && ia < 5)
                            {
                                //700 & 760 & 800 & 810
                                ObjetoTareasModeloLecturaDto o = new ObjetoTareasModeloLecturaDto();
                                o.Id = t.Id;
                                o.Nombre = t.Nombre;
                                o.Configuracion = t.Configuracion;
                                var lectura = _modelo.ObtenerBitacoraLectura(t.TokenPlaca.Token, ia, t.Nombre);
                                if (lectura != null)
                                {

                                    o.CienPorcientoValor = 100;
                                    o.PinAnalogo = lectura.PinAnalogo.ToString();
                                    o.ValorLectura = lectura.Valor;
                                    var arrV = lectura.Valor.Split('.');
                                    int valorLecturaPr = Int32.Parse(arrV[0]);
                                    var valueValorAccionI = Convert.ToDouble(arrV[0]);
                                    if (lectura.Valor.Length > 0)
                                    {
                                        int lecturaInt = Int32.Parse(lectura.Valor);
                                        //999 100
                                        //134  > valor sensor
                                        //865  >difernecia entre 999 - valor sensor
                                        //100  
                                        //965  >se le agrega el margen que no se medira, 100 al 999; es decir se suman 100
                                        //entoncs seria 965*100/999 =====>   96,5965966
                                        int valorParaCalculo = (999 - lecturaInt) + 100;
                                        // int valorParaCalculo = 899 - lecturaInt;
                                        double valorCalculoFloat = Convert.ToDouble(valorParaCalculo);
                                        double porcentaje = (valorCalculoFloat * (double)100) / (double)999;
                                        var valo = porcentaje.ToString().Split(',');
                                        int porcentajeInt = Int32.Parse(valo[0]);
                                        o.PorcentajeActual = porcentajeInt.ToString() + "%";

                                    }
                                    else
                                    {
                                        o.PorcentajeActual = "";
                                    }
                                    ///  o.PorcentajeActual = lectura.Valor;
                                    int acumSeg = _modelo.ObtenerSegundosEjecutados(t.TokenPlaca.Token, ia,t.Nombre);
                                    //1/4 pulgadas	goteo	1,65
                                    //salida agua 0.75 LxMin
                                    decimal minutosSegDec = (decimal)acumSeg / (decimal)60;
                                    var litrosMin = (Convert.ToDouble(minutosSegDec) * 0.75);
                                    o.Encendido = litrosMin.ToString() + " litros";


                                    //o.Encendido = .Sum(a => a.AccionEjecutada);//Int32.Parse(lectura.AccionEjecutada);

                                    #region NEGOCIO DE RANGO NIVELES DE HUMEDAD
                                    //CONFIGURACION EUROPA
                                    //                                        if (accion > 0 && accion < 280)
                                    //$('#input' + numInput + 'Class').attr('class', 'muyHumedo');
                                    //                                        if (accion >= 280 && accion < 529)
                                    //$('#input' + numInput + 'Class').attr('class', 'humedo');
                                    //                                        if (accion >= 529 && accion < 730)
                                    //$('#input' + numInput + 'Class').attr('class', 'medioHumedo');
                                    //                                        if (accion >= 730 && accion < 931)
                                    //$('#input' + numInput + 'Class').attr('class', 'medioSeco');
                                    //                                        if (accion >= 931 && accion < 1025)
                                    //$('#input' + numInput + 'Class').attr('class', 'seco');

                                    if (valorLecturaPr > 0 && valorLecturaPr < 280)
                                        o.Nivel = 1;//muy humedo
                                    if (valorLecturaPr >= 280 && valorLecturaPr < 529)
                                        o.Nivel = 2;// humedo
                                    if (valorLecturaPr >= 529 && valorLecturaPr < 730)
                                        o.Nivel = 3;//medio humedo
                                    if (valorLecturaPr >= 730 && valorLecturaPr < 931)
                                        o.Nivel = 4;//medio seco
                                    if (valorLecturaPr >= 931 && valorLecturaPr < 1025)
                                        o.Nivel = 5;//seco


                                    #endregion
                                }

                                if (configVal[ia].Length > 0)
                                {
                                    int lecturaInt = Int32.Parse(configVal[ia]);
                                    //999 100
                                    //134  > valor sensor
                                    //865  >difernecia entre 999 - valor sensor
                                    //100  
                                    //965  >se le agrega el margen que no se medira, 100 al 999; es decir se suman 100
                                    //entoncs seria 965*100/999 =====>   96,5965966
                                    int valorParaCalculo = (999 - lecturaInt) + 100;
                                    // int valorParaCalculo = 899 - lecturaInt;
                                    double valorCalculoFloat = Convert.ToDouble(valorParaCalculo);
                                    double porcentaje = (valorCalculoFloat * (double)100) / (double)999;
                                    var valo = porcentaje.ToString().Split(',');
                                    int porcentajeInt = Int32.Parse(valo[0]);
                                    o.Porcentajes = porcentajeInt.ToString() + "%";

                                }
                                else
                                {
                                    o.Porcentajes = "";
                                }
                                o.NombreEquipo = "Puerto PIN: " + ia.ToString();
                                dto.Add(o);
                            }
                        }
                    }
                }
            }
            return dto;
        }
        #endregion
        #region ObtenerEstadoSaludMaquinas
        public List<ObjetoEstadoSaludMaquinaDto> ObtenerEstadoSaludMaquinas(int usuarioId)
        {
            List<ObjetoEstadoSaludMaquinaDto> dto = new List<ObjetoEstadoSaludMaquinaDto>();
            var empresT = _usuario.ObtenerEstablecimientosUsuario(usuarioId);
            foreach (var p in empresT)
            {
                var tokenss = _modelo.ObtenerTokensPlacaEmpresa(p.Establecimiento.EmpresaId.Value);
                foreach (var ton in tokenss)
                {
                    ObjetoEstadoSaludMaquinaDto o = new ObjetoEstadoSaludMaquinaDto();
                    int tokenTon = Int32.Parse(ton.Token);
                    var configuraciones = _modelo.ObtenerConfiguracionesPlaca(ton.Id);
                    int configVigente = 0;
                    foreach (var t in configuraciones)
                    {
                        if (t.Vigente.Value)
                            configVigente++;
                    }

                    if (configVigente > 0)
                        o.EstadoTarea = "Activo";//CON TAREAS EN PROCESO
                    else
                        o.EstadoTarea = "Inactivo";//NO EJECUTA ACTUALMENTE ACTIVIDAD

                    int empresaId = _usuario.ObtenerEstablecimientosUsuario(usuarioId).First().Establecimiento.EmpresaId.Value;

                    var modl = _modelo.ObtenerBitacoraLecturaToken(ton.Token);
                    if (modl.Count() > 0)
                    {
                        var lectura = modl.OrderByDescending(f => f.Id).First();

                        DateTime fechaActual = DateTime.Now;

                        TimeSpan ts = fechaActual - lectura.FechaRegistro.Value;



                        if (ton.PorcentajeCorrespondiente == null || ton.ValorSensor == null || ton.ValorSensor == -1)
                        {
                            o.EstadoSalud = -3;
                            o.MensajeEstadoSalud = "equipo sin calibrar";
                        }
                        else
                        {
                            if (ts.Days > 1)
                            {
                                o.EstadoSalud = -2;
                                o.MensajeEstadoSalud = "supero un dia con problemas!";
                            }
                            else
                            {
                                if (ts.Minutes > 10 && ts.Minutes < 15)
                                {
                                    o.EstadoSalud = 0;
                                    o.MensajeEstadoSalud = "Sin Conexión!";
                                }
                                if (ts.Minutes > 15)
                                {
                                    o.EstadoSalud = -1;
                                    o.MensajeEstadoSalud = "Con Problemas!";
                                }
                                if (ts.Minutes <= 10)
                                {
                                    o.EstadoSalud = 1;
                                    o.MensajeEstadoSalud = "Estado OK!";
                                }
                            }
                        }
                    }
                    else
                    {
                        o.EstadoSalud = -3;
                        o.MensajeEstadoSalud = "equipo sin calibrar";
                    }

                    o.Nombre = ton.Identificacion;
                    o.Latitud = ton.Latitud;
                    o.Longitud = ton.Longitud;
                    o.Id = ton.Id;
                    dto.Add(o);
                }
            }

            return dto;
        }
        #endregion
        #region ObtenerAccionesEjecutadasMaquinas
        public List<ObjetoEjecucionesMaquinaDto> ObtenerAccionesEjecutadasMaquinas(int usuarioId)
        {
            List<ObjetoEjecucionesMaquinaDto> dto = new List<ObjetoEjecucionesMaquinaDto>();
            var empresT = _usuario.ObtenerEstablecimientosUsuario(usuarioId);
            foreach (var p in empresT)
            {
                var tokenss = _modelo.ObtenerTokensPlacaEmpresa(p.Establecimiento.EmpresaId.Value);
                foreach (var ton in tokenss)
                {
                    #region pin1 
                    for (int ia = 1; ia < 5; ia++)
                    {
                        var lectura = _modelo.ObtenerBitacoraLecturaTokenTiempo(ton.Token, ia);
                        // string accionEjecutada = "-1";
                        foreach (var lec in lectura)
                        {
                            ObjetoEjecucionesMaquinaDto o = new ObjetoEjecucionesMaquinaDto();

                            decimal minutosSegDec = (decimal)lec.PinDigital / (decimal)60;
                            var litrosMin = (Convert.ToDouble(minutosSegDec) * 0.75);
                            o.LitrosMin = litrosMin.ToString() + " litros";

                            o.FechaAccion = lec.FechaRegistro.Value.ToShortDateString() + " " + lec.FechaRegistro.Value.ToShortTimeString() + ":" + lec.FechaRegistro.Value.Second;
                            o.NombreTarea = lec.TareaEjecutada;
                            o.Nombre = lec.IdentificacionEquipo + "-" + lec.PinAnalogo.ToString();
                            o.Id = lec.Id;
                            dto.Add(o);


                        }
                    }

                    #endregion
                }

            }

            return dto;
        }
        #endregion
        #region ObtenerConfiguracionFrm
        public ObjetoComponentesFormDto ObtenerConfiguracionFrm(int id, int modeloId)
        {
            ObjetoComponentesFormDto dto = new ObjetoComponentesFormDto();

            var config = _modelo.ObtenerConfiguracionPlaca(id);

            if (config != null)
            {
                dto.Id = config.Id;
                dto.FechaInicio = config.FechaInicio.Value.ToShortDateString();
                dto.FechaFin = config.FechaFin.Value.ToShortDateString();
                dto.Nombre = config.Nombre;

                var configString = config.Configuracion.Split('|');

                dto.Valor1 = Int32.Parse(configString[1]);
                dto.Valor2 = Int32.Parse(configString[2]);
                dto.Valor3 = Int32.Parse(configString[3]);
                dto.Valor4 = Int32.Parse(configString[4]);
                dto.Iteracciones = Int32.Parse(configString[5]);
                dto.EjecutandoTarea = Int32.Parse(configString[7]);
                dto.SegundosAgua = Int32.Parse(configString[8]);
            }
            else
            {
                dto.Id = 0;
            }
            return dto;
        }
        #endregion
        #region CrearEditarTarea
        public TransaccionDto CrearEditarTarea(int tokenplacaId, int idConfig, string nombre, string fechaInicio, string fechaFin, string configuracion, int porcentajeOptimo, int porcentajeAccion)
        {
            TransaccionDto dto = new TransaccionDto();
            DateTime fechaIniD = DateTime.Now;
            DateTime fechaFinD = DateTime.Now;
            try
            {
                //fechaIniD = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null);
                fechaIniD = Convert.ToDateTime(fechaInicio);
                //fechaFinD = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null);
                fechaFinD = Convert.ToDateTime(fechaFin);
            }
            catch
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Formato Incorrecto";
                dto.IdNuevo = -2;
                return dto;
            }
            if (fechaIniD > fechaFinD)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "La fecha de Inicio no debe ser mayor a la fecha fin";
                dto.IdNuevo = -1;
                return dto;
            }

            var configs = _modelo.ObtenerConfiguracionesPlaca(tokenplacaId);


            DateTime fechaActual = DateTime.Now;
            var fechaActSinHora = fechaActual.ToShortDateString();
            DateTime fechaAValidar = Convert.ToDateTime(fechaActSinHora);

            foreach (var c in configs)
            {
                if (c.FechaInicio <= fechaAValidar && fechaAValidar <= c.FechaFin)
                {
                    _modelo.DejarONoVigenteConfiguracion(c.Id, true);

                }
                else
                {
                    //DEJAR NO VIGENTE LA CONFIGURACION
                    _modelo.DejarONoVigenteConfiguracion(c.Id, false);

                }
            }

            var tokenPl = _modelo.ObtenerTokenPlaca(tokenplacaId);
            if (tokenPl != null)
            {
                if (tokenPl.ValorSensor == null || tokenPl.ValorSensor.Value == -1 || tokenPl.PorcentajeCorrespondiente == null)
                {
                    dto.CodigoMensaje = CodigoMensaje.Error;
                    dto.Mensaje = "No puede planificar su cultivo, favor calibre su equipo para una correcta lectura.";
                    dto.IdNuevo = -5;
                    return dto;
                }
                //else
                //{
                //    var bita = _modelo.ObtenerBitacoraLectura(tokenPl.EmpresaId.Value).OrderByDescending(i => i.Id).FirstOrDefault(i => i.AccionEjecutada == "3");
                //    if (bita == null)
                //    {
                //        dto.CodigoMensaje = CodigoMensaje.Error;
                //        dto.Mensaje = "No puede planificar su cultivo, favor calibre su equipo para una correcta lectura.";
                //        dto.IdNuevo = -5;
                //        return dto;
                //    }
                //}
            }
            else
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "No existe token placa";
                dto.IdNuevo = -4;
                return dto;
            }

            ConfiguracionPlaca cP = new ConfiguracionPlaca();
            try
            {
                cP.Configuracion = configuracion;
                cP.FechaFin = fechaFinD;
                cP.FechaInicio = fechaIniD;
                cP.Nombre = nombre;
                cP.TokenPlacaId = tokenplacaId;
                cP.Vigente = true;
                cP.PorcentajeAccion = porcentajeAccion;
                cP.PorcentajeOptimo = porcentajeOptimo;
                if (idConfig == 0)
                {
                    var tareasPlaca = _modelo.ObtenerConfiguracionesPlaca(tokenplacaId);
                    foreach (var t in tareasPlaca)
                    {
                        if (fechaIniD >= t.FechaInicio.Value && fechaIniD <= t.FechaFin.Value)
                        {
                            dto.CodigoMensaje = CodigoMensaje.Error;
                            dto.Mensaje = "La fecha de Inicio ya contiene una tarea";
                            dto.IdNuevo = -1;
                            return dto;
                        }
                        if (fechaFinD <= t.FechaFin.Value && fechaFinD >= t.FechaInicio.Value)
                        {
                            dto.CodigoMensaje = CodigoMensaje.Error;
                            dto.Mensaje = "La fecha de Fin ya contiene una tarea";
                            dto.IdNuevo = -1;
                            return dto;
                        }
                    }
                    //NUEVO
                    _modelo.CrearTarea(cP);
                }
                else
                {
                    //UPDATE
                    var tareasPlaca = _modelo.ObtenerConfiguracionesPlaca(tokenplacaId).Where(c => c.Id != idConfig);
                    foreach (var t in tareasPlaca)
                    {
                        if (fechaIniD >= t.FechaInicio.Value && fechaIniD <= t.FechaFin.Value)
                        {
                            dto.CodigoMensaje = CodigoMensaje.Error;
                            dto.Mensaje = "La fecha de Inicio ya contiene una tarea";
                            dto.IdNuevo = -1;
                            return dto;
                        }
                        if (fechaFinD <= t.FechaFin.Value && fechaFinD >= t.FechaInicio.Value)
                        {
                            dto.CodigoMensaje = CodigoMensaje.Error;
                            dto.Mensaje = "La fecha de Fin ya contiene una tarea";
                            dto.IdNuevo = -1;
                            return dto;
                        }
                    }
                    cP.Id = idConfig;
                    _modelo.ModificarTarea(cP);
                }
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Existe una Excepción en el sistema, favor contacte con su administrador";
                dto.IdNuevo = -2;
            }
            return dto;
        }
        #endregion
        #region EliminarTarea
        public TransaccionDto EliminarTarea(int id)
        {
            TransaccionDto dto = new TransaccionDto();

            try
            {
                _modelo.EliminarTarea(id);
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";
                dto.IdNuevo = 1;
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Error en el sistema, favor contacte a su adminitrador";
                dto.IdNuevo = -1;
            }
            return dto;
        }
        #endregion
        #region EditarTokenPlacaCalibracion
        public TransaccionDto EditarTokenPlacaCalibracion(int tokenplacaId, int porcentaje, int valorSensor, int valorBajo)
        {
            TransaccionDto dto = new TransaccionDto();

            try
            {
                if (valorSensor == -2)
                {
                    //calibracion automatica
                    var tokenPlaca = _modelo.ObtenerTokenPlaca(tokenplacaId);
                    if (tokenPlaca != null)
                    {
                        var bita = _modelo.ObtenerBitacoraLecturaToken(tokenPlaca.Token).OrderBy(i => Convert.ToDouble(i.Valor)).First();
                        if (bita != null)
                        {
                            var varr = bita.Valor.Split('.');
                            varr = varr[0].Split(',');
                            int elCien = Convert.ToInt32(varr[0]);
                            _modelo.ModificarCalibrarTokenPlaca(tokenplacaId, porcentaje, Int32.Parse(varr[0]), valorBajo);

                            var conf = _modelo.ObtenerConfiguracionesPlaca(tokenplacaId);
                            int valorAccionF = 0;
                            int valorOptimoF = 0;

                            //0&674&>&614&4
                            string config = "0&1024&>&1024&4";
                            if (conf.Count() > 0)
                                _modelo.ModificarConfiguracion(config, tokenplacaId);
                        }
                        else
                        {
                            dto.CodigoMensaje = CodigoMensaje.Error;
                            dto.Mensaje = "En estos momentos no tiene ninguna lectura";
                            dto.IdNuevo = -4;
                        }
                    }
                    else
                    {
                        dto.CodigoMensaje = CodigoMensaje.Error;
                        dto.Mensaje = "Existe una Excepción en el sistema, su token no esta correcto";
                        dto.IdNuevo = -3;
                    }
                }
                else
                {
                    _modelo.ModificarCalibrarTokenPlaca(tokenplacaId, porcentaje, valorSensor, valorBajo);

                    var conf = _modelo.ObtenerConfiguracionesPlaca(tokenplacaId);
                    int valorAccionF = 0;
                    int valorOptimoF = 0;
                    foreach (var ra in conf)
                    {
                        //PRIMER VALOR
                        for (int a = 1; a <= 1024; a++)
                        {
                            //ir uno por uno para encontrar el valor optimo
                            double di = (double)a / (double)valorSensor;
                            var mul = di * 100;
                            var res = 100 - mul;
                            var sum = 100 + res;

                            int comparacion = (int)Math.Round(sum, MidpointRounding.AwayFromZero); // 3

                            if (comparacion == ra.PorcentajeAccion.Value)
                            {
                                valorAccionF = a;
                            }
                        }
                        for (int a = 1; a <= 1024; a++)
                        {
                            //ir uno por uno para encontrar el valor optimo
                            var di = (double)a / (double)valorSensor;
                            var mul = di * 100;
                            var res = 100 - mul;
                            var sum = 100 + res;

                            int comparacion = (int)Math.Round(sum, MidpointRounding.AwayFromZero); // 3

                            if (comparacion == ra.PorcentajeOptimo)
                            {
                                valorOptimoF = a;
                            }
                        }
                    }
                    //0&674&>&614&4
                    string config = "0&1024&>&1024&4";
                    if (conf.Count() > 0)
                        _modelo.ModificarConfiguracion(config, tokenplacaId);
                }
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Existe una Excepción en el sistema, favor contacte con su administrador";
                dto.IdNuevo = -2;
            }
            return dto;
        }
        #endregion
        #region CrearBitacoraLecturaAccion
        public void CrearBitacoraLecturaAccion(string data)
        {
            //token;456#12;31|;13;0|1;0|;2;0 => 'token';token#valor1;accion1|valor2;accion2|valor3;accion3|valor4;accion4

            if (data.Length > 0)
            {
                var dataT = data.Split('#');
                var datosToken = dataT[0].Split(';');
                //TOKEN
                string tokenHead = datosToken[1];
                var tokenPlaca = _modelo.ObtenerTokenPlaca(tokenHead);
                var dataMacro = dataT[1].Split('|');

                int pin = 1;
                foreach (var d in dataMacro)
                {
                    var datos = d.Split(';');

                    string pinAnalogo = pin.ToString();
                    string valor = datos[0];
                    string pinDigital = pin.ToString();
                    string accion = datos[1];
                    DateTime fecha = DateTime.Now;
                    pin++;

                    BitacoraLecturasAcciones obj = new BitacoraLecturasAcciones();

                    obj.AccionEjecutada = accion;
                    obj.EmpresaId = _usuario.ObtenerEstablecimientosUsuario(tokenPlaca.UsuarioId.Value).First().Establecimiento.EmpresaId;
                    obj.FechaRegistro = fecha;
                    obj.FechaRegistroUnix = (long)DateTimeToUnixTimestamp(fecha);
                    obj.IdentificacionEquipo = tokenPlaca.Identificacion;
                    obj.Latitud = tokenPlaca.Latitud;
                    obj.Longitud = tokenPlaca.Longitud;
                    obj.ModeloEquipo = tokenPlaca.Modelo.Nombre;
                    obj.PinAnalogo = Int32.Parse(pinAnalogo);
                    obj.PinDigital = Int32.Parse(accion);
                    var tarea = tokenPlaca.ConfiguracionPlaca.FirstOrDefault(c => c.FechaInicio <= fecha.Date && c.FechaFin >= fecha.Date);
                    if (tarea != null)
                        obj.TareaEjecutada = tarea.Nombre;
                    obj.Token = tokenHead;
                    obj.UsuarioId = tokenPlaca.UsuarioId;
                    obj.Valor = valor;

                    int valorSensor = Int32.Parse(valor);
                    if (valorSensor < 1001)
                        _modelo.CrearBitacoraLecturaAccion(obj);
                }
            }
        }
        #endregion
        #region ObtenerIndicadorLectura
        public string ObtenerIndicadorLectura(int idUsuario, int dia, int mes, int anio)
        {
            //int empresaId = _usuario.ObtenerEstablecimientosUsuario(idUsuario).First().Establecimiento.EmpresaId.Value;

            var empresT = _usuario.ObtenerEstablecimientosUsuario(idUsuario);
            var lecturas = "[";
            int flag = 0;
            int flag2 = 0;
            int flag3 = 0;
            int flag4 = 0;
            foreach (var pp in empresT)
            {
                var placas = _modelo.ObtenerTokensPlacaEmpresa(pp.Establecimiento.EmpresaId.Value);

                foreach (var p in placas)
                {
                    if (p.ValorSensor != null && p.PorcentajeCorrespondiente != null)
                    {
                        if (p.ConfiguracionPlaca != null)
                        {
                            var existeVigente = p.ConfiguracionPlaca.FirstOrDefault(ass => ass.Vigente == true);

                            if (existeVigente != null)
                            {
                                #region sensor1
                                var lecLocal = "{name: '" + p.Identificacion + " 1',type: 'line',pointInterval: 300000,";
                                var nSubida = "";
                                var placa = from mtra in _modelo.ObtenerBitacoraLecturaTokenTarea(p.Token, existeVigente.Nombre, dia, mes, anio)
                                            where mtra.Token == p.Token && mtra.PinAnalogo == 1
                                            orderby mtra.FechaRegistroUnix ascending
                                            select mtra;
                                foreach (var m in placa)
                                {
                                    string fecha = m.FechaRegistro.Value.ToString("dd/M/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                                    int anis = m.FechaRegistro.Value.Year;
                                    int mess = m.FechaRegistro.Value.Month;
                                    int daya = m.FechaRegistro.Value.Day;
                                    int hora = m.FechaRegistro.Value.Hour;
                                    int minuto = m.FechaRegistro.Value.Minute;

                                    var dateTime = new DateTime(anis, mess, daya, hora, minuto, 0, DateTimeKind.Local);
                                    var fecha2 = dateTime.AddHours(-3);
                                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    var unixDateTime = (fecha2.ToUniversalTime() - epoch).TotalSeconds;


                                    //if (p.PorcentajeCorrespondiente.Value == 0)
                                    //    p.PorcentajeCorrespondiente = 1;
                                    //int valorPorcentaje100 = (100 / p.PorcentajeCorrespondiente.Value) * p.ValorSensor.Value;


                                    int lecturaInt = Int32.Parse(m.Valor);
                                    //999 100
                                    //134  > valor sensor
                                    //865  >difernecia entre 999 - valor sensor
                                    //100  
                                    //965  >se le agrega el margen que no se medira, 100 al 999; es decir se suman 100
                                    //entoncs seria 965*100/999 =====>   96,5965966
                                    int valorParaCalculo = (999 - lecturaInt) + 100;
                                    // int valorParaCalculo = 899 - lecturaInt;
                                    double valorCalculoFloat = Convert.ToDouble(valorParaCalculo);
                                    double porcentaje = (valorCalculoFloat * (double)100) / (double)999;
                                    var valo = porcentaje.ToString().Split(',');
                                    int porcentajeInt = Int32.Parse(valo[0]);
                                    //o.Porcentajes = porcentajeInt.ToString() + "%";
                                    //var valorSplit = m.Valor.Split('.');
                                    //var totalS = Convert.ToDouble(valorSplit[0]);

                                    var timeSpan = TimeSpan.FromSeconds(unixDateTime);
                                    var localDateTime = new DateTime(timeSpan.Ticks).ToLocalTime();
                                    if (nSubida.Length == 0)
                                    {
                                        nSubida = "[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                    else
                                    {
                                        nSubida += ",[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                }

                                lecLocal += "data: [" + nSubida + "]}";

                                if (flag == 0)
                                {
                                    flag++;
                                    lecturas += lecLocal;
                                }
                                else
                                {
                                    lecturas += "," + lecLocal;
                                }
                                #endregion

                                #region sensor2
                                lecLocal = "{name: '" + p.Identificacion + " 2',type: 'line',pointInterval: 300000,";
                                var nSubida2 = "";
                                var placa2 = from mtra in _modelo.ObtenerBitacoraLecturaTokenTarea(p.Token, existeVigente.Nombre, dia, mes, anio)
                                             where mtra.Token == p.Token && mtra.PinAnalogo == 2
                                             orderby mtra.FechaRegistroUnix ascending
                                             select mtra;
                                foreach (var m in placa2)
                                {
                                    string fecha = m.FechaRegistro.Value.ToString("dd/M/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                                    int anis = m.FechaRegistro.Value.Year;
                                    int mess = m.FechaRegistro.Value.Month;
                                    int daya = m.FechaRegistro.Value.Day;
                                    int hora = m.FechaRegistro.Value.Hour;
                                    int minuto = m.FechaRegistro.Value.Minute;

                                    var dateTime = new DateTime(anis, mess, daya, hora, minuto, 0, DateTimeKind.Local);
                                    var fecha2 = dateTime.AddHours(-3);
                                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    var unixDateTime = (fecha2.ToUniversalTime() - epoch).TotalSeconds;

                                    //if (p.PorcentajeCorrespondiente.Value == 0)
                                    //    p.PorcentajeCorrespondiente = 1;
                                    //int valorPorcentaje100 = (100 / p.PorcentajeCorrespondiente.Value) * p.ValorSensor.Value;

                                    //var valorSplit = m.Valor.Split('.');
                                    //var totalS = Convert.ToDouble(valorSplit[0]);
                                    int lecturaInt = Int32.Parse(m.Valor);
                                    //999 100
                                    //134  > valor sensor
                                    //865  >difernecia entre 999 - valor sensor
                                    //100  
                                    //965  >se le agrega el margen que no se medira, 100 al 999; es decir se suman 100
                                    //entoncs seria 965*100/999 =====>   96,5965966
                                    int valorParaCalculo = (999 - lecturaInt) + 100;
                                    // int valorParaCalculo = 899 - lecturaInt;
                                    double valorCalculoFloat = Convert.ToDouble(valorParaCalculo);
                                    double porcentaje = (valorCalculoFloat * (double)100) / (double)999;
                                    var valo = porcentaje.ToString().Split(',');
                                    int porcentajeInt = Int32.Parse(valo[0]);
                                    //o.Porcentajes = porcentajeInt.ToString() + "%";
                                    //var valorSplit = m.Valor.Split('.');
                                    //var totalS = Convert.ToDouble(valorSplit[0]);

                                    var timeSpan = TimeSpan.FromSeconds(unixDateTime);
                                    var localDateTime = new DateTime(timeSpan.Ticks).ToLocalTime();
                                    if (nSubida2.Length == 0)
                                    {
                                        nSubida2 = "[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                    else
                                    {
                                        nSubida2 += ",[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                }

                                lecLocal += "data: [" + nSubida2 + "]}";

                                if (flag == 0)
                                {
                                    flag++;
                                    lecturas += lecLocal;
                                }
                                else
                                {
                                    lecturas += "," + lecLocal;
                                }
                                #endregion
                                #region sensor3
                                lecLocal = "{name: '" + p.Identificacion + " 3',type: 'line',pointInterval: 300000,";
                                var nSubida3 = "";
                                var placa3 = from mtra in _modelo.ObtenerBitacoraLecturaTokenTarea(p.Token, existeVigente.Nombre, dia, mes, anio)
                                             where mtra.Token == p.Token && mtra.PinAnalogo == 3
                                             orderby mtra.FechaRegistroUnix ascending
                                             select mtra;
                                foreach (var m in placa3)
                                {
                                    string fecha = m.FechaRegistro.Value.ToString("dd/M/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                                    int anis = m.FechaRegistro.Value.Year;
                                    int mess = m.FechaRegistro.Value.Month;
                                    int daya = m.FechaRegistro.Value.Day;
                                    int hora = m.FechaRegistro.Value.Hour;
                                    int minuto = m.FechaRegistro.Value.Minute;

                                    var dateTime = new DateTime(anis, mess, daya, hora, minuto, 0, DateTimeKind.Local);
                                    var fecha2 = dateTime.AddHours(-3);
                                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    var unixDateTime = (fecha2.ToUniversalTime() - epoch).TotalSeconds;

                                    //if (p.PorcentajeCorrespondiente.Value == 0)
                                    //    p.PorcentajeCorrespondiente = 1;
                                    //int valorPorcentaje100 = (100 / p.PorcentajeCorrespondiente.Value) * p.ValorSensor.Value;

                                    //var valorSplit = m.Valor.Split('.');
                                    //var totalS = Convert.ToDouble(valorSplit[0]);
                                    int lecturaInt = Int32.Parse(m.Valor);
                                    //999 100
                                    //134  > valor sensor
                                    //865  >difernecia entre 999 - valor sensor
                                    //100  
                                    //965  >se le agrega el margen que no se medira, 100 al 999; es decir se suman 100
                                    //entoncs seria 965*100/999 =====>   96,5965966
                                    int valorParaCalculo = (999 - lecturaInt) + 100;
                                    // int valorParaCalculo = 899 - lecturaInt;
                                    double valorCalculoFloat = Convert.ToDouble(valorParaCalculo);
                                    double porcentaje = (valorCalculoFloat * (double)100) / (double)999;
                                    var valo = porcentaje.ToString().Split(',');
                                    int porcentajeInt = Int32.Parse(valo[0]);
                                    //o.Porcentajes = porcentajeInt.ToString() + "%";
                                    //var valorSplit = m.Valor.Split('.');
                                    //var totalS = Convert.ToDouble(valorSplit[0]);

                                    var timeSpan = TimeSpan.FromSeconds(unixDateTime);
                                    var localDateTime = new DateTime(timeSpan.Ticks).ToLocalTime();
                                    if (nSubida3.Length == 0)
                                    {
                                        nSubida3 = "[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                    else
                                    {
                                        nSubida3 += ",[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                }

                                lecLocal += "data: [" + nSubida3 + "]}";

                                if (flag == 0)
                                {
                                    flag++;
                                    lecturas += lecLocal;
                                }
                                else
                                {
                                    lecturas += "," + lecLocal;
                                }
                                #endregion
                                #region sensor4
                                lecLocal = "{name: '" + p.Identificacion + " 4',type: 'line',pointInterval: 300000,";
                                var nSubida4 = "";
                                var placa4 = from mtra in _modelo.ObtenerBitacoraLecturaTokenTarea(p.Token, existeVigente.Nombre, dia, mes, anio)
                                             where mtra.Token == p.Token && mtra.PinAnalogo == 4
                                             orderby mtra.FechaRegistroUnix ascending
                                             select mtra;
                                foreach (var m in placa4)
                                {
                                    string fecha = m.FechaRegistro.Value.ToString("dd/M/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                                    int anis = m.FechaRegistro.Value.Year;
                                    int mess = m.FechaRegistro.Value.Month;
                                    int daya = m.FechaRegistro.Value.Day;
                                    int hora = m.FechaRegistro.Value.Hour;
                                    int minuto = m.FechaRegistro.Value.Minute;

                                    var dateTime = new DateTime(anis, mess, daya, hora, minuto, 0, DateTimeKind.Local);
                                    var fecha2 = dateTime.AddHours(-3);
                                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    var unixDateTime = (fecha2.ToUniversalTime() - epoch).TotalSeconds;

                                    //if (p.PorcentajeCorrespondiente.Value == 0)
                                    //    p.PorcentajeCorrespondiente = 1;
                                    //int valorPorcentaje100 = (100 / p.PorcentajeCorrespondiente.Value) * p.ValorSensor.Value;

                                    //var valorSplit = m.Valor.Split('.');
                                    //var totalS = Convert.ToDouble(valorSplit[0]);

                                    int lecturaInt = Int32.Parse(m.Valor);
                                    //999 100
                                    //134  > valor sensor
                                    //865  >difernecia entre 999 - valor sensor
                                    //100  
                                    //965  >se le agrega el margen que no se medira, 100 al 999; es decir se suman 100
                                    //entoncs seria 965*100/999 =====>   96,5965966
                                    int valorParaCalculo = (999 - lecturaInt) + 100;
                                    // int valorParaCalculo = 899 - lecturaInt;
                                    double valorCalculoFloat = Convert.ToDouble(valorParaCalculo);
                                    double porcentaje = (valorCalculoFloat * (double)100) / (double)999;
                                    var valo = porcentaje.ToString().Split(',');
                                    int porcentajeInt = Int32.Parse(valo[0]);
                                    //o.Porcentajes = porcentajeInt.ToString() + "%";
                                    //var valorSplit = m.Valor.Split('.');
                                    //var totalS = Convert.ToDouble(valorSplit[0]);

                                    var timeSpan = TimeSpan.FromSeconds(unixDateTime);
                                    var localDateTime = new DateTime(timeSpan.Ticks).ToLocalTime();
                                    if (nSubida4.Length == 0)
                                    {
                                        nSubida4 = "[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                    else
                                    {
                                        nSubida4 += ",[" + unixDateTime + "000," + porcentajeInt.ToString() + "]";
                                    }
                                }

                                lecLocal += "data: [" + nSubida4 + "]}";

                                if (flag == 0)
                                {
                                    flag++;
                                    lecturas += lecLocal;
                                }
                                else
                                {
                                    lecturas += "," + lecLocal;
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            lecturas += "]";
            return lecturas;
            // return "[{name: 'Nacional Subida',type: 'line',pointInterval: 300000,data: [" + nSubida + "]}, {name: 'Nacional Bajada',type: 'line',pointInterval: 300000,data: [" + nBajada + "]}, {name: 'Internacional Subida',type: 'line',pointInterval: 300000,data: [" + iSubida + "]}, {name: 'Internacional Bajada',type: 'line',pointInterval: 300000,data: [" + iBajada + "]}]";
        }
        #endregion
        #region ObtenerValorSensorCalibracion
        public string ObtenerValorSensorCalibracion(int idToken)
        {
            var tokenPlaca = _modelo.ObtenerTokenPlaca(idToken);
            if (tokenPlaca != null)
            {
                var bita = _modelo.ObtenerBitacoraLecturaToken(tokenPlaca.Token).OrderByDescending(i => i.Id).FirstOrDefault();
                if (bita != null)
                    return bita.Valor.ToString() + "&" + bita.FechaRegistro.Value.ToShortDateString() + " " + bita.FechaRegistro.Value.ToShortTimeString();
                else
                    return "";
            }

            return "";
        }
        #endregion
        #region ObtenerPanelControl
        public ObjetoPanelControlDto ObtenerPanelControl(int tokenId, int usuarioId)
        {
            ObjetoPanelControlDto dto = new ObjetoPanelControlDto();
            var tok = _modelo.ObtenerTokenPlaca(tokenId);
            if (tok != null)
            {
                var data = _modelo.ObtenerPanelControl(tok.Token, usuarioId);

                if (data != null)
                {
                    dto.Id = data.Id;
                    //dto.AguaDadaMas10Min = data.AguaDadaMas10Min.Value;
                    dto.AguaDadaMas3Min = data.AguaDadaMas3Min.Value;
                    //dto.AguaDadaMas5Min = data.AguaDadaMas5Min.Value;
                    dto.Descripcion = data.Descripcion;
                    dto.EnvioCorreo = data.EnvioCorreo.Value;
                    dto.EnvioCorreoInforme1Hora = data.EnvioCorreoInforme1Hora.Value;
                    dto.Nombre = data.Nombre;
                    dto.SinTareaEjecutando = data.SinTareaEjecutando.Value;
                    dto.Vigente = data.Vigente.Value;
                }
                else
                {
                    dto.Id = 0;
                }
            }
            else
            {
                dto.Id = 0;
            }
            return dto;
        }
        #endregion
        #region CrearEditarPanelControl
        public TransaccionDto CrearEditarPanelControl(int usuarioId, int tokenId, int panelId, bool correo, bool tarea, bool minutos, bool informe)
        {
            TransaccionDto dto = new TransaccionDto();

            try
            {
                var d = _modelo.ObtenerTokenPlaca(tokenId);

                PanelControl obj = new PanelControl();

                obj.AguaDadaMas3Min = minutos;
                obj.EnvioCorreo = correo;
                obj.EnvioCorreoInforme1Hora = informe;
                obj.SinTareaEjecutando = tarea;
                obj.Vigente = true;
                obj.Id = panelId;
                obj.UsuarioId = usuarioId;
                obj.Token = d.Token;

                if (panelId == 0)
                {
                    //NUEVO PANEL
                    obj.SinTareaEjecutadoCorreo = false;
                    _modelo.CrearPanel(obj);
                }
                else
                {
                    _modelo.ModificarPanel(obj);
                }
                dto.CodigoMensaje = CodigoMensaje.Ok;
                dto.Mensaje = "OK";
                dto.IdNuevo = obj.Id;
            }
            catch (Exception ex)
            {
                dto.CodigoMensaje = CodigoMensaje.Error;
                dto.Mensaje = "Existe una Excepción en el sistema, favor contacte con su administrador";
                dto.IdNuevo = -2;
            }
            return dto;
        }
        #endregion
        #region EjecutarPanelControl
        public void EjecutarPanelControl()
        {

            var paneles = _modelo.ObtenerPanelesControl();

            foreach (var p in paneles)
            {
                var usuario = _usuario.ObtenerUsuario(p.UsuarioId.Value);
                var tokenPlaca = _modelo.ObtenerTokenPlaca(p.Token);
                if (usuario != null)
                {
                    var hoy = DateTimeToUnixTimestamp(DateTime.Now);
                    //VER SI LE CORRESPONDE IR
                    var eject = _modelo.ObtenerEjecucionTokenEmail(tokenPlaca.Token);
                    EjecucionTokenEmail ejecTo = new EjecucionTokenEmail();
                    ejecTo.UnixTimeEjecucionEmail = (long)hoy;
                    ejecTo.Token = tokenPlaca.Token;

                    if (eject == null)
                    {
                        _modelo.CrearEjecucionToken(ejecTo);
                    }
                    else
                    {
                        var diferenciaTok = hoy - eject.UnixTimeEjecucionEmail.Value;

                        if (diferenciaTok > 300)
                        {
                            _modelo.ModificarEjecucionToken(ejecTo);
                            //PROCESAR
                            #region Agua 3 Min
                            if (p.AguaDadaMas3Min.Value)
                            {
                                var bita = _modelo.ObtenerBitacoraLecturaToken(tokenPlaca.Token).OrderByDescending(i => i.Id).FirstOrDefault();
                                if (bita != null)
                                {
                                    var diferencia = hoy - bita.FechaRegistroUnix.Value;

                                    if (bita.AccionEjecutada == "1")
                                    {
                                        if (diferencia > 180)
                                        {
                                            //  SON MAS DE 3 MINUTOS
                                            //ENVIO DE NOTIFICACION
                                            string nombrePersona2 = p.Usuario.NombrePersona + " " + p.Usuario.ApellidosPersona;
                                            EnvioCorreo(nombrePersona2, p.Usuario.EmailPersona, "E-Mapu: Notificación Consumo de Agua", 1, tokenPlaca.Identificacion, tokenPlaca.Token);
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Sin Tarea
                            if (p.SinTareaEjecutando.Value)
                            {
                                var config = _modelo.ObtenerConfiguracionesPlaca(tokenPlaca.Id).Where(t => t.Vigente == true);
                                if (config.Count() == 0)
                                {
                                    //NO  HAY NADA VIGENTE
                                    //ENVIO DE NOTIFICACION
                                    string nombrePersona2 = p.Usuario.NombrePersona + " " + p.Usuario.ApellidosPersona;
                                    EnvioCorreo(nombrePersona2, p.Usuario.EmailPersona, "E-Mapu: Notificación Sin Tareas", 2, tokenPlaca.Identificacion, tokenPlaca.Token);
                                }
                            }
                            #endregion
                            #region Informe 1 hora
                            if (p.EnvioCorreoInforme1Hora.Value)
                            {
                                if (p.UnixTimeEnvioInforme != null)
                                {
                                    var diferenciaInf = hoy - p.UnixTimeEnvioInforme.Value;

                                    if (diferenciaInf > 3599)
                                    {
                                        //1 HORA
                                        string nombrePersona2 = p.Usuario.NombrePersona + " " + p.Usuario.ApellidosPersona;
                                        EnvioCorreo(nombrePersona2, p.Usuario.EmailPersona, "E-Mapu: Informe", 3, tokenPlaca.Identificacion, tokenPlaca.Token);

                                        _modelo.ModificarPanel(p.Id, (long)hoy);

                                    }
                                }
                                else
                                {
                                    _modelo.ModificarPanel(p.Id, (long)hoy);
                                }
                            }
                            #endregion
                        }
                    }

                }
            }
        }
        #endregion
        #region EnvioCorreo
        public void EnvioCorreo(string nombrePersona, string emailBd, string asunto, int que, string nombreMaquina, string token)
        {
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(emailBd));
            email.From = new MailAddress("informacion@ssoft.cl");
            email.Subject = asunto;
            email.Body = ObtenerInformeHtml(que, nombrePersona, nombreMaquina, token);
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.ssoft.cl";
            smtp.Port = 25;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("informacion@ssoft.cl", "cerro69pedro");
            try
            {
                smtp.Send(email);
                email.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region ObtenerInformeHtml
        public string ObtenerInformeHtml(int que, string nombrePersona, string nombreMaquina, string token)
        {
            if (que == 1)
            {
                //AGUA 3 MIN
                var htmls = "<p>Estimad@ " + nombrePersona + "</p>";
                htmls += "<p>Actualmente su maquina con nombre <b>" + nombreMaquina + "</b> tiene accionado el paso de agua por mas de 3 minutos, se recomienda revisar el lugar de instalación.</p>";
                htmls += "<p>Saludos</p>";
                htmls += "</br>";
                htmls += "<p>Atentamente</p>";
                htmls += "<p><b>Equipo E-Mapu</b></p>";

                return htmls;
            }
            if (que == 2)
            {
                //SIN TAREA
                var htmls = "<p>Estimad@ " + nombrePersona + "</p>";
                htmls += "<p>Actualmente su maquina con nombre <b>" + nombreMaquina + "</b> se encuentra sin tareas por ejecutar.</p>";
                htmls += "<p>Saludos</p>";
                htmls += "</br>";
                htmls += "<p>Atentamente</p>";
                htmls += "<p><b>Equipo E-Mapu</b></p>";

                return htmls;
            }
            if (que == 3)
            {
                //INFORME
                var htmls = "<p>Estimad@ " + nombrePersona + "</p>";
                htmls += "<p>Actualmente su maquina con nombre <b>" + nombreMaquina + "</b> dispone de la siguiente información:</p>";
                htmls += "</br>";
                htmls += "<table><tr>";
                htmls += "<td>Nivel humedad: </td><td><b>" + ObtenerNivelActual(token) + "</b></td>";
                htmls += "</tr>";
                htmls += "<tr>";
                htmls += "<td>Tiempo agua accionada: </td><td><b>" + CalcularTiempo(ObtenerSegundosEjecutados(token)) + " Horas</b></td>";
                htmls += "</tr>";
                htmls += "</table>";
                htmls += "<p>Saludos</p>";
                htmls += "</br>";
                htmls += "<p>Atentamente</p>";
                htmls += "<p><b>Equipo E-Mapu</b></p>";

                return htmls;
            }
            return "";
        }
        #endregion
        #region ObtenerSegundosEjecutados
        public int ObtenerSegundosEjecutados(string token)
        {
            var lectura = _modelo.ObtenerBitacoraLecturaToken(token);
            string accionEjecutada = "-1";
            long unixEncendido = 0;
            long unixApagado = 0;
            long diferenciaEncendidoApagado = 0;
            long sumaTotalToken = 0;
            foreach (var lec in lectura)
            {
                ObjetoEjecucionesMaquinaDto o = new ObjetoEjecucionesMaquinaDto();
                if (accionEjecutada != lec.AccionEjecutada)
                {
                    if (lec.AccionEjecutada == "1")
                    {
                        //ENCENDIDO
                        o.Accion = "Encendido";
                        unixEncendido = lec.FechaRegistroUnix.Value;
                    }
                    if (lec.AccionEjecutada == "0")
                    {
                        //APAGADO
                        o.Accion = "Apagado";
                        unixApagado = lec.FechaRegistroUnix.Value;
                    }
                    if (unixApagado > 0 && unixEncendido > 0)
                        diferenciaEncendidoApagado = unixApagado - unixEncendido;
                    //sumaTotalToken = sumaTotalToken + diferenciaEncendidoApagado;
                }

                accionEjecutada = lec.AccionEjecutada;
            }

            return (int)diferenciaEncendidoApagado;
        }
        #endregion
        #region ObtenerNivelActual
        public string ObtenerNivelActual(string token)
        {
            var bita = _modelo.ObtenerBitacoraLecturaToken(token).OrderByDescending(i => i.Id).FirstOrDefault();
            string retorno = "";
            if (bita != null)
            {
                var tokenPlaca = _modelo.ObtenerTokenPlaca(token);
                int valor = Int32.Parse(bita.Valor);
                string circulo = "";
                string texto = "";
                #region NEGOCIO DE RANGO NIVELES DE HUMEDAD

                //CONFIGURACION CAPACITIVO

                decimal flo = tokenPlaca.ValorSensor.Value / 5;//SENSOR CAPACITIVO EN EL AIRE
                decimal roundedB = Math.Round(flo, 0, MidpointRounding.AwayFromZero); // Output: 2
                int x = Convert.ToInt32(roundedB);

                //CONFIGURACION CAPACITIVO
                if (valor >= 0 && valor <= x)
                {
                    circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #006B41;cursor: pointer;\"> </div>";
                    texto = "Muy humedo";
                }
                int humedo = x + x;
                if (valor > x && valor <= humedo)
                {
                    circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #00A052;cursor: pointer;\"> </div>";
                    texto = "Humedo";
                }
                int medioHumedo = humedo + x;
                if (valor > humedo && valor <= medioHumedo)
                {
                    circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #E2E54E;cursor: pointer;\"> </div>";
                    texto = "Medio humedo";
                }
                int medioSeco = medioHumedo + x;
                if (valor > medioHumedo && valor <= medioSeco)
                {
                    circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #F48432;cursor: pointer;\"> </div>";
                    texto = "Medio seco";
                }
                var seco = medioSeco + x;
                if (valor > medioSeco && valor <= 1024)
                {
                    circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #C5231E;cursor: pointer;\"> </div>";
                    texto = "Seco";
                }


                //////////if (tokenPlaca.ValorSensor >= 300)
                //////////{
                //////////    //CONFIGURACION TITAN
                //////////        if (valor >= 0 && valor < 382)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #006B41;cursor: pointer;\"> </div>";
                //////////        texto = "Muy humedo";
                //////////    }
                //////////    if (valor > 381 && valor <= 460)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #00A052;cursor: pointer;\"> </div>";
                //////////        texto = "Humedo";
                //////////    }
                //////////    if (valor > 461 && valor <= 549)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #E2E54E;cursor: pointer;\"> </div>";
                //////////        texto = "Medio humedo";
                //////////    }
                //////////    if (valor > 550 && valor <= 674)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #F48432;cursor: pointer;\"> </div>";
                //////////        texto = "Medio seco";
                //////////    }
                //////////    if (valor > 675 && valor <= 1024)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #C5231E;cursor: pointer;\"> </div>";
                //////////        texto = "Seco";
                //////////    }
                //////////}

                //////////if (tokenPlaca.ValorSensor <= 200)
                //////////{
                //////////    //CONFIGURACION EUROPA
                //////////    if (valor > 0 && valor <= 329)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #006B41;cursor: pointer;\"> </div>";
                //////////        texto = "Muy humedo";
                //////////    }
                //////////    if (valor > 329 && valor <= 529)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #00A052;cursor: pointer;\"> </div>";
                //////////        texto = "Humedo";
                //////////    }
                //////////    if (valor > 530 && valor <= 730)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #E2E54E;cursor: pointer;\"> </div>";
                //////////        texto = "Medio humedo";
                //////////    }
                //////////    if (valor > 731 && valor <= 931)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #F48432;cursor: pointer;\"> </div>";
                //////////        texto = "Medio seco";
                //////////    }
                //////////    if (valor > 932 && valor <= 1025)
                //////////    {
                //////////        circulo = "<div style=\"margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius:50%;border-radius: 50%;background: #C5231E;cursor: pointer;\"> </div>";
                //////////        texto = "Seco";
                //////////    }
                //////////}
                #endregion

                retorno = "<table><tr><td> Es de: " + valor + "</td><td>" + circulo + "</td><td>" + texto + "</td></tr></table>";
            }
            return retorno;
        }
        #endregion
        #region ToEpochTimeSeconds
        public long ToEpochTimeSeconds(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }
        #endregion
        #region DateTimeToUnixTimestamp
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }
        #endregion
        #region UTC
        public string Utc(string isoTime)
        {
            var utc = isoTime.Replace(':', ',');
            utc = utc.Replace('/', ',');
            var sep = utc.Split(' ');
            var fecha = sep[0] + "," + sep[1];
            return fecha;
        }
        #endregion
        #region UTC YMD
        public string UtcYmd(string isoTime)
        {
            var utc = isoTime.Replace(':', ',');
            utc = utc.Replace('/', ',');
            var sep = utc.Split(' ');
            var x = sep[0].Split(',');
            int menos = Int32.Parse(x[1]) - 1;

            var ymd = x[2] + ',' + menos.ToString() + ',' + x[0];
            var fecha = ymd + "," + sep[1];
            return fecha;
        }
        #endregion
        #region UTC MDY
        public string UtcMdy(string isoTime)
        {
            var utc = isoTime.Replace(':', ',');
            utc = utc.Replace('/', ',');
            var sep = utc.Split(' ');
            var x = sep[0].Split(',');
            int menos = Int32.Parse(x[1]) - 1;

            var mdy = menos.ToString() + ',' + x[2] + ',' + x[0];
            var fecha = mdy + "," + sep[1];
            return fecha;
        }
        #endregion
        #region CalcularTiempo
        public string CalcularTiempo(Int32 tsegundos)
        {
            Int32 horas = (tsegundos / 3600);
            Int32 minutos = ((tsegundos - horas * 3600) / 60);
            Int32 segundos = tsegundos - (horas * 3600 + minutos * 60);
            return horas.ToString() + ":" + minutos.ToString() + ":" + segundos.ToString();
        }
        #endregion
        #endregion
    }
}

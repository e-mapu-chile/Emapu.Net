using SSoft.Aplicacion.Dto.Base;
using SSoft.Aplicacion.Servicios;
using SSoft.Transversal.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace SSoft.IoT.MVC.Controllers
{
    public class CultivoApiController : ApiController
    {
        ServicioCultivoOnline _servicioCultivo = new ServicioCultivoOnline();

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //[Route("api/protocolo/ObtenerProtocolo")]
        public string ObtenerConfiguracion(string token)
        {
            DateTime d = DateTime.Today;
            string fechaHoy = d.Date.Year + "/" + d.Date.Month + "/" + d.Date.Day;
            string CS = "Server=190.114.254.220;Database=emapu;User Id=sa;Password=Desarrollo2012";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand command = new SqlCommand("SP_ObtenerPlanFechaHoy", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@tokenAparato", SqlDbType.VarChar).Value = token;
            command.Parameters.Add("@fechaHoy", SqlDbType.VarChar).Value = fechaHoy;

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet("Tabla");
            da.Fill(ds, "NuevaTabla");
            var data = ds;


            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow dr in table.Rows)
                {

                    return "[" + dr[0].ToString() + "]";
                }
            }

            return "";


        }

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //[Route("api/protocolo/ObtenerProtocolo")]
        public string SetValueCalibracion(string token, string valorCalibrado)
        {
            string CS = "Server=190.114.254.220;Database=emapu;User Id=sa;Password=Desarrollo2012";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand command = new SqlCommand("SP_ActualizarValorCalibrado", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@TokenAparato", SqlDbType.VarChar).Value = token;
            command.Parameters.Add("@ValorCalibrado", SqlDbType.VarChar).Value = valorCalibrado;

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet("Tabla");
            da.Fill(ds, "NuevaTabla");
            var data = ds;


            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow dr in table.Rows)
                {

                    return dr[0].ToString();
                }
            }

            return "";


        }


        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //[Route("api/protocolo/ObtenerProtocolo")]
        public string GuardarLecturaAccion(string dataIn)
        {
            var dataString = dataIn.Split('|');

            string promedioTierra = dataString[0];
            int temperatura = Convert.ToInt32(dataString[1]);
            int humedad = Convert.ToInt32(dataString[2]);
            int accion = Convert.ToInt32(dataString[3]);
            string token = dataString[4];

            if (accion == 1)
            {
                string CS = "Server=190.114.254.220;Database=emapu;User Id=sa;Password=Desarrollo2012";
                SqlConnection con = new SqlConnection(CS);
                SqlCommand command = new SqlCommand("SP_InsertarBitacoraTierra", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ValorTierra", SqlDbType.VarChar).Value = promedioTierra;
                command.Parameters.Add("@ValorTemp", SqlDbType.Int).Value = temperatura;
                command.Parameters.Add("@ValorHum", SqlDbType.Int).Value = humedad;
                command.Parameters.Add("@TokenAparato", SqlDbType.VarChar).Value = token;

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("Tabla");
                da.Fill(ds, "NuevaTabla");
                var data = ds;


                //foreach (DataTable table in ds.Tables)
                //{
                //    foreach (DataRow dr in table.Rows)
                //    {

                //        return dr[0].ToString();
                //    }
                //}


            }

            return "";

        }




    }
}

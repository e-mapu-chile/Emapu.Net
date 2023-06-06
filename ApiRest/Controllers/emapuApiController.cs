using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiRest.Controllers
{
    public class emapuApiController : ApiController
    {

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public string ObtenerConfiguracion(string token)
        {
            string sUrlRequest = "https://pssoft.cl/emapu/getPlan?tokenAparato=" + token + "";
            var json = new WebClient().DownloadString(sUrlRequest);
            return json;
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

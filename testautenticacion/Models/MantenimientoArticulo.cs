using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace testautenticacion.Models
{
    public class MantenimientoArticulo
    {
        private SqlConnection con;

        private void conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["administracion"].ToString();
            con = new SqlConnection(constr);
        }
        public List<Articulo> RecuperarTodos()//Traer todos los articulos
        {
            conectar();
            List<Articulo> articulos = new List<Articulo>();
            SqlCommand com = new SqlCommand("SELECT codigo,descripcion,precio FROM articulos",con);
            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())//Si realmente se da lectura del comando indicado select
            {
                Articulo art = new Articulo {
                    Codigo = int.Parse(registros["codigo"].ToString()),
                    Descripcion = registros["descripcion"].ToString(),
                    Precio = float.Parse(registros["precio"].ToString())
                };

                articulos.Add(art);//Almacenando un objeto art en la lista de obj de tipo Articulo

                      
            }
            con.Close();
            return articulos; //Retornando el listado con cada articulio

        }
    }
}
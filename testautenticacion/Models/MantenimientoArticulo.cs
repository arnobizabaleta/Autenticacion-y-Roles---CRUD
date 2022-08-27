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

        public Articulo Recuperar(int codigo)
        {
            conectar();
            SqlCommand comando = new SqlCommand("SELECT codigo,descripcion,precio FROM articulos WHERE codigo = @codigo", con);
            comando.Parameters.Add("@codigo",SqlDbType.Int);
            comando.Parameters["@codigo"].Value = codigo;
            con.Open();
            SqlDataReader registros = comando.ExecuteReader();
             Articulo articulo = new Articulo();
            if (registros.Read())
            {
                articulo.Codigo = codigo;
                articulo.Descripcion = registros["descripcion"].ToString();
                articulo.Precio = float.Parse(registros["precio"].ToString());
            }

            con.Close();
            return articulo;
        }

        public int Modificar(Articulo art)
        {
            conectar();
            SqlCommand comando = new SqlCommand("UPDATE articulos SET descripcion = @descripcion ,precio = @precio  WHERE codigo = @codigo", con);
            comando.Parameters.Add("@codigo", SqlDbType.Int);
            comando.Parameters["@codigo"].Value = art.Codigo;
            comando.Parameters.Add("@descripcion", SqlDbType.VarChar);
            comando.Parameters["@descripcion"].Value = art.Descripcion;
            comando.Parameters.Add("@precio", SqlDbType.Float);
            comando.Parameters["@precio"].Value = art.Precio;
            con.Open();
            int i = comando.ExecuteNonQuery();
            con.Close();
            return i; //Retornado lo que nos retorno el querY UPDATE
        }
        public int Borrar(int codigo)
        {
            conectar();
            SqlCommand cm = new SqlCommand("DELETE FROM articulos WHERE codigo = @codigo ", con);
            cm.Parameters.Add("@codigo", SqlDbType.Int);
            cm.Parameters["@codigo"].Value = codigo;
            con.Open();
            int i = cm.ExecuteNonQuery();
            con.Close();
            return i; //Retornado lo que nos retorno el querY DELETE
        }
    }
}
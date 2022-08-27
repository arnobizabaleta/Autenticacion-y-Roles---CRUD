﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using testautenticacion.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace testautenticacion.Logica
{
    public class LO_Usuario
    {


        public Usuarios EncontrarUsuario(string correo, string clave) {

            

            Usuarios objeto = new Usuarios();


            using (SqlConnection conexion = new SqlConnection("Data Source= . ; Initial Catalog=autenticacion; Integrated Security=true")) {

                string query = "select IdUsuario,Nombres,Correo,Clave,IdRol from USUARIOS where Correo = @pcorreo and Clave = @pclave";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@pcorreo", correo);
                cmd.Parameters.AddWithValue("@pclave", clave);
                cmd.CommandType = CommandType.Text;


                conexion.Open();


                using (SqlDataReader dr = cmd.ExecuteReader()) {

                    while (dr.Read()) {

                        objeto = new Usuarios()

                        {
                            IdUsuario = Convert.ToInt32( dr["IdUsuario"]),
                            Nombres = dr["Nombres"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            IdRol = (Rol)dr["IdRol"],

                        };
                    }
                
                }
            }
            return objeto;

        }

      




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using testautenticacion.Models;
using testautenticacion.Logica;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;

namespace testautenticacion.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source= . ; Initial Catalog=autenticacion; Integrated Security=true";
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(string correo, string clave)

        {    //Validando que si ya la clave está encryptada(Por registro directo de usuarios en la BD)
            //No la volvamos a encryptar
            // if(clave.Length > 20)//Verificando que la clave sea mayor a esa longitud
                
           // {
                clave = ConvertirSha256(clave); //Encryptando password digitado por el usuario
            //}
           
            Usuarios objeto = new LO_Usuario().EncontrarUsuario(correo, clave);

            if (objeto.Nombres != null) {


                FormsAuthentication.SetAuthCookie(objeto.Correo, false);

                Session["Usuario"] = objeto;

                return RedirectToAction("Index", "Home");
            }



            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado, verifique";
                return View();
            }
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuarios oUsuario)
        {
            bool registrado;
            string mensaje;

            if (oUsuario.Clave == oUsuario.ConfirmarClave)
            {

               // oUsuario.Clave =oUsuario.Clave;
               oUsuario.Clave = ConvertirSha256(oUsuario.Clave); //Encryptando clave
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("SP_REGISTRAR_USUARIO", cn);
                cmd.Parameters.AddWithValue("Nombres", oUsuario.Nombres);
                cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();


            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Index", "Acceso");//RETORNANDO AL LOGIN
            }
            else
            {
                return View();
            }

        }


        public static string ConvertirSha256(string texto)
        {
            //using System.Text;
            //USAR LA REFERENCIA DE "System.Security.Cryptography"

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
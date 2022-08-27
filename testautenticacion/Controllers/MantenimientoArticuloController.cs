using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using testautenticacion.Models;
using System.Web.Security;


using testautenticacion.Permisos;

namespace testautenticacion.Controllers
{
    [Authorize]
    public class MantenimientoArticuloController : Controller
    {
        
        private SqlConnection con;
        // GET: MantenimientoArticulo
        public ActionResult Index()
        {
            MantenimientoArticulo ma = new MantenimientoArticulo();

            return View(ma.RecuperarTodos());//Retorne la vista con el listado de los articulos
        }

        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["administracion"].ToString();
            con = new SqlConnection(constr);
        }

        // GET: MantenimientoArticulo/Create
        [PermisosRol(Rol.Administrador)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MantenimientoArticulo/Create
        [HttpPost]
        public ActionResult Create(Articulo art)
        {
            bool registrado;
            string mensaje;
            Conectar();
            SqlCommand cmd = new SqlCommand("sp_RegistrarArticulo", con);
            cmd.Parameters.Add("@codigo", SqlDbType.Int);
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar);
            cmd.Parameters.Add("@precio", SqlDbType.Float);
            cmd.Parameters["@codigo"].Value = art.Codigo;
            cmd.Parameters["@descripcion"].Value = art.Descripcion;
            cmd.Parameters["@precio"].Value = art.Precio;
            cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            int i = cmd.ExecuteNonQuery();

            registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Index", "MantenimientoArticulo");
            }
            else
            {
                return View();
            }
        }

        // GET: MantenimientoArticulo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

    

        // GET: MantenimientoArticulo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MantenimientoArticulo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MantenimientoArticulo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MantenimientoArticulo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

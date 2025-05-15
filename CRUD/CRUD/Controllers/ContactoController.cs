using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD.Models;

namespace CRUD.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();

        private static List<Contacto> olista = new List<Contacto>();

        // GET: Contacto
        public ActionResult Inicio()
        {
            olista = new List<Contacto>(); //inicializar la lista(la limpia)

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Contacto", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto(); //tipo de clase contacto
                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombres = dr["Nombres"].ToString();
                        nuevoContacto.Apellidos = dr["Apellidos"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        olista.Add(nuevoContacto); //agregar a la lista
                    }
                }
            }
                return View(olista);
        }

        [HttpGet] //devuelve la vista
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Editar(int? idcontacto)
        {
            if(idcontacto == null)
            {
                return RedirectToAction("Inicio", "Contacto");
            }

            Contacto ocontacto = olista.Where(c => c.IdContacto == idcontacto).FirstOrDefault();

            return View(ocontacto);
        }

        [HttpPost] //se pueden enviar elementos
        public ActionResult Registrar(Contacto ocontacto)
        {
            olista = new List<Contacto>();

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR", oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Nombres", ocontacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", ocontacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio","Contacto");
        }

        [HttpPost]
        public ActionResult Editar(Contacto ocontacto)
        {
            olista = new List<Contacto>();

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_EDITAR", oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("IdContacto", ocontacto.IdContacto);
                cmd.Parameters.AddWithValue("Nombres", ocontacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", ocontacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Contacto");
        }
    }
}
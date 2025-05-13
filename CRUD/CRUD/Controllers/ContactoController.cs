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

        public ActionResult Registrar()
        {
            return View();
        }
    }
}
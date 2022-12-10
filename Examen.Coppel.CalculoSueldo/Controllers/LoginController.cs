using Examen.Coppel.CalculoSueldo.Models;
using Examen.Coppel.CalculoSueldo.ViewModels;
using Examen.Coppel.CalculoSueldo.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Examen.Coppel.CalculoSueldo.Controllers
{
    public class LoginController : Controller
    {
        public IConfiguration Configuration { get; }

        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ActionResult Index()
        {
            return View("Registro");
        }

        [HttpPost]
        public ActionResult Registro(UsuarioModel usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Registrar", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_UsuarioClave", usuario.UsuarioClave);
                        cmd.Parameters.AddWithValue("@p_Nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@p_Password", usuario.Password);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    return RedirectToAction("Index","Home");
                }

            }
            catch(Exception)
            {
                return View("Registro");
            }
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Registrar", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_UsuarioClave", login.UsuarioClave);
                        cmd.Parameters.AddWithValue("@p_Password", login.Password);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            Response.Cookies.Append("user", "Bienvenido " + login.UsuarioClave);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Error = "Error en Login";
                        }
                        con.Close();
                    }
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception)
            {
                return View("Login");
            }
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("user");
            return RedirectToAction("Index", "Home");
        }
    }
}

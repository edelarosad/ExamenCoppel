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
    public class EmpleadoController : Controller
    {
        public IActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerEmpleados", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    List<EmpleadosViewModel> lista = new List<EmpleadosViewModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lista.Add(new EmpleadosViewModel()
                        {
                            EmpleadoId = Convert.ToInt32(dt.Rows[i]["EmpleadoId"].ToString()),
                            NumeroEmpleado = Int32.Parse(dt.Rows[i]["NumeroEmpleado"].ToString()),
                            Nombre = dt.Rows[i]["Nombre"].ToString(),
                            RolId = Int32.Parse(dt.Rows[i]["RolId"].ToString()),
                            RolClave = dt.Rows[i]["RolClave"].ToString(),
                            Email = dt.Rows[i]["Email"].ToString(),
                            Telefono = dt.Rows[i]["Telefono"].ToString()
                        });
                    }
                    ViewBag.Empleados = lista;
                    con.Close();
                }
                return View();
            }
        }

        public IConfiguration Configuration { get; }

        public EmpleadoController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult AltaEmpleado(int? id)
        {

            EmpleadoModel empleado = new EmpleadoModel();
            RolController rol = new RolController(Configuration);
            var roles = rol.ObtenerRolesListaGenerica();

            ConfiguracionController configuracion = new ConfiguracionController(Configuration);
            var configuracionVM = configuracion.ObtenerUltimoNumeroEmpleado();

            if(id > 0)
            {
                empleado = ObtenerEmpleadoId(id);
            }

            ViewBag.Roles = roles;
            ViewBag.Configuracion = configuracionVM;
            return View(empleado);
        }

        [HttpPost]
        public IActionResult AltaEmpleado(EmpleadoModel empleado)
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using(SqlCommand cmd = new SqlCommand("sp_EmpleadoAltaEdita", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_EmpleadoId", empleado.EmpleadoId);
                    cmd.Parameters.AddWithValue("@p_NumeroEmpleado", empleado.NumeroEmpleado);
                    cmd.Parameters.AddWithValue("@p_Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@p_ApellidoPaterno", empleado.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@p_ApellidoMaterno", empleado.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@p_RolId", empleado.RolId);
                    cmd.Parameters.AddWithValue("@p_Email", empleado.Email);
                    cmd.Parameters.AddWithValue("@p_Telefono", empleado.Telefono);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Eliminar(int? id)
        {
            EmpleadoModel empleado = ObtenerEmpleadoId(id);
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarElimina(int id)
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarEmpleadoId", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_EmpleadoId", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
        }

        [NonAction]
        public List<SelectListItem> ObtenerEmpleadosListaGenerica()
        {
            List<SelectListItem> lstEmpleadosGnr = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerEmpleados", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lstEmpleadosGnr.Add(new SelectListItem { Value = dt.Rows[i]["NumeroEmpleado"].ToString(), Text = dt.Rows[i]["NumeroEmpleado"].ToString() + "-" + dt.Rows[i]["Nombre"].ToString() });
                        }
                    }
                    con.Close();


                    return lstEmpleadosGnr;
                }
            }
        }

        [NonAction]
        public EmpleadoModel ObtenerEmpleadoId(int? id)
        {
            EmpleadoModel empleado = new EmpleadoModel();
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerEmpleadoId", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_EmpleadoId", id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        empleado.EmpleadoId = int.Parse(dt.Rows[0]["EmpleadoId"].ToString());
                        empleado.NumeroEmpleado = int.Parse(dt.Rows[0]["NumeroEmpleado"].ToString());
                        empleado.Nombre = dt.Rows[0]["Nombre"].ToString();
                        empleado.ApellidoPaterno = dt.Rows[0]["ApellidoPaterno"].ToString();
                        empleado.ApellidoMaterno = dt.Rows[0]["ApellidoMaterno"].ToString();
                        empleado.RolId = int.Parse(dt.Rows[0]["RolId"].ToString());
                        empleado.Email = dt.Rows[0]["Email"].ToString();
                        empleado.Telefono = dt.Rows[0]["Telefono"].ToString();
                    }
                    con.Close();


                    return empleado;
                }
            }
        }
    }
}

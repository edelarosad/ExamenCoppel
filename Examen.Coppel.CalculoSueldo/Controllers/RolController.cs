using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen.Coppel.CalculoSueldo.Models;
using Examen.Coppel.CalculoSueldo.ViewModels;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Examen.Coppel.CalculoSueldo.Controllers
{
    public class RolController : Controller
    {
        public IActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerRoles", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    List<RolModel> lista = new List<RolModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lista.Add(new RolModel()
                        {
                            RolId = Convert.ToInt32(dt.Rows[i]["RolId"].ToString()),
                            RolClave = dt.Rows[i]["RolClave"].ToString(),
                            Descripcion = dt.Rows[i]["Descripcion"].ToString(),
                            //bActivo = Convert.ToBoolean(Convert.ToInt32(dt.Rows[i]["bActivo"].ToString()))
                        });
                    }
                    ViewBag.Roles = lista;
                    con.Close();
                }
                return View();
            }
        }

        public IConfiguration Configuration { get; }

        public RolController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public IActionResult AltaRol()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult ObtenerRol()
        {
            using( SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using( SqlCommand cmd = new SqlCommand("sp_ObtenerRoles", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    List<RolModel> listaRol = new List<RolModel>();

                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        listaRol.Add(new RolModel()
                        {
                            RolId = Convert.ToInt32(dt.Rows[i]["RolId"].ToString()),
                            RolClave = dt.Rows[i]["RolClave"].ToString(),
                            Descripcion = dt.Rows[i]["Descripcion"].ToString(),
                            //bActivo = Convert.ToBoolean(Convert.ToInt32(dt.Rows[i]["bActivo"].ToString()))
                        });
                    }
                    con.Close();


                    return Ok(listaRol);
                }
            }
        }

        public IActionResult AltaRol(int? id)
        {
            RolModel rol = new RolModel();
            if(id > 0)
            {
                rol = ObtenerRolId(id);
            }
            return View(rol);
        }

        [HttpPost]
        public IActionResult AltaRol(RolModel rolModel)
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_RolAltaEdita", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_RolId", rolModel.RolId);
                    cmd.Parameters.AddWithValue("@p_RolClave", rolModel.RolClave);
                    cmd.Parameters.AddWithValue("@p_Descripcion", rolModel.Descripcion);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Eliminar(int? id)
        {
            RolModel rol = ObtenerRolId(id);
            return View(rol);
        }

        [HttpPost,ActionName("Eliminar")]
        public IActionResult ConfirmarElimina(int id)
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarRolId", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_RolId", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
        }

        [NonAction]
        public RolModel ObtenerRolId(int? id)
        {
            RolModel rol = new RolModel();

            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerRolId", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_RolId", id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if(dt.Rows.Count > 0)
                    {
                        rol.RolId = int.Parse(dt.Rows[0]["RolId"].ToString());
                        rol.RolClave = dt.Rows[0]["RolClave"].ToString();
                        rol.Descripcion = dt.Rows[0]["Descripcion"].ToString();
                    }
                    con.Close();


                    return rol;
                }
            }

        }

        [NonAction]
        public List<SelectListItem> ObtenerRolesListaGenerica()
        {
            List<SelectListItem> lstEmpleadosGnr = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerRoles", con))
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
                            lstEmpleadosGnr.Add(new SelectListItem { Value = dt.Rows[i]["RolId"].ToString(), Text = dt.Rows[i]["RolClave"].ToString() });
                        }
                    }
                    con.Close();


                    return lstEmpleadosGnr;
                }
            }
        }
    }
}

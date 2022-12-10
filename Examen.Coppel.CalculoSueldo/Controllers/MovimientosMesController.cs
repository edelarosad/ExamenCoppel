using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen.Coppel.CalculoSueldo.Models;
using Examen.Coppel.CalculoSueldo.ViewModels;
using Examen.Coppel.CalculoSueldo.Controllers;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Examen.Coppel.CalculoSueldo.Controllers
{
    public class MovimientosMesController : Controller
    {
        public IConfiguration Configuration { get; }

        public MovimientosMesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerMovimientos", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    List<MovimientoMesViewModel> lista = new List<MovimientoMesViewModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string strMes = ObtenerMesId(Int32.Parse(dt.Rows[i]["Mes"].ToString()));
                        lista.Add(new MovimientoMesViewModel()
                        {
                            MovimientoMesId = Convert.ToInt32(dt.Rows[i]["MovimientoMesId"].ToString()),
                            NumeroEmpleado = Int32.Parse(dt.Rows[i]["NumeroEmpleado"].ToString()),
                            Nombre = dt.Rows[i]["Nombre"].ToString(),
                            Rol = dt.Rows[i]["Rol"].ToString(),
                            Mes = strMes,
                            CantidadEntregas = Int32.Parse(dt.Rows[i]["CantidadEntregas"].ToString())
                        });
                    }
                    ViewBag.Movimientos = lista;
                    con.Close();
                }
                return View();
            }
        }

        public IActionResult AltaEditarMovimiento(int? id)
        {
            MovimientoMesModel movimiento = new MovimientoMesModel();
            EmpleadoController empleado = new EmpleadoController(Configuration);
            var lstEmpleados = empleado.ObtenerEmpleadosListaGenerica();

            if (id > 0)
            {
                movimiento = ObtenerMovimientoId(id);
            }
            ViewBag.empleados = lstEmpleados;
            return View(movimiento);
        }

        [HttpPost]
        public IActionResult AltaEditarMovimiento(MovimientoMesModel model)
        {

            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_MovimientoMesAltaEdita", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_MovimientoMesId", model.MovimientoMesId);
                    cmd.Parameters.AddWithValue("@p_NumeroEmpleado", model.NumeroEmpleado);
                    cmd.Parameters.AddWithValue("@p_Nombre", model.Nombre);
                    cmd.Parameters.AddWithValue("@p_RolId", model.RolId);
                    cmd.Parameters.AddWithValue("@p_Mes", model.Mes);
                    cmd.Parameters.AddWithValue("@p_CantidadEntregas", model.CantidadEntregas);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult CalcularSueldo(int? id)
        {
            MovimientoMesViewModel movimiento = ObtenerMovimientoVMId(id);
            return View(movimiento);
        }

        [HttpPost]
        public EmpleadosViewModel ObtenerEmpleadoId(int id)
        {
            EmpleadosViewModel empleado = new EmpleadosViewModel();

            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerEmpleadoNumero", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_NumeroEmpleado", id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        empleado.NumeroEmpleado = int.Parse(dt.Rows[0]["NumeroEmpleado"].ToString());
                        empleado.Nombre = dt.Rows[0]["Nombre"].ToString();
                        empleado.RolId = int.Parse(dt.Rows[0]["RolId"].ToString());
                        empleado.RolClave = dt.Rows[0]["RolClave"].ToString();
                    }
                    con.Close();


                    return empleado;
                }
            }
        }

        [NonAction]
        public MovimientoMesModel ObtenerMovimientoId(int? id)
        {
            MovimientoMesModel movimiento = new MovimientoMesModel();

            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerMovimientoId", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_MovimientoMesId", id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        movimiento.MovimientoMesId = int.Parse(dt.Rows[0]["MovimientoMesId"].ToString());
                        movimiento.NumeroEmpleado = int.Parse(dt.Rows[0]["NumeroEmpleado"].ToString());
                        movimiento.Nombre = dt.Rows[0]["Nombre"].ToString();
                        movimiento.RolId = int.Parse(dt.Rows[0]["RolId"].ToString());
                        movimiento.Mes = int.Parse(dt.Rows[0]["Mes"].ToString());
                        movimiento.CantidadEntregas = int.Parse(dt.Rows[0]["CantidadEntregas"].ToString());
                        //movimiento.SueldoMesualTotal = decimal.Parse(dt.Rows[0]["SueltoMensualTotal"].ToString());
                        //movimiento.ValesDespensa = decimal.Parse(dt.Rows[0]["ValesDespensa"].ToString());
                    }
                    con.Close();


                    return movimiento;
                }
            }

        }

        [NonAction]
        public MovimientoMesViewModel ObtenerMovimientoVMId(int? id)
        {
            MovimientoMesViewModel movimiento = new MovimientoMesViewModel();

            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerMovimientoVMId", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_MovimientoMesId", id);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        string strMes = ObtenerMesId(Int32.Parse(dt.Rows[0]["Mes"].ToString()));

                        movimiento.MovimientoMesId = int.Parse(dt.Rows[0]["MovimientoMesId"].ToString());
                        movimiento.NumeroEmpleado = int.Parse(dt.Rows[0]["NumeroEmpleado"].ToString());
                        movimiento.Nombre = dt.Rows[0]["Nombre"].ToString();
                        movimiento.Rol = dt.Rows[0]["Rol"].ToString();
                        movimiento.Mes = strMes;
                        movimiento.CantidadEntregas = int.Parse(dt.Rows[0]["CantidadEntregas"].ToString());
                        movimiento.SueldoBruto = decimal.Parse(dt.Rows[0]["SueldoBruto"].ToString());
                        movimiento.Isr = decimal.Parse(dt.Rows[0]["Isr"].ToString());
                        movimiento.Bono = decimal.Parse(dt.Rows[0]["Bono"].ToString());
                        movimiento.ValesDespensa = decimal.Parse(dt.Rows[0]["ValesDespensa"].ToString());
                        movimiento.SueldoTotal = decimal.Parse(dt.Rows[0]["SueldoTotal"].ToString());
                    }
                    con.Close();


                    return movimiento;
                }
            }

        }

        [NonAction]
        public string ObtenerMesId(int id)
        {
            string mes = "";

            switch(id)
            {
                case 1:
                    mes = "ENERO";
                    break;
                case 2:
                    mes = "FEBRERO";
                    break;
                case 3:
                    mes = "MARZO";
                    break;
                case 4:
                    mes = "ABRIL";
                    break;
                case 5:
                    mes = "MAYO";
                    break;
                case 6:
                    mes = "JUNIO";
                    break;
                case 7:
                    mes = "JULIO";
                    break;
                case 8:
                    mes = "AGOSTO";
                    break;
                case 9:
                    mes = "SEPTIEMBRE";
                    break;
                case 10:
                    mes = "OCTUBRE";
                    break;
                case 11:
                    mes = "NOVIEMBRE";
                    break;
                case 12:
                    mes = "DICIEMBRE";
                    break;
            }

            return mes;
        }
    }
}

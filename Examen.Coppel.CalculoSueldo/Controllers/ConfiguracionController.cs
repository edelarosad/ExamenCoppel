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

namespace Examen.Coppel.CalculoSueldo.Controllers
{
    public class ConfiguracionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IConfiguration Configuration { get; }

        public ConfiguracionController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult ObtenerUltimoNumeroEmpleado()
        {
            using (SqlConnection con = new SqlConnection(Configuration["ConnectionStrings:defaultConexion"]))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerUltimoNumeroEmpleado", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                    ConfiguracionViewModel configuracionVM = new ConfiguracionViewModel();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        configuracionVM.ConfiguracionClave = dt.Rows[i]["ConfiguracionClave"].ToString();
                        configuracionVM.Descripcion = dt.Rows[i]["Descripcion"].ToString();
                        configuracionVM.Valor1 = Convert.ToInt32(dt.Rows[i]["Valor1"].ToString());
                    }
                    con.Close();

                    return Ok(configuracionVM);
                }
            }
        }
    }

    //sp_ObtenerUltimoNumeroEmpleado
}

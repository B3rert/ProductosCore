using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ProductosCore.Models;

namespace ProductosCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        //cadena de coneccion
        private readonly string _connectionString;

        public ConsultasController(IConfiguration configuration)
        {
            //asignar la cadena de coneccion de appsettings.json
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        //Obtener todos los prodcutos
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            //Manejo de errores
            try
            {
                //Abrir conexion
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    //Comando sql 
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Productos", sql))
                    {
                        //Tipo de comando 
                        cmd.CommandType = CommandType.Text;

                        //guaradr mis valores
                        List<ProductModel> products = new List<ProductModel>(); 

                        //abir la coneccion
                        await sql.OpenAsync();

                        //Ejucutar la consulta
                        using (var reader = await cmd.ExecuteReaderAsync() )
                        {
                            while ( await reader.ReadAsync())
                            {

                                products.Add(MapToValue(reader));

                            }
                        }

                        return Ok(products);
                    }
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private ProductModel MapToValue(SqlDataReader reader)
        {
            return new ProductModel()
            {
                ProCodigo =         (int)reader["pro_codigo"],
                ProNombre =         (string)reader["pro_nombre"],
                ProDescripcion =    (string)reader["pro_descripcion"],
                ProPrecio =         (decimal)reader["pro_precio"],
            };
        }
    }
}

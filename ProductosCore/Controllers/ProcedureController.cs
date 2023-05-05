using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductosCore.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProductosCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureController : ControllerBase
    {
        //cadena de coneccion
        private readonly string _connectionString;

        public ProcedureController(IConfiguration configuration)
        {
            //asignar la cadena de coneccion de appsettings.json
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        //Eleminar un recurso
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //Manejo de errores
            try
            {
                //sql 
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    //comando sql (consulta)
                    using (SqlCommand cmd = new SqlCommand("DELETE_PRODUCT", sql))
                    {
                        //Tipo de comando procedimiento almacenado
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Parametros
                        cmd.Parameters.Add("@pro_codigo", SqlDbType.Int).Value = id;
                       

                        //Abrir la conexion
                        await sql.OpenAsync();

                        //Ejecuto el comando
                        await cmd.ExecuteNonQueryAsync();

                        //si el comando se ejecuta correctamente
                        return Ok(id);
                    }
                }
            }
            catch (Exception e)
            {
                //retornar error
                return BadRequest(e.Message);
            }
        }

        //Actualizar un producto
        [HttpPut]
        public async Task<IActionResult> PutProduct([FromBody] ProductModel product)
        {
            //Manejo de errores
            try
            {
                //sql 
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    //comando sql (consulta)
                    using (SqlCommand cmd = new SqlCommand("PUT_PRODUCT", sql))
                    {
                        //Tipo de comando procedimiento almacenado
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Parametros
                        cmd.Parameters.Add("@pro_codigo", SqlDbType.Int).Value = product.ProCodigo;
                        cmd.Parameters.Add("@pro_nombre", SqlDbType.VarChar).Value = product.ProNombre;
                        cmd.Parameters.Add("@pro_descripcion", SqlDbType.VarChar).Value = product.ProDescripcion;
                        cmd.Parameters.Add("@pro_precio", SqlDbType.Decimal).Value = product.ProPrecio;

                        //Abrir la conexion
                        await sql.OpenAsync();

                        //Ejecuto el comando
                        await cmd.ExecuteNonQueryAsync();

                        //si el comando se ejecuta correctamente
                        return Ok(product);
                    }
                }
            }
            catch (Exception e)
            {
                //retornar error
                return BadRequest(e.Message);
            }
        }

            //Crear un producto 
            [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductModel product)
        {
            //Manejo de errores
            try
            {
                //sql 
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    //comando sql (consulta)
                    using (SqlCommand cmd = new SqlCommand("POST_PRODUCT", sql))
                    {
                        //Tipo de comando procedimiento almacenado
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Parametros
                        cmd.Parameters.Add("@pro_nombre", SqlDbType.VarChar).Value = product.ProNombre;
                        cmd.Parameters.Add("@pro_descripcion", SqlDbType.VarChar).Value = product.ProDescripcion;
                        cmd.Parameters.Add("@pro_precio", SqlDbType.Decimal).Value = product.ProPrecio;

                        //Abrir la conexion
                        await sql.OpenAsync();

                        //Ejecuto el comando
                        await cmd.ExecuteNonQueryAsync();

                        //si el comando se ejecuta correctamente
                        return Ok(product);
                    }
                }
            }
            catch (Exception e )
            {
                //retornar error
                return BadRequest(e.Message);
            }

        }


        //obtener productos
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            //Manejo de errores
            try
            {
                //sql 
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    //comando sql nombre del procedimiento  
                    using (SqlCommand cmd = new SqlCommand("GET_PRODUCTS", sql))
                    {
                        //Tipo de comando procedimiento almacenadi 
                        cmd.CommandType = CommandType.StoredProcedure;


                        //guaradr mis valores
                        List<ProductModel> products = new List<ProductModel>();

                        //abir la coneccion
                        await sql.OpenAsync();

                        //Ejucutar la consulta
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
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
                ProCodigo = (int)reader["pro_codigo"],
                ProNombre = (string)reader["pro_nombre"],
                ProDescripcion = (string)reader["pro_descripcion"],
                ProPrecio = (decimal)reader["pro_precio"],
            };
        }
    }
}

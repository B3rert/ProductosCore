﻿using Microsoft.AspNetCore.Http;
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

        //Eleminar un recurso
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {

                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand($"DELETE FROM\r\n  Productos\r\nWHERE\r\n  pro_codigo = {id}", sql))
                    {
                        //Tipo del comando 
                        cmd.CommandType = CommandType.Text;

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


                //si el comando no se ejecuta correctamente

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
                //conexion de sql 
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(
                        $"  UPDATE\r\n  Productos\r\nSET\r\n  pro_nombre = '{product.ProNombre}',\r\n  pro_descripcion = '{product.ProDescripcion}',\r\n  pro_precio = {product.ProPrecio}\r\nWHERE\r\n  pro_codigo = {product.ProCodigo}", sql))
                    {
                        //Tipo del comando 
                        cmd.CommandType = CommandType.Text;

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


                //si el comando no se ejecuta correctamente


                return BadRequest(e.Message);
            }

        }

        //Crear un nuvo producto
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductModel product)
        {

            //Manejo de errores
            try
            {

                //Conexion sql server
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    //Comando sql
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Productos " +
                        "(" +
                        "pro_nombre, " +
                        "pro_descripcion, " +
                        "pro_precio" +
                        ")" +
                        "VALUES" +
                        "(" +
                        $"'{product.ProNombre}', " +
                        $"'{product.ProDescripcion}', " +
                        $"{product.ProPrecio}" +
                        ")", sql))
                    {

                        //Tipo de comando
                        cmd.CommandType = CommandType.Text;

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

                //si el comando no se ejecuta correctamente


                return BadRequest(e.Message);
            }
           
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

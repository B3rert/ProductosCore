using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductosCore.Models;

namespace ProductosCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrosController : ControllerBase
    {
       
        //Body form 
        [HttpPut("form/{edad}")]
        public IActionResult PostForm([FromForm] string nombre, int edad)
        {
            return Ok($"Hola {nombre}, edad {edad}");
        }


        //Body json
        [HttpPost]
        public IActionResult PostBodyJson([FromBody] UserModel user )
        {
            return Ok(user);
        }


        //parametros header path
        [HttpGet("header/path/{user}")]
        public IActionResult GetUusario(string user, [FromHeader] string password)
        {

            return Ok(user + password);

        }

        //Paremtros encabezados
        [HttpGet("Header")]
        public IActionResult GetHead([FromHeader] string usuario, [FromHeader] string password)
        {
            return Ok(usuario + password);
        }


        //Paramatros en path
        [HttpGet("{nombre}/{apellido}")]
        public IActionResult GetNombre(string nombre, string apellido)
        {
            return Ok($"Hola {nombre} {apellido}");

        }


        //Codigos de estado
        [HttpGet]
        public IActionResult GetAction()
        {
            //return Ok("Hola mmundo"); 200
            return  BadRequest("Hola Mundo");
        }

        

        //Nombres de los servicios
        [HttpGet("Hola/mundo")]
        public string GetHello()
        {
            return "Hola mundo";
        }

    }
}

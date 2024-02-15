using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaProyect.Models;
using System.Data.SqlClient;

namespace PruebaTecnicaProyect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public ApiController(IConfiguration configuration)      //Inyección de dependencias
        {
            _configuration = configuration;
        }


        //Utilice transacciones dado que un usuario esta relacionado con su negocio

        [HttpPost]
        public async Task<ActionResult<string>> RegistrarInformacion(DatosRegistro datos)
        {
            try
            {
                using var conexion = new SqlConnection(_configuration.GetConnectionString("DefouldConecction"));
                await conexion.OpenAsync();

                using var transaccion = await conexion.BeginTransactionAsync(); //Iniciamos la transaccion

                try
                {
                    // Validar si el correo electrónico ya está registrado
                    if (CorreoEstaRegistrado(datos.Correo))
                    {
                        return BadRequest("El correo electrónico ya está registrado.");
                    }

                    // Generar un código de referencia único
                    string codigoReferencia = GenerarCodigoReferencia();
                    datos.CodigoReferencia = codigoReferencia;

                    var usuarioId = await conexion.ExecuteScalarAsync<int>("INSERT INTO Usuarios VALUES(@Nombre, @Correo, @Contrasena); SELECT SCOPE_IDENTITY();",
                                                                    new { datos.Nombre, datos.Correo, datos.Contrasena }, transaccion);


                    // Insertar información del negocio con el ID del usuario
                    await conexion.ExecuteAsync("INSERT INTO Negocios VALUES(@IdUsuario, @Ruc, @NombreNegocio, @Direccion, @CodigoReferencia)",
                                                new { IdUsuario = usuarioId, datos.RUC, datos.NombreNegocio, datos.Direccion, datos.CodigoReferencia }, transaccion);

             
                    transaccion.Commit(); //Realizamos el commit una vez que todo halla sido ingresado correctamente

                    return Ok(codigoReferencia);
                }
                catch (Exception ex)
                {
                   
                    transaccion.Rollback();   //Regresamos la base de datos a como estaba en caso de un error
                    throw;
                }
            }
            catch (Exception ex)
            {
                
                return BadRequest($"Error al registrar la información: {ex.Message}");
            }
        }

        private bool CorreoEstaRegistrado(string correo) //En este metodo me aseguro de que el correo que ingrese no exista el el registro
        {

            using var conexion = new SqlConnection(_configuration.GetConnectionString("DefouldConecction"));
            var correoRegistrado =  conexion.QueryFirstOrDefault<int>("SELECT COUNT(*) FROM Usuarios WHERE Correo = @Correo", new {correo });

            if (correoRegistrado > 0)
            {
                return true;
            }          
            return false;

        }

        private string GenerarCodigoReferencia()
        {          
            return Guid.NewGuid().ToString();
        }

    }
}

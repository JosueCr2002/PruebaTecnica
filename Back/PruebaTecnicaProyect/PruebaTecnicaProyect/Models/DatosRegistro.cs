using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaProyect.Models
{
    public class DatosRegistro
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        public string Contrasena { get; set; }

        // Datos del negocio
        [Required]
        public string NombreNegocio { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string RUC { get; set; }

        // Referencia al usuario que registra el negocio
        public int UsuarioId { get; set; }

        public string CodigoReferencia { get; set; }

    }
}

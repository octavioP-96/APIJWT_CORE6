using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Auth {
    public class RegisterModel {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El apellido paterno es requerido")]
        public string? ApellidoP { get; set; }
        [Required(ErrorMessage = "El apellido materno es requerido")]
        public string? ApellidoM { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "El mail es requerido")]
        public string? Email { get; set; }
    }
}

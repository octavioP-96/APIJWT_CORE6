using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AuthDTO {
    public class UsuarioMapper {
        public static UsuarioDTO Map(Usuario usr) {
            return new UsuarioDTO {
                Nombre = usr.Nombre,
                ApellidoP = usr.ApellidoP,
                ApellidoM = usr.ApellidoM,
                Email = usr.Email
            };
        }

        public static Usuario Map(UsuarioDTO usr) {
            return new Usuario {
                Nombre = usr.Nombre,
                ApellidoP = usr.ApellidoP,
                ApellidoM = usr.ApellidoM,
                Email = usr.Email
            };
        }
    }
}

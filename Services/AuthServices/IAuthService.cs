using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthServices {
    public interface IAuthService {
        public string GenerarToken(Usuario usr);
        public Usuario Login(string email, string contrasenia);
        public Usuario Registrar(Usuario usr);
        public Usuario ObtenerUsuario(string correo);
    }
}

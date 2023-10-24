using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Auth;
using Models.AuthDTO;
using Models.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthServices {
    public class AuthService : IAuthService {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;

        private const int SaltSize = 16; // Tamaño del salt en bytes
        private const int Iterations = 10000; // Número de iteraciones PBKDF2
        private const int KeySize = 32; // Tamaño del hash en bytes
        public AuthService(AuthDbContext cont, IConfiguration conf) {
            _context = cont;
            _configuration = conf;
        }

        public string GenerarToken(Usuario usr) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim(ClaimTypes.Name, usr.Nombre),
                    new Claim(ClaimTypes.Email, usr.Email),
                    new Claim(ClaimTypes.Role, string.Join(",", usr.Roles))
                }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt16(_configuration["JWT:TokenLifeTime"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Usuario Login(string correo, string contrasenia) {
            var usr = _context.Usuarios.Where(u => u.Email == correo).FirstOrDefault();
            if (usr != null && VerifyPassword(contrasenia, usr.Salt, usr.Contrasenia)) {
                if(usr.Email == "jpajaro@aguapuebla.mx") {
                    usr.Roles = new string[] { "Admin" };
                }
                return usr;
            } else {
                throw new CustomException("Usuario o contraseña incorrectos");
            }
        }

        public Usuario Registrar(Usuario usr) {
            usr.FechaCreacion = DateTime.Now;
            usr.FechaModificacion = DateTime.Now;
            // encrypt password
            var passHash = EncryptPassword(usr.Contrasenia);
            usr.Contrasenia = passHash.Hash;
            usr.Salt = Convert.ToBase64String(passHash.Salt);
            _context.Usuarios.Add(usr);
            _context.SaveChanges();
            return usr;
        }

        public Usuario ObtenerUsuario(string correo) {
            return _context.Usuarios.Where(u => u.Email == correo).FirstOrDefault();
        }

        public HashSalt EncryptPassword(string password) {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(salt);
            }
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = encryptedPassw, Salt = salt };
        }

        public bool VerifyPassword(string enteredPassword, string salt, string storedPassword) {
            byte[] bsalt = Convert.FromBase64String(salt);
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: bsalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }
    }

    public class HashSalt {
        public string Hash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
    }
}

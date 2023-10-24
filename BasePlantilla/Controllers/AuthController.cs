using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Auth;
using Models.AuthDTO;
using Models.Helpers;
using Services.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasePlantilla.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller {
        private readonly IAuthService _authService;

        public AuthController( IAuthService authServ) {
            _authService = authServ;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }
                var user = _authService.Login(model.Email, model.Password);
                var token = _authService.GenerarToken(user);
                var usuarioMap = UsuarioMapper.Map(user);
                return Ok(new Respuesta {
                    Success = true,
                    Message = "Bienvenido",
                    Data = new {
                        Token = token,
                        Usuario = usuarioMap
                    }
                });
            } catch(CustomException ex) {
                return Ok(new Respuesta { Success = false, Message = ex.Message });
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta { Success = false, Message = ex.Message } );
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }
                var registroUsuario = _authService.Registrar(new Usuario {
                    Nombre = model.Nombre,
                    ApellidoP = model.ApellidoP,
                    ApellidoM = model.ApellidoM,
                    Contrasenia = model.Password,
                    Email = model.Email
                });
                return Ok(new Respuesta {
                    Success = true,
                    Message = "Usuario registrado exitosamente",
                    Data = UsuarioMapper.Map(registroUsuario)
                });
            }catch(CustomException ex) {
                return Ok(new Respuesta { Success = false, Message = ex.Message });
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta { Success = false, Message = ex.Message });
            }
        }

    }
}

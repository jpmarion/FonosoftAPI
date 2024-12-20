using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Modelos.LoginController;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Aplicacion;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace FonosoftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private IResponse<IUsuario> _response;
        private readonly ILoginRepo _loginRepo;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuario usuario,
                               IResponse<IUsuario> response,
                               ILoginRepo loginRepo,
                               IConfiguration configuration)
        {
            _usuario = usuario;
            _response = response;
            _loginRepo = loginRepo;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("RegistrarUsuario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarUsuario([FromServices] IAuthEmail authEmail,
                                                          [FromServices] IAes aes,
                                                          [FromServices] Func<string, IValidar> validarFactory,
                                                          RqsRegistrarUsuario rqsRegistrarUsuario)
        {
            _usuario.NombreUsuario = rqsRegistrarUsuario.NombreUsuario;
            _usuario.Email = rqsRegistrarUsuario.Email;

            IValidar validar = validarFactory("ValidarRegistrarUsuario");

            AEjecutarCUAsync<IUsuario> registrarUsuarioCU = new RegistrarUsuarioCU<IUsuario>(_response,
                                                                                           _loginRepo,
                                                                                           _usuario,
                                                                                           validar,
                                                                                           aes,
                                                                                           authEmail);

            _response = await registrarUsuarioCU.Ejecutar();

            if (_response.Error != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, _response.Error);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RspLoginUsuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromServices] Func<string, IValidar> validarFactory,
                                                      [FromServices] IAes aes,
                                                      RqsLoginUsuario rqsLoginUsuario)
        {
            _usuario.NombreUsuario = rqsLoginUsuario.NombreUsuario;
            _usuario.Contrasenia = rqsLoginUsuario.Contrasenia;

            IValidar validar = validarFactory("ValidarLoginUsuario");

            AEjecutarCUAsync<IUsuario> loginUsuario = new LoginUsuarioCU<IUsuario>(_response, _loginRepo, _usuario, validar, aes);
            _response = await loginUsuario.Ejecutar();

            if (_response.Error != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, _response.Error);
            }

            if (_response.Data == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, _response.Error);
            }


            RspLoginUsuario rspLoginUsuario = new RspLoginUsuario();
            rspLoginUsuario.Id = _response.Data.Id;
            rspLoginUsuario.Token = GenerarToken(_response.Data);

            return StatusCode(StatusCodes.Status200OK, rspLoginUsuario);

        }

        [HttpPatch]
        [Route("ResetContrasenia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetContrasenia([FromServices] Func<string, IValidar> validarFactory,
                                                          [FromServices] IAes aes,
                                                          [FromServices] IAuthEmail authEmail,
                                                          RqsResetContrasenia rqsResetContrasenia)
        {
            _usuario.NombreUsuario = rqsResetContrasenia.NombreUsuario;

            IValidar validar = validarFactory("ValidarResetContrasenia");

            AEjecutarCUAsync<IUsuario> resetContrasenia = new ResetContraseniaCU<IUsuario>(_response,
                                                                                           _loginRepo,
                                                                                           validar,
                                                                                           aes,
                                                                                           _usuario,
                                                                                           authEmail);

            _response = await resetContrasenia.Ejecutar();

            if (_response.Error != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, _response.Error);
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet]
        [Route("TokenValido")]
        [ProducesResponseType(typeof(RspTokenValido), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IError), StatusCodes.Status400BadRequest)]
        public IActionResult TokenValido(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Ingrese el token");
            }

            RspTokenValido rspTokenValido = new RspTokenValido();

            rspTokenValido.EsTokenValido = ValidarToken(token);

            return StatusCode(StatusCodes.Status200OK, rspTokenValido);
        }

        private string? GenerarToken(IUsuario usuario)
        {
            string key = _configuration["Jwt:Key"] + "ryvZcx4ERMl+hVUduyQuKdQUDI4hD+qE8AemMhLzXJI=";

            var jwtKey = key.Substring(0, 44);
            var jwtIssuer = _configuration["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]{
                new Claim(ClaimTypes.Sid, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, _usuario.NombreUsuario!),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool ValidarToken(string token)
        {
            var key = _configuration["Jwt:Key"] + "ryvZcx4ERMl+hVUduyQuKdQUDI4hD+qE8AemMhLzXJI=";
            var jwtKey = key.Substring(0, 44);
            var issuer = _configuration["Jwt:Issuer"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
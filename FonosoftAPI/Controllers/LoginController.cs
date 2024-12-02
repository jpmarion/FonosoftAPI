using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Modelos.LoginController;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Aplicacion;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FonosoftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private IResponse<IUsuario> _response;
        private readonly ILoginRepo _loginRepo;

        public LoginController(IUsuario usuario, IResponse<IUsuario> response, ILoginRepo loginRepo)
        {
            _usuario = usuario;
            _response = response;
            _loginRepo = loginRepo;
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
    }
}
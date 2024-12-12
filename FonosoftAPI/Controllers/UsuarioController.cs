using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Modelos.LoginController;
using FonosoftAPI.Modelos.UsuarioController;
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private IResponse<IUsuario> _response;
        private readonly ILoginRepo _loginRepo;
        public UsuarioController(IUsuario usuario,
                               IResponse<IUsuario> response,
                               ILoginRepo loginRepo)
        {
            _usuario = usuario;
            _response = response;
            _loginRepo = loginRepo;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RspGetUsuario), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get([FromServices] IAes aes,
                                             [FromServices] Func<string, IValidar> validarFactory,
                                             int id)
        {
            _usuario.Id = id;

            IValidar validar = validarFactory("ValidarBuscarUsuarioXId");

            AEjecutarCUAsync<IUsuario> buscarUsuarioXIdCU = new BuscarUsuarioXIdCU<IUsuario>(_response,
                                                                            _loginRepo,
                                                                            validar,
                                                                            aes,
                                                                            _usuario);
            _response = await buscarUsuarioXIdCU.Ejecutar();

            if (_response.Error != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, _response.Error);
            }

            if (_response.Data == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            RspGetUsuario rspGetUsuario = new RspGetUsuario();
            rspGetUsuario.Id = _response.Data.Id;
            rspGetUsuario.NombreUsuario = _response.Data.NombreUsuario;
            rspGetUsuario.Email = _response.Data.Email;
            return StatusCode(StatusCodes.Status200OK, rspGetUsuario);
        }


        [HttpPatch]
        [Route("ModificarContrasenia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ModificarContrasenia(
                [FromServices] IAes aes,
                [FromServices] Func<string, IValidar> validarFactory,
                RqsModificarContrasenia rqsModificarContrasenia)
        {
            _usuario.NombreUsuario = rqsModificarContrasenia.NombreUsuario;
            _usuario.Contrasenia = rqsModificarContrasenia.Contrasenia;
            _usuario.NuevaContrasenia = rqsModificarContrasenia.NuevaContrasenia;
            _usuario.RepetirContrasenia = rqsModificarContrasenia.NuevaContrasenia;

            IValidar validar = validarFactory("ValidarModificarUsuario");

            AEjecutarCUAsync<IUsuario> modificarContrasenia = new ModificarContraseniaCU<IUsuario>(_response,
                                                                                                   _loginRepo,
                                                                                                   _usuario,
                                                                                                   aes,
                                                                                                   validar);

            _response = await modificarContrasenia.Ejecutar();

            if (_response.Error != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, _response.Error);
            }

            return StatusCode(StatusCodes.Status200OK);
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
    }
}
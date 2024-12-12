using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarBuscarUsuarioXId : IValidar
    {
        private readonly AValidarUsuario _validarUsuarioId = new ValidarUsuarioId();
        private readonly IUsuario _usuario;

        public ValidarBuscarUsuarioXId(IUsuario usuario)
        {
            _usuario = usuario;
        }

        public IError Validar()
        {
            return _validarUsuarioId.EsValido(_usuario);
        }
    }
}
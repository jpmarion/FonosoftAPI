using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Abstracta
{
    public abstract class AValidarUsuario
    {
        public AValidarUsuario? _validador;
        public void ProximaValidacion(AValidarUsuario validador)
        {
            _validador = validador;
        }
        public abstract IError EsValido(IUsuario usuario);
    }
}
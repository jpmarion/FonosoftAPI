using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Aplicacion
{
    public class LoginUsuarioCU<T> : AEjecutarCUAsync<T>
    {
        private readonly ILoginRepo _loginRepo;
        private IUsuario _usuario;
        private readonly IValidar _validar;
        private readonly IAes _aes;

        public LoginUsuarioCU(IResponse<T> response,
                              ILoginRepo loginRepo,
                              IUsuario usuario,
                              IValidar validar,
                              IAes aes) : base(response)
        {
            _loginRepo = loginRepo;
            _usuario = usuario;
            _validar = validar;
            _aes = aes;
        }

        public override IError Especificaciones()
        {
            return _validar.Validar();
        }

        public override async Task<IResponse<T>> Proceso()
        {
            IUsuario usuarioBuscar = (IUsuario)_usuario.Clone();
            usuarioBuscar.NombreUsuario = _aes.Encriptar(_usuario.NombreUsuario!);

            usuarioBuscar = await _loginRepo.BuscarUsuarioXNombreUsuario(usuarioBuscar);

            IResponse<T> response = new Response<T>();
            if (usuarioBuscar.Contrasenia != _aes.Encriptar(_usuario.Contrasenia!))
            {
                LoginErrores loginErrores = LoginErrores.UsuarioContraseniaErronea;

                IError error = new Error();
                error.NroError = loginErrores.NroError;
                error.MsgError = loginErrores.MsgError;

                response.Error = error;

                return response;
            }

            response.Data = (T)usuarioBuscar;

            return response;
        }

        public override void BeginTransaction()
        {
            _loginRepo.BeginTransaction();
        }
        public override void CommitTransaction()
        {
            _loginRepo.CommitTransaction();
        }
        public override async Task ConexionOpen()
        {
            await _loginRepo.ConexionOpen();
        }
        public override void RollBackTransaction()
        {
            _loginRepo.RollbackTransaction();
        }
    }
}
using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Aplicacion
{
    public class ModificarContraseniaCU<T> : AEjecutarCUAsync<T>
    {
        private readonly ILoginRepo _loginRepo;
        private readonly IUsuario _usuario;
        private readonly IAes _aes;
        private readonly IValidar _validar;

        public ModificarContraseniaCU(IResponse<T> response,
                                      ILoginRepo loginRepo,
                                      IUsuario usuario,
                                      IAes aes,
                                      IValidar validar) : base(response)
        {
            _loginRepo = loginRepo;
            _usuario = usuario;
            _aes = aes;
            _validar = validar;
        }



        public override IError Especificaciones()
        {
            return _validar.Validar();
        }

        public override async Task<IResponse<T>> Proceso()
        {
            IUsuario usuarioBuscar = (IUsuario)_usuario.Clone();
            usuarioBuscar.NombreUsuario = _aes.Encriptar(usuarioBuscar.NombreUsuario!);
            usuarioBuscar = await _loginRepo.BuscarUsuarioXNombreUsuario(usuarioBuscar);

            IResponse<T> response = new Response<T>();
            if (_aes.Encriptar(_usuario.Contrasenia!) != usuarioBuscar.Contrasenia)
            {
                LoginErrores loginErrores = LoginErrores.ContraseniaNullOrEmpty;

                IError error = new Error();
                error.NroError = loginErrores.NroError;
                error.MsgError = loginErrores.MsgError;

                response.Error = error;

                return response;
            }

            _usuario.Id = usuarioBuscar.Id;
            _usuario.Contrasenia = _aes.Encriptar(_usuario.NuevaContrasenia!);

            await _loginRepo.ModificarContrasenia(_usuario);

            response!.Data = (T)_usuario;

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
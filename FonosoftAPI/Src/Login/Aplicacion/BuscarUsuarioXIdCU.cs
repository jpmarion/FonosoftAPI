using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Aplicacion
{
    public class BuscarUsuarioXIdCU<T> : AEjecutarCUAsync<T>
    {
        private readonly ILoginRepo _loginRepo;
        private readonly IValidar _validar;
        private readonly IAes _aes;
        private IUsuario _usuario;

        public BuscarUsuarioXIdCU(IResponse<T> response,
                                  ILoginRepo loginRepo,
                                  IValidar validar,
                                  IAes aes,
                                  IUsuario usuario) : base(response)
        {
            _loginRepo = loginRepo;
            _validar = validar;
            _aes = aes;
            _usuario = usuario;
        }

        public override IError Especificaciones()
        {
            return _validar.Validar();
        }

        public override async Task<IResponse<T>> Proceso()
        {
            _usuario = await _loginRepo.BuscarUsuarioXId(_usuario);

            if (_usuario != null)
            {
                _usuario.NombreUsuario = _aes.Desencriptar(_usuario.NombreUsuario!);
                _usuario.Email = _aes.Desencriptar(_usuario.Email!);
                _response.Data = (T)_usuario;
            }

            return _response;
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
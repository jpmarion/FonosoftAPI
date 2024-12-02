using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarUsuarioNombreUsuarioExiste : AValidarUsuario
    {
        private readonly ILoginRepo _loginRepo;

        public ValidarUsuarioNombreUsuarioExiste(ILoginRepo loginRepo)
        {
            _loginRepo = loginRepo;
        }

        public override IError EsValido(IUsuario usuario)
        {
            if (ExisteNombreUsuario(usuario))
            {
                LoginErrores loginErrores = LoginErrores.NombreUsuarioExistente;

                IError error = new Error();
                error.NroError = loginErrores.NroError;
                error.MsgError = loginErrores.MsgError;

                return error;
            }

            if (_validador != null)
            {
                return _validador.EsValido(usuario);
            }
            return null!;
        }

        private bool ExisteNombreUsuario(IUsuario usuario)
        {
            IUsuario usuarioBuscar = (IUsuario)usuario.Clone();
            usuarioBuscar = _loginRepo.BuscarUsuarioXNombreUsuario(usuarioBuscar).GetAwaiter().GetResult();

            if (usuarioBuscar == null)
            {
                return false;
            }

            return true;
        }
    }
}
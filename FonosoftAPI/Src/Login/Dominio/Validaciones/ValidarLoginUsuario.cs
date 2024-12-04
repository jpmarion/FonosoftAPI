using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Login.Dominio.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

namespace FonosoftAPI.Src.Login.Dominio.Validaciones
{
    public class ValidarLoginUsuario : IValidar
    {
        private readonly AValidarUsuario _validarUsuarioNombreUsuario = new ValidarUsuarioNombreUsuario();
        private readonly AValidarUsuario _validarNombreUsuarioInexistente;
        private readonly AValidarUsuario _validarUsuarioContrasenia = new ValidarUsuarioContrasenia();
        private readonly IUsuario _usuario;
        private readonly ILoginRepo _loginRepo;
        private readonly IAes _aes;

        public ValidarLoginUsuario(IUsuario usuario, ILoginRepo loginRepo, IAes aes)
        {
            _usuario = usuario;
            _loginRepo = loginRepo;
            _aes = aes;

            _validarNombreUsuarioInexistente = new ValidarUsuarioNombreUsuarioInexistente(_loginRepo, _aes);
            _validarUsuarioNombreUsuario.ProximaValidacion(_validarNombreUsuarioInexistente);
            _validarNombreUsuarioInexistente.ProximaValidacion(_validarUsuarioContrasenia);
        }

        public IError Validar()
        {
            return _validarUsuarioNombreUsuario.EsValido(_usuario);
        }
    }
}
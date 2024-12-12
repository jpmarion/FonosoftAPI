using System.Data;
using FonosoftAPI.Src.Compartido.Abstracta;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Infraestructura.Interface;
using MySql.Data.MySqlClient;

namespace FonosoftAPI.Src.Login.Infraestructura
{
    public class LoginMysqlRepo : AMysqlRepo, ILoginRepo
    {
        public LoginMysqlRepo(string server,
                              string user,
                              string database,
                              string port,
                              string password) : base(server, user, database, port, password)
        {
        }

        public async Task<IUsuario> BuscarUsuarioXId(IUsuario usuario){
            using MySqlCommand cmd = new MySqlCommand("S_UsuarioXId", ObtenerConexion());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("pId", usuario.Id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                usuario.Id = reader.GetInt32("id");
                usuario.NombreUsuario = reader.GetString("nombre_usuario");
                usuario.Email = reader.GetString("email");

                return usuario;
            }
            return null!;
        }
        public async Task<IUsuario> BuscarUsuarioXNombreUsuario(IUsuario usuario)
        {
            using MySqlCommand cmd = new MySqlCommand("S_UsuarioXNombreUsuario", ObtenerConexion());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("pNombreUsuario", usuario.NombreUsuario);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                usuario.Id = reader.GetInt32("id");
                usuario.Email = reader.GetString("email");
                usuario.Contrasenia = reader.GetString("contrasenia");

                return usuario;
            }
            return null!;
        }
        public async Task ModificarContrasenia(IUsuario usuario)
        {
            using MySqlCommand cmd = new MySqlCommand("U_UsuarioModifContrasenia", ObtenerConexion());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("pId", usuario.Id);
            cmd.Parameters.AddWithValue("pContrasenia", usuario.Contrasenia);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<IUsuario> RegistrarUsuario(IUsuario usuario)
        {
            using MySqlCommand cmd = new MySqlCommand("I_Usuario", ObtenerConexion());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("pNombreUsuario", usuario.NombreUsuario);
            cmd.Parameters.AddWithValue("pEmail", usuario.Email);
            cmd.Parameters.AddWithValue("pContrasenia", usuario.Contrasenia);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                usuario.Id = reader.GetInt32("Id");

                return usuario;
            }
            return null!;
        }
    }
}
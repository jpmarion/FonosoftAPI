using System.Data;
using FonosoftAPI.Src.Compartido.Interface;
using MySql.Data.MySqlClient;

namespace FonosoftAPI.Src.Compartido.Abstracta
{
    public class AMysqlRepo : ISqlRepo
    {
        private readonly string _connectionString;
        private MySqlConnection? _conexion;
        MySqlTransaction? _mySqlTransaction = null;
        protected AMysqlRepo(string server, string user, string database, string port, string password)
        {
            _connectionString = $"server={server};user={user};database={database};port={port};password={password}";
        }
        public void BeginTransaction()
        {
            _mySqlTransaction = _conexion!.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _mySqlTransaction?.Commit();
            _conexion!.CloseAsync();
            _conexion.Dispose();
        }
        public void RollbackTransaction()
        {
            if (_conexion != null)
            {
                if (_mySqlTransaction != null)
                {
                    _mySqlTransaction!.Rollback();
                    _conexion!.CloseAsync();
                    _conexion.Dispose();
                }
            }
        }
        public async Task ConexionOpen()
        {
            _conexion = new MySqlConnection(_connectionString);
            if (_conexion.State != ConnectionState.Open)
            {
                await _conexion.OpenAsync();
            }
        }
        public MySqlConnection ObtenerConexion()
        {
            return _conexion!;
        }

        public void Dispose()
        {
            _conexion!.Close();
        }
    }
}
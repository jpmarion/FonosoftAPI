using MySql.Data.MySqlClient;

namespace FonosoftAPI.Src.Compartido.Interface
{
    public interface ISqlRepo
    {
        public void BeginTransaction();
        public void CommitTransaction();
        public void RollbackTransaction();
        public Task ConexionOpen();
        public MySqlConnection ObtenerConexion();
    }
}
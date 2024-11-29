namespace FonosoftAPI.Src.Compartido.Interface
{
    public interface IAEjecutarCUAsync
    {
        Task ConexionOpen();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
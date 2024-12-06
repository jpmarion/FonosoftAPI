using System.Collections;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;

namespace FonosoftAPI.Src.Compartido.Abstracta
{
    public abstract class AEjecutarCUAsync<T>
    {
        public IResponse<T> _response;

        protected AEjecutarCUAsync(IResponse<T> response)
        {
            _response = response;
        }

        public abstract Task<IResponse<T>> Proceso();
        public abstract Task ConexionOpen();
        public abstract void BeginTransaction();
        public abstract void CommitTransaction();
        public abstract void RollBackTransaction();
        public abstract IError Especificaciones();
        public async Task<IResponse<T>> Ejecutar()
        {
            try
            {
                await ConexionOpen();
                IError error = Especificaciones();
                if (error != null)
                {
                    _response.Error = error;
                    return _response;
                }
                BeginTransaction();
                _response = await Proceso();
                if (_response.Error == null)
                {
                    CommitTransaction();
                }
                else
                {
                    RollBackTransaction();
                }

                return _response;
            }
            catch (Exception ex)
            {
                RollBackTransaction();
                IError error = new Error();
                if (ex.Data.Count != 0)
                {
                    foreach (DictionaryEntry item in ex.Data)
                    {
                        error.NroError = item.Key.ToString();
                        error.MsgError = item.Value!.ToString();
                        _response.Error = error;
                    }
                    return _response;
                }
                else
                {
                    error.NroError = ex.StackTrace;
                    error.MsgError = ex.Message;
                    _response.Error = error;
                    return _response;
                }
            }
        }
    }
}
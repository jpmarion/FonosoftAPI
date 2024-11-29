using FonosoftAPI.Src.Compartido.Interface;

namespace FonosoftAPI.Src.Compartido.Modelo
{
    public class Response<T> : IResponse<T>
    {
        public IList<T>? Datas { get; set; }
        public T? Data { get; set; }
        public IError? Error { get; set; }
    }
}
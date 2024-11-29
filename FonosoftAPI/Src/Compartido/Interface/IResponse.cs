namespace FonosoftAPI.Src.Compartido.Interface
{
    public interface IResponse<T>
    {
        public IList<T>? Datas { get; set; }
        public T? Data { get; set; }
        public IError? Error { get; set; }
    }
}
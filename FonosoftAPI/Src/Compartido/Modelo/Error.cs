using FonosoftAPI.Src.Compartido.Interface;

namespace FonosoftAPI.Src.Compartido.Modelo
{
    public class Error : IError
    {
        public string? NroError { get; set; }
        public string? MsgError { get; set; }
    }
}
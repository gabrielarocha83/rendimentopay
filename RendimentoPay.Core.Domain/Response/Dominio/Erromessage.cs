namespace RendimentoPay.Core.Domain.Response.Dominio
{
    public class Erromessage
    {
        public int statusCode { get; set; }
        public object message { get; set; }
        public object[] errors { get; set; }
    }
}
using RendimentoPay.Core.Domain.Response.Dominio;

namespace RendimentoPay.Core.Domain.Response
{
    public class OrdemPagamentoResponse : Mensagem
  {
        public Value value { get; set; }
        public object message { get; set; }
        public string transactionId { get; set; }      
       
    }
}
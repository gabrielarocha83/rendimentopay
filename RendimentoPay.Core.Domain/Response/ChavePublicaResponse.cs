using RendimentoPay.Core.Domain.Response.Dominio;

namespace RendimentoPay.Core.Domain.Response
{
    public class ChavePublicaResponse
    {
        public ConteudoChavePublica conteudo { get; set; }
        public object[] erros { get; set; }
        public object[] alertas { get; set; }
        public bool sucesso { get; set; }
    }
}
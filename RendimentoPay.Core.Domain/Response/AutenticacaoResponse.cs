using RendimentoPay.Core.Domain.Response.Dominio;

namespace RendimentoPay.Core.Domain.Response
{

    public class AutenticacaoResponse
    {
        public object conteudo { get; set; }
        public ErroAutenticacao[] erros { get; set; }
        public object[] alertas { get; set; }
        public bool sucesso { get; set; }
    }
}
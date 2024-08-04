using RendimentoPay.Core.Domain.Request.Dominio;

namespace RendimentoPay.Core.Domain.Request
{
    public class OrdemPagamentoRequest
    {
        public Pagador Pagador { get; set; }
        public Beneficiario Beneficiario { get; set; }
        public Dadosoperacao DadosOperacao { get; set; }
    }
}
using RendimentoPay.Core.Domain.Response.Dominio;

namespace RendimentoPay.Core.Domain.Response
{
    /// <summary>
    /// Fila 9606 - Consultar conta (Usado pelo BFF)
    /// {{urlapi_interna}}/Pix/Rendcard/NumeroDeIdentificacaoLegado/2302705176500010702/Documento/27051765000107
    /// </summary>
    public class ContaConsultaResponse : Mensagem
  {
        public ConteudoConsultaConta conteudo { get; set; }
        
    }
}
using RendimentoPay.Core.Domain.Response.Dominio;

namespace RendimentoPay.Core.Domain.Response;

/// <summary>
/// Fila 9603 - Carregar Rendcard (sensibiliza saldo positivamente)
/// {{urlapi_interna}}/Pix/Rendcard/Carga
/// </summary>
public class ContaCargaPositivaResponse : Mensagem
{
  public ConteudoSensibilizaContaPositivo conteudo { get; set; }

}

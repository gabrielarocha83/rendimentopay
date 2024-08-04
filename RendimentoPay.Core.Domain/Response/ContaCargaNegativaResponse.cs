using RendimentoPay.Core.Domain.Response.Dominio;

namespace RendimentoPay.Core.Domain.Response;

/// <summary>
/// Fila 9604 - Descarregar Rendcard (sensibiliza saldo negativamente)
/// {{urlapi_interna}}/Pix/Rendcard/Descarga
/// </summary>
public class ContaCargaNegativaResponse : Mensagem
{
  public ConteudoSensibilizaContaNegativo conteudo { get; set; }

}
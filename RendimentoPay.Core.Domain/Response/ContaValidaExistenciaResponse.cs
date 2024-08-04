using RendimentoPay.Core.Domain.Request.Dominio;

namespace RendimentoPay.Core.Domain.Response;

/// <summary>
/// Fila 9607- Valida Existencia conta
/// {{urlapi_interna}}/Pix/Rendcard/Conta
/// </summary>   
public class ContaValidaExistenciaResponse : Mensagem
{
  public ConteudoValidaExistencia conteudo { get; set; }

}
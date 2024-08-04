namespace RendimentoPay.Core.Domain.Request;
public class AutenticacaoRequest
{
  public required string nomeDoUsuario { get; set; }
  public string? senha { get; set; }
}
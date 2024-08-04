namespace RendimentoPay.Core.Domain.Request
{
  public class AccessToken : AccessTokenDICT
  {
    public string chaveAcesso { get; set; }
    public string Agencia { get; set; }
    public string Conta { get; set; }
  }
}

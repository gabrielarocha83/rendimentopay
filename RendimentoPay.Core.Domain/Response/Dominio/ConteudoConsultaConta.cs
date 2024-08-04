namespace RendimentoPay.Core.Domain.Response.Dominio;

public class ConteudoConsultaConta
{
  public int conta { get; set; }
  public string agencia { get; set; }
  public string numerodeidentificacaolegado { get; set; }
  public string tipodeidentificadorlegado { get; set; }
  public int codigodamoeda { get; set; }
  public int codigodoreseller { get; set; }
  public int codigodaempresa { get; set; }
  public int tipodeempresa { get; set; }
  public string documento { get; set; }
  public string status { get; set; }
}
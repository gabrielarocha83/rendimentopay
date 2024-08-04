namespace RendimentoPay.Core.Domain.Request.Dominio;

public class Dadosoperacao
{
  public string CampoLivre { get; set; }
  public string ReferenciaInterna { get; set; }
  public object DataPagamento { get; set; }
  public float ValorOperacao { get; set; }
  public string NSU { get; set; }
  public object DataHoraAceitacao { get; set; }
  public string FinalidadeTransacao { get; set; }
}
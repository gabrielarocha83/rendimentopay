namespace RendimentoPay.Core.Domain.Response.Dominio
{
    public class ConteudoSensibilizaContaPositivo
    {
        public string codigodasolicitacao { get; set; }
        public int conta { get; set; }
        public string numerodeidentificacaolegado { get; set; }
        public string tipodeidentificacaolegado { get; set; }
        public float valor { get; set; }
        public string operacaorealizada { get; set; }
        public string numerodereferencia { get; set; }
    }
}
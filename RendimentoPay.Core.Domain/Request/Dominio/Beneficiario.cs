namespace RendimentoPay.Core.Domain.Request.Dominio
{
    public class Beneficiario
    {
        public string Chave { get; set; }
        public string EndToEnd { get; set; }
        public Conta1 Conta { get; set; }
        public Pessoa1 Pessoa { get; set; }
    }
}
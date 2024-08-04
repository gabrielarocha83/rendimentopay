namespace RendimentoPay.Core.Domain.Response.Dominio
{
    public class Ordempagamento
    {
        public int numero { get; set; }
        public string endToEnd { get; set; }
        public Status status { get; set; }
        public string informacoes { get; set; }
    }
}
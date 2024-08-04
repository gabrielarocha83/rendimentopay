namespace RendimentoPay.Core.Domain.Request
{
    /// <summary>
    /// Fila 9603 - Carregar Rendcard (sensibiliza saldo positivamente)
    /// {{urlapi_interna}}/Pix/Rendcard/Carga
    /// </summary>
    public class ContaCargaPositivaRequest
    {
        public required string agencia { get; set; }
        public required int conta { get; set; }
        public required float valor { get; set; }
        public string observacao { get; set; }
        public required string numerodereferencia { get; set; }
    }
}
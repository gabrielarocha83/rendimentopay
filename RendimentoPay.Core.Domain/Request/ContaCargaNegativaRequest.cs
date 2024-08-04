namespace RendimentoPay.Core.Domain.Request
{
    /// <summary>
    /// Fila 9604 - Descarregar Rendcard (sensibiliza saldo negativamente)
    /// {{urlapi_interna}}/Pix/Rendcard/Descarga
    /// </summary>
    public class ContaCargaNegativaRequest
    {
        public required string agencia { get; set; }
        public required int conta { get; set; }
        public required float valor { get; set; }
        public string? observacao { get; set; }
        public required string numerodereferencia { get; set; }
    }
}
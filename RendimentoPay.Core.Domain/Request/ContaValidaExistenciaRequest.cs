using RendimentoPay.Core.Domain.Request.Dominio;

namespace RendimentoPay.Core.Domain.Request
{
    /// <summary>
    /// Fila 9607- Valida Existencia conta
    /// {{urlapi_interna}}/Pix/Rendcard/Conta
    /// </summary>
    public class ContaValidaExistenciaRequest
    {        
        public required string op { get; set; }
        public required string path { get; set; }
        public required Value value { get; set; }
    }
}
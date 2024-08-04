using Microsoft.AspNetCore.Mvc;

namespace RendimentoPay.Core.Api.DTO
{
    public class EnviaPixResquestDTO
    {
        [FromHeader]
        public required string client_id { get; set; }
        [FromHeader]
        public required string agencia { get; set; }
        [FromHeader]
        public required string conta { get; set; }
        [FromHeader]
        public required float valor { get; set; }
        [FromHeader]
        public string? observacao { get; set; }
        [FromHeader]
        public required string numerodereferencia { get; set; }
    }
}
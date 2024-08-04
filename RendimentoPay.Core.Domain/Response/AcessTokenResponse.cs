namespace RendimentoPay.Core.Domain.Response
{
    /// <summary>
    /// Autenticação Sensedia
    /// {{EnderecoAPI}}/oauth/access-token
    /// </summary>
    public class AcessTokenResponse
    {
        public required string access_token { get; set; }
        public required string token_type { get; set; }
        public required int expires_in { get; set; }
    }
}

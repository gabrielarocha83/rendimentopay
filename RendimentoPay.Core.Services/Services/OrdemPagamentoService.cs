using Microsoft.Extensions.Configuration;
using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Response;
using RendimentoPay.Core.Services.Interface;
using System.Text.Json;

namespace RendimentoPay.Core.Services.Services
{
    public class OrdemPagamentoService : IOrdemPagamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly IRedisManagerService _redis;
        private readonly RequestBuilder _requestBuilder;
        private readonly bool _habilitaRedis;
        public string ApiUrl = "http://localhost:5180/api/v1/Autenticacao/Autenticacao";

        public OrdemPagamentoService(IRedisManagerService redis, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _redis = redis;
            _requestBuilder = new RequestBuilder();
            _habilitaRedis = Convert.ToBoolean(configuration.GetRequiredSection("CacheSettings").GetSection("HabilitaRedis").Value);
        }

        public async Task<OrdemPagamentoResponse> EnviarOrdemPagamento(OrdemPagamentoRequest dados, AccessToken accessToken)
        {
            string endpointIncluir = "/api/OrdemPagamento/incluir";
            var token = GetAccessTokenAsync(accessToken).ToString();
            accessToken.access_token = token;

            var request = _requestBuilder.BuildPostDictRequest(endpointIncluir, dados, accessToken);

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<OrdemPagamentoResponse>(responseBody);
            }
            catch (Exception ex)
            {
                // Registra exceção no log se necessário
                throw ex;
            }
        }        

        public string GetAccessTokenAsync(AccessToken requestData)
        {
            return "e27d9dd9-dec1-47cd-8934-d7a98ac92ed";
           /* var url = "http://localhost:5180/api/v1/Autenticacao/Autenticacao";

            try
            {
                // Serializa os dados da requisição para JSON
                var jsonContent = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Faz a requisição POST ao endpoint de autenticação
                var response = await _httpClient.PostAsync(url, content);

                // Verifica se a resposta foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    // Lê o conteúdo da resposta
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent; // Retorna o token de acesso
                }
                else
                {
                    // Loga o erro e lança uma exceção
                    //_logger.LogError($"Falha ao obter o token de acesso: {response.ReasonPhrase}");
                    throw new Exception($"Falha ao obter o token de acesso: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Loga a exceção e lança novamente
                //_logger.LogError(ex, "Erro ao tentar obter o token de acesso");
                throw;
            }*/
        }


        // Outros métodos relacionados a ordem de pagamento
    }
}

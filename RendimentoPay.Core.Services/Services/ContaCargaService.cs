using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Response;
using RendimentoPay.Core.Services.Interface;
using System.Text.Json;

namespace RendimentoPay.Core.Services.Services
{
    /// <summary>
    /// Serviço de carga e descarga de saldo em conta
    /// </summary>
    public class ContaCargaService : IContaCargaService
    {
        private readonly HttpClient _httpClient;
        private readonly IRedisManagerService _redis;
        private readonly RequestBuilder _requestBuilder;
        private readonly bool _habilitaRedis;
        private readonly ILogger<ContaCargaService> _logger;        
        
        public ContaCargaService(RequestBuilder requestBuilder, IRedisManagerService redis, IConfiguration configuration, ILogger<ContaCargaService> logger)
        {
            _httpClient = new HttpClient();
            _redis = redis; // Inicialize o campo com o serviço injetado
            _requestBuilder = new RequestBuilder();
            _habilitaRedis = Convert.ToBoolean(configuration.GetRequiredSection("CacheSettings").GetSection("HabilitaRedis").Value);
            _logger = logger;
            _requestBuilder = requestBuilder;
        }

        public async Task<ContaCargaPositivaResponse> Carga(ContaCargaPositivaRequest dados, AccessToken accessToken)
        {
            if (dados == null)
            {
                throw new ArgumentNullException(nameof(dados), "O objeto 'dados' não pode ser nulo.");
            }

            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken), "O objeto 'accessToken' não pode ser nulo.");
            }

            var endpoint = $"/Pix/Rendcard/Carga";
            var request = _requestBuilder.BuildPostRequest(endpoint, dados, accessToken);

            // Habilitar Log
            _logger.LogInformation("Request: {Method} {Url} {Headers} {Body}",
             request.Method, request.RequestUri, request.Headers, JsonSerializer.Serialize(request));

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                ContaCargaPositivaResponse resultado = JsonSerializer.Deserialize<ContaCargaPositivaResponse>(responseBody);

                // Implemente o salvamento de dados no _redis
                // if (_habilitaRedis)
                // {
                //     _redis.SetKeyValue("token", resultado.access_token);
                // }

                // Habilitar Log
                _logger.LogInformation("Response: {StatusCode} {Headers} {Body}",
                 response.StatusCode, response.Headers, responseBody);

                return resultado;
            }
            catch (Exception ex)
            {
                // Habilitar Log
                _logger.LogError("Response: {StatusCode} {Headers} {Body}",
                "400", "cabecalhos", "body");

                throw new InvalidOperationException($"Ocorreu um erro enquanto processava a requisição. {ex.Message}", ex);
            }
        }

        public async Task<ContaCargaNegativaResponse> Descarga(ContaCargaNegativaRequest dados, AccessToken accessToken, Boolean boLocal = true)
        {            
            if (dados == null)
            {
                throw new ArgumentNullException(nameof(dados), "O objeto 'dados' não pode ser nulo.");
            }

            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken), "O objeto 'accessToken' não pode ser nulo.");
            }
          
            var endpoint = $"/Pix/Rendcard/Descarga";
            var request = _requestBuilder.BuildPostRequest(endpoint, dados, accessToken);

            // Habilitar Log
            _logger.LogInformation("Request: {Method} {Url} {Headers} {Body}",
             request.Method, request.RequestUri, request.Headers, JsonSerializer.Serialize(request));
            
            HttpResponseMessage response = new HttpResponseMessage();
            string responseBody = "";
            ContaCargaNegativaResponse resultado = new();
            try
            {
                if (!boLocal)
                {
                    response.EnsureSuccessStatusCode();

                    response = await _httpClient.SendAsync(request);
                    responseBody = await response.Content.ReadAsStringAsync();
                    resultado = JsonSerializer.Deserialize<ContaCargaNegativaResponse>(responseBody);
                }
                else
                {
                    responseBody = @"{
                                                ""value"": null,
                                                ""message"": null,
                                                ""transactionId"": ""f4c1216a-d402-4414-9aae-43431920546b"",
                                                ""erroMessage"": {
                                                    ""statusCode"": 404,
                                                    ""message"": ""Não foi possível realizar a sensibilização negativa da conta."",
                                                    ""errors"": [
                                                        {
                                                            ""domain"": ""E00000"",
                                                            ""reason"": null,
                                                            ""message"": ""Nenhuma chave encontrada""
                                                        }
                                                    ]
                                                },
                                                ""isSuccess"": true,
                                                ""isFailure"": false
                                            }";
                    resultado = JsonSerializer.Deserialize<ContaCargaNegativaResponse>(responseBody);
                }
                // Implemente o salvamento de dados no _redis
                // if (_habilitaRedis)
                // {
                //     _redis.SetKeyValue("token", resultado.access_token);
                // }

                // Habilitar Log
                _logger.LogInformation("Response: {StatusCode} {Headers} {Body}",
                 response.StatusCode, response.Headers, responseBody);               

                return resultado;
            }
            catch (Exception ex)
            {
                // Habilitar Log
                _logger.LogError("Response: {StatusCode} {Headers} {Body}",
                "400", "cabecalhos", "body");                

                throw new InvalidOperationException($"Ocorreu um erro enquanto processava a requisição. {ex.Message}", ex);
            }
        }

        public async Task<ContaSaldoResponse> Saldo(AccessToken accessToken, Boolean boLocal = true)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken), "O objeto 'accessToken' não pode ser nulo.");
            }

            var endpoint = $"/Pix/Rendcard/Agencia/{accessToken.Agencia}/Conta/{accessToken.Conta}/saldo";
            var request = _requestBuilder.BuildGetRequest(endpoint, accessToken);

            _logger.LogInformation("Request: {Method} {Url} {Headers} ",
             request.Method, request.RequestUri, request.Headers);

            HttpResponseMessage response = new HttpResponseMessage();
            string responseBody = "";
            ContaSaldoResponse resultado = new ContaSaldoResponse();
            try
            {
                if (!boLocal)
                {
                    response.EnsureSuccessStatusCode();

                    response = await _httpClient.SendAsync(request);
                    responseBody = await response.Content.ReadAsStringAsync();
                    resultado = JsonSerializer.Deserialize<ContaSaldoResponse>(responseBody);
                }
                else
                {
                    responseBody = @"{
                                                ""saldo"": 1000,
                                                ""message"": null,
                                                ""transactionId"": ""f4c1216a-d402-4414-9aae-43431920546b"",
                                                ""erroMessage"": {
                                                    ""statusCode"": 404,
                                                    ""message"": ""Saldo insuficiente."",
                                                    ""errors"": [
                                                        {
                                                            ""domain"": ""E00000"",
                                                            ""reason"": null,
                                                            ""message"": ""Nenhuma chave encontrada""
                                                        }
                                                    ]
                                                },
                                                ""isSuccess"": true,
                                                ""isFailure"": false
                                            }";
                    resultado = JsonSerializer.Deserialize<ContaSaldoResponse>(responseBody);
                }
                            
                // Implemente o salvamento de dados no _redis
                // if (_habilitaRedis)
                // {
                //     _redis.SetKeyValue("token", resultado.access_token);
                // }

                // Implemente a lógica de salvamento no banco conforme necessário
                
                _logger.LogInformation("Response: {StatusCode} {Headers} {Body}",
                 response.StatusCode, response.Headers, responseBody);

                return resultado;
            }
            catch (Exception ex)
            {
                // Registra exceção no log se necessário
                throw new InvalidOperationException($"Ocorreu um erro enquanto processava a requisição. {ex.Message}", ex);
            }
        }

        public async Task<ContaConsultaResponse> Consulta(AccessToken accessToken)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken), "O objeto 'accessToken' não pode ser nulo.");
            }

            var endpoint = $"/Pix/Rendcard/NumeroDeIdentificacaoLegado/2302705176500010702/Documento/27051765000107";            
            var request = _requestBuilder.BuildGetRequest(endpoint, accessToken);

            _logger.LogInformation("Request: {Method} {Url} {Headers} ",
             request.Method, request.RequestUri, request.Headers);

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                ContaConsultaResponse resultado = JsonSerializer.Deserialize<ContaConsultaResponse>(responseBody);

                // Implemente o salvamento de dados no _redis
                // if (_habilitaRedis)
                // {
                //     _redis.SetKeyValue("token", resultado.access_token);
                // }

                // Implemente a lógica de salvamento no banco conforme necessário

                _logger.LogInformation("Response: {StatusCode} {Headers} {Body}",
                 response.StatusCode, response.Headers, responseBody);

                return resultado;
            }
            catch (Exception ex)
            {
                // Registra exceção no log se necessário
                throw new InvalidOperationException($"Ocorreu um erro enquanto processava a requisição. {ex.Message}", ex);
            }
        }       

        public async Task<ContaValidaExistenciaResponse> ValidaExistencia(ContaValidaExistenciaRequest dados, AccessToken accessToken, Boolean boLocal = true)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken), "O objeto 'accessToken' não pode ser nulo.");
            }

            var endpoint = $"/Pix/Rendcard/Conta";
            var request = _requestBuilder.BuildPatchRequest(endpoint, dados, accessToken);

            _logger.LogInformation("Request: {Method} {Url} {Headers} ",
             request.Method, request.RequestUri, request.Headers);

            HttpResponseMessage response = new HttpResponseMessage();
            string responseBody = "";
            ContaValidaExistenciaResponse resultado = new ContaValidaExistenciaResponse();
            try
            {
                if (!boLocal)
                {
                    
                    response.EnsureSuccessStatusCode();

                    response = await _httpClient.SendAsync(request);
                    responseBody = await response.Content.ReadAsStringAsync();
                    resultado = JsonSerializer.Deserialize<ContaValidaExistenciaResponse>(responseBody);
                }
                else
                {
                    responseBody = @"{
                                                ""value"": null,
                                                ""message"": null,
                                                ""transactionId"": ""f4c1216a-d402-4414-9aae-43431920546b"",
                                                ""erroMessage"": {
                                                    ""statusCode"": 404,
                                                    ""message"": ""A conta não está ativa."",
                                                    ""errors"": [
                                                        {
                                                            ""domain"": ""E00000"",
                                                            ""reason"": null,
                                                            ""message"": ""Nenhuma chave encontrada""
                                                        }
                                                    ]
                                                },
                                                ""isSuccess"": true,
                                                ""isFailure"": false
                                            }";
                    resultado = JsonSerializer.Deserialize<ContaValidaExistenciaResponse>(responseBody);
                }

                // Implemente o salvamento de dados no _redis
                // if (_habilitaRedis)
                // {
                //     _redis.SetKeyValue("token", resultado.access_token);
                // }

                // Implemente a lógica de salvamento no banco conforme necessário

                _logger.LogInformation("Response: {StatusCode} {Headers} {Body}",
                 response.StatusCode, response.Headers, responseBody);

                return resultado;
            }
            catch (Exception ex)
            {
                // Registra exceção no log se necessário
                throw new InvalidOperationException($"Ocorreu um erro enquanto processava a requisição. {ex.Message}", ex);
            }
        }       
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Response;
using RendimentoPay.Core.Services.Interface;
using System.Text;
using System.Text.Json;

namespace RendimentoPay.Core.Services.Services;

public class AutenticacaoService : IAutenticacaoService
{
  private readonly HttpClient _httpClient;
  private readonly IRedisManagerService _redis;
  private readonly RequestBuilder _requestBuilder;
  private readonly bool _habilitaRedis;
  private readonly ILogger<ContaCargaService> _logger;

  public AutenticacaoService(RequestBuilder requestBuilder, IRedisManagerService redis, IConfiguration configuration, ILogger<ContaCargaService> logger)
  {
    _httpClient = new HttpClient();
    _redis = redis; // Inicialize o campo com o serviço injetado
    _requestBuilder = new RequestBuilder();
    _habilitaRedis = Convert.ToBoolean(configuration.GetRequiredSection("CacheSettings").GetSection("HabilitaRedis").Value);
    _logger = logger;
    _requestBuilder = requestBuilder;
  }

  public async Task<AutenticacaoResponse> Autenticar(AutenticacaoRequest dados, Domain.Request.AccessToken token)
  {
    if (dados == null)
    {
      throw new ArgumentNullException(nameof(dados), "O objeto 'dados' não pode ser nulo.");
    }

    if (token == null)
    {
      throw new ArgumentNullException(nameof(token), "O objeto 'token' não pode ser nulo.");
    }

    if (string.IsNullOrEmpty(_redis.GetKeyValue("senhacriptografada")))
    {
      await ObterChavePublica(token.client_id);
    }

    dados.senha = _redis.GetKeyValue("senhacriptografada");

    var endpoint = $"/AplicacaoCliente/{token.client_id}/Acesso";
    var request = _requestBuilder.BuildPostRequest(endpoint, dados, token);

    // Habilitar Log
    // TODO: Desabilitar este log quando terminar o periodo de desenvolvimento
    _logger.LogInformation("Request: {Method} {Url} {Headers} {Body}",
     request.Method, request.RequestUri, request.Headers, JsonSerializer.Serialize(request));

    try
    {
      HttpResponseMessage response = await _httpClient.SendAsync(request);
      response.EnsureSuccessStatusCode();

      string responseBody = await response.Content.ReadAsStringAsync();
      AutenticacaoResponse resultado = JsonSerializer.Deserialize<AutenticacaoResponse>(responseBody);

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

  private async Task<ChavePublicaResponse> ObterChavePublica(string clientId)
  {
    if (string.IsNullOrEmpty(clientId))
    {
      throw new ArgumentNullException(nameof(clientId), "O objeto 'clientId' não pode ser nulo.");
    }

    var endpoint = $"/AplicacaoCliente/{clientId}/Chave";
    var request = _requestBuilder.BuildGetRequest(endpoint, null);

    // Habilitar Log
    _logger.LogInformation("Request: {Method} {Url} {Headers} {Body}",
     request.Method, request.RequestUri, request.Headers, JsonSerializer.Serialize(request));

    try
    {
      HttpResponseMessage response = await _httpClient.SendAsync(request);
      response.EnsureSuccessStatusCode();

      string responseBody = await response.Content.ReadAsStringAsync();
      ChavePublicaResponse resultado = JsonSerializer.Deserialize<ChavePublicaResponse>(responseBody);

      // Implemente o salvamento de dados no _redis
      if (_habilitaRedis)
      {
        _redis.SetKeyValue("chave", resultado.conteudo.chave);
        _redis.SetKeyValue("senhacriptografada", Encriptar(resultado.conteudo.chave, "8b627277a"));
      }

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

  private static string Encriptar(string chavePublica, string mensagem)
  {
    AsymmetricKeyParameter parameters = (AsymmetricKeyParameter)new PemReader(new StringReader(chavePublica)).ReadObject();
    byte[] bytes = Encoding.UTF8.GetBytes(mensagem);
    RsaEngine rsaEngine = new RsaEngine();
    rsaEngine.Init(forEncryption: true, parameters);
    int num = bytes.Length;
    int num2 = rsaEngine.GetInputBlockSize() + 1;
    List<byte> list = new List<byte>();
    for (int i = 0; i < num; i += num2)
    {
      int inLen = Math.Min(num2, num - i);
      list.AddRange(rsaEngine.ProcessBlock(bytes, i, inLen));
    }
    return Convert.ToBase64String(list.ToArray());
  }
}
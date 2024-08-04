using RendimentoPay.Core.Domain.Request;
using System.Text;
using System.Text.Json;

namespace RendimentoPay.Core.Services.Services;
public class RequestBuilder
{
  private readonly string _urlApi_interna = "https://agiplatapi.hom.agi.app.br";
  private readonly string _urlapi_sensedia = "https://apisandbox.agillitas.com.br/hom";
  private readonly string _urlapi_localHost = "http://localhost:5180";

  private HttpRequestMessage _request;

  #region Métodos para montar o request do CORE
  /// <summary>
  /// Escreve o header dos métodos do CORE
  /// </summary>
  /// <param name="accessToken"></param>
  private void BuildRequestBase(AccessToken accessToken)
  {
    _request.Headers.Add("Client_id", accessToken.client_id);
  }

  /// <summary>
  /// Monta o Request para metodos POST do CORE
  /// </summary>
  /// <param name="endpoint">Informe o EndPoint</param>
  /// <param name="content">Informe o conteudo do body</param>
  /// <param name="accessToken">Informe os dados do header</param>
  /// <returns></returns>
  public HttpRequestMessage BuildPostRequest(string endpoint, object content, AccessToken accessToken)
  {
    _request = new HttpRequestMessage(HttpMethod.Post, _urlapi_localHost + endpoint);

    BuildRequestBase(accessToken);

    var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    _request.Content = jsonContent;

    return _request;
  }

  /// <summary>
  /// Monta o Request para metodos GET do CORE
  /// </summary>
  /// <param name="endpoint">Informe o EndPoint</param>
  /// <param name="accessToken">Informe os dados do header</param>
  /// <param name="urlBase">Neste caso, temos endpoints com urlbase diferentes. Usar conforme cada caso</param>
  /// <returns></returns>
  public HttpRequestMessage BuildGetRequest(string endpoint, AccessToken accessToken, string urlBase = "urlbase")
  {
    _request = new HttpRequestMessage(HttpMethod.Get, (urlBase == "urlbase" ? _urlApi_interna : _urlapi_sensedia) + endpoint);

    BuildRequestBase(accessToken);

    return _request;
  }

  /// <summary>
  /// Montar o Request para metodos DELETE do CORE
  /// </summary>
  /// <param name="endpoint">Informe o endPoint</param>
  /// <param name="content">Informe o conteudo do body</param>
  /// <param name="accessToken">Informe os dados do Header</param>
  /// <returns></returns>
  public HttpRequestMessage BuildDeleteRequest(string endpoint, object content, AccessToken accessToken)
  {
    _request = new HttpRequestMessage(HttpMethod.Delete, _urlApi_interna + endpoint);
    BuildRequestBase(accessToken);

    var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    _request.Content = jsonContent;

    return _request;
  }

  /// <summary>
  /// Montar o Request para metodos PATCH do CORE
  /// </summary>
  /// <param name="endpoint">Informe o endPoint</param>
  /// <param name="content">Informe o conteudo do body</param>
  /// <param name="accessToken">Informe os dados do Header</param>
  /// <returns></returns>
  public HttpRequestMessage BuildPatchRequest(string endpoint, object content, AccessToken accessToken)
  {
    _request = new HttpRequestMessage(HttpMethod.Patch, _urlApi_interna + endpoint);
    BuildRequestBase(accessToken);
    var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    _request.Content = jsonContent;

    return _request;
  }
  #endregion

  #region Métodos para montar o request do DICT
  /// <summary>
  /// Monta o Request para metodos POST do DICT
  /// </summary>
  /// <param name="endpoint">Informe o EndPoint</param>
  /// <param name="content">Informe o conteudo do body</param>
  /// <param name="accessToken">Informe os dados do header</param>
  /// <returns></returns>
  public HttpRequestMessage BuildPostDictRequest(string endpoint, object content, AccessToken accessToken)
  {
    _request = new HttpRequestMessage(HttpMethod.Post, _urlapi_localHost + endpoint);

    BuildDictRequestBase(accessToken);
    if (string.IsNullOrWhiteSpace(accessToken.chaveAcesso))
    {
      BuildRequestBaseCh(accessToken);
    }
    var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    _request.Content = jsonContent;

    return _request;
  }

  /// <summary>
  /// Escreve os headers do endpoint do DICT
  /// </summary>
  /// <param name="accessToken"></param>
  private void BuildDictRequestBase(AccessToken accessToken)
  {
    _request.Headers.Add("Authorization", accessToken.Authorization);
    _request.Headers.Add("Client_id", accessToken.client_id);
    _request.Headers.Add("Client_secret", accessToken.client_secret);
    _request.Headers.Add("Access_token", accessToken.access_token);
    _request.Headers.Add("ChaveAcesso", accessToken.chaveAcesso);
  }

  /// <summary>
  /// Complemente a escrita do header dos métodos do DICT 
  /// </summary>
  /// <returns>token de acesso válido</returns>
  /// <exception cref="NotImplementedException"></exception>    
  private void BuildRequestBaseCh(AccessToken accessToken)
  {
    _request.Headers.Add("ChaveAcesso", accessToken.chaveAcesso);
  }
  #endregion
}
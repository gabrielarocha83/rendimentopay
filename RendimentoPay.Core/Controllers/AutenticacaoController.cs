using Microsoft.AspNetCore.Mvc;
using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Services.Interface;

namespace RendimentoPay.Core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AutenticacaoController : ControllerBase
{
  private readonly IAutenticacaoService _autenticacaoService;
  private readonly ILogger<AutenticacaoController> _logger;

  public AutenticacaoController(IAutenticacaoService autenticacaoService, ILogger<AutenticacaoController> logger)
  {
    _autenticacaoService = autenticacaoService;
    _logger = logger;
  }

  [HttpPost("autenticar")]
  public async Task<IActionResult> Autenticar([FromBody] AutenticacaoRequest dados, [FromHeader] string clientId, [FromHeader] string clientSecret)
  {
    if (dados == null)
    {
      return BadRequest("Dados de autenticação são obrigatórios.");
    }

    var token = new AccessToken { client_id = clientId, client_secret = clientSecret };

    try
    {
      var response = await _autenticacaoService.Autenticar(dados, token);
      return Ok(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao autenticar.");
      return StatusCode(500, "Ocorreu um erro ao processar a solicitação.");
    }
  }
}
using Microsoft.AspNetCore.Mvc;
using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Services.Interface;

namespace RendimentoPay.Core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrdemPagamentoController : ControllerBase
{
  private readonly IOrdemPagamentoService _ordemPagamentoService;
  private readonly ILogger<OrdemPagamentoController> _logger;

  public OrdemPagamentoController(IOrdemPagamentoService ordemPagamentoService, ILogger<OrdemPagamentoController> logger)
  {
    _ordemPagamentoService = ordemPagamentoService;
    _logger = logger;
  }

  [HttpPost("enviar")]
  public async Task<IActionResult> EnviarOrdemPagamento([FromBody] OrdemPagamentoRequest dados, [FromHeader] string clientId, [FromHeader] string clientSecret)
  {
    if (dados == null)
    {
      return BadRequest("Dados da ordem de pagamento são obrigatórios.");
    }

    if (string.IsNullOrEmpty(clientId))
    {
      return BadRequest("ClientId é obrigatório.");
    }

    if (string.IsNullOrEmpty(clientSecret))
    {
      return BadRequest("clientSecret é obrigatório.");
    }

    var accessToken = new AccessToken { client_id = clientId, client_secret = clientSecret };

    try
    {
      var response = await _ordemPagamentoService.EnviarOrdemPagamento(dados, accessToken);
      return Ok(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao enviar ordem de pagamento.");
      return StatusCode(500, "Ocorreu um erro ao processar a solicitação.");
    }
  }
}
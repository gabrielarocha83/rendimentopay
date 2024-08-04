using Microsoft.AspNetCore.Mvc;
using RendimentoPay.Core.Api.DTO;
using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Request.Dominio;
using RendimentoPay.Core.Services.Interface;
using RendimentoPay.Core.Services.Services;

namespace RendimentoPay.Core.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ContaController : ControllerBase
{
  private readonly IContaCargaService _contaCargaService;
  private readonly IOrdemPagamentoService _ordemPagamentoService;
  private readonly ILogger<ContaController> _logger;

  public ContaController(IContaCargaService contaCargaService, IOrdemPagamentoService ordemPagamentoService, ILogger<ContaController> logger)
  {
    _contaCargaService = new ContaCargaServiceDecorator(contaCargaService);
    _ordemPagamentoService = ordemPagamentoService;
    _logger = logger;
  }

  [HttpPost("EnviaPix")]
  public async Task<IActionResult> EnviaPix(ContaValidaExistenciaRequest validaConta, EnviaPixResquestDTO accessToken)
  {
    if (accessToken == null)
    {
      throw new ArgumentNullException(nameof(accessToken), "O objeto 'accessToken' não pode ser nulo.");
    }

    //Monta ContaValidaExistenciaRequest
    var validaReq = validaConta;

    var token = new AccessToken()
    {
      Agencia = accessToken.agencia,
      client_id = accessToken.client_id,
      Conta = accessToken.conta
    };

    var negativaRequest = new ContaCargaNegativaRequest
    {
      agencia = accessToken.agencia,
      conta = Convert.ToInt32(accessToken.conta),
      valor = accessToken.valor,
      observacao = accessToken.observacao,
      numerodereferencia = accessToken.numerodereferencia
    };

    // Chamada 1 - Consultar conta No API CC para validar se a conta está ativa
    var contaAtiva = await _contaCargaService.ValidaExistencia(validaReq, token);

    if (!contaAtiva.isSuccess)
    {
      var erroResponse = new
      {
        Sucesso = false,
        Mensagem = contaAtiva.erroMessage.message,
        StatusCode = contaAtiva.erroMessage.statusCode
      };

      return new ObjectResult(erroResponse)
      {
        StatusCode = 400 // Bad Request
      };
    }

    // Chamada 2 - Consultar Saldo no API CC
    var saldo = await _contaCargaService.Saldo(token);

    if (saldo.conteudo.saldo < accessToken.valor)
    {
      var erroResponse = new
      {
        Sucesso = false,
        Mensagem = saldo.erroMessage.message,
        StatusCode = saldo.erroMessage.statusCode
      };

      return new ObjectResult(erroResponse)
      {
        StatusCode = 400 // Bad Request
      };
    }

    // Chamada 3 - Sensibiliza saldo negativo 
    var descargaSaldo = await _contaCargaService.Descarga(negativaRequest, token);
    if (descargaSaldo.isFailure)
    {
      var erroResponse = new
      {
        Sucesso = false,
        Mensagem = descargaSaldo.erroMessage.message,
        StatusCode = descargaSaldo.erroMessage.statusCode
      };

      return new ObjectResult(erroResponse)
      {
        StatusCode = 400 // Bad Request
      };
    }

    //Chamada 4 - Decorator
    //Consumir a api do DICT de envio de OP
    //http://localhost:5180/api/OrdemPagamento/incluir          

    var contaBeneficiario = new Conta1()
    {
      TipoId = 1,
      Agencia = "3194",
      ISPBParticipante = "13776742",
      Numero = "19798"
    };
    var pessoaBeneficiario = new Pessoa1()
    {
      InscricaoNacional = "03180155795",
      Nome = "Flavio Barbosa"
    };
    var beneficiario = new Beneficiario()
    {
      Chave = "37952189000",
      EndToEnd = "E13776742202407281243qdFpQyo1YZI",
      Conta = contaBeneficiario,
      Pessoa = pessoaBeneficiario
    };

    var contaPagador = new Conta()
    {
      TipoId = "1",
      Agencia = "3194",
      ISPBParticipante = "13776742",
      Numero = "19798"
    };
    var pessoaPagadora = new Pessoa()
    {
      InscricaoNacional = "09509548022",
      Nome = "Teste RendimentoPay",
      NomeFantasia = "",
      TipoId = "1"
    };
    var pagador = new Pagador()
    {
      Conta = contaPagador,
      Pessoa = pessoaPagadora
    };

    var dados = new Dadosoperacao()
    {
      CampoLivre = "",
      DataHoraAceitacao = null,
      DataPagamento = null,
      FinalidadeTransacao = "",
      NSU = "",
      ReferenciaInterna = "",
      ValorOperacao = 10.35F
    };

    var opReq = new OrdemPagamentoRequest()
    {
      Beneficiario = beneficiario,
      Pagador = pagador,
      DadosOperacao = dados
    };

    var tokench = new AccessToken()
    {
      Agencia = "00019",
      Conta = "0000263874",
      Authorization = "Basic NzJhZGRjMWMtNzRjMC00YmVhLWFlZmMtN2I1NWI3ZDUyNmJkOjdiZjU2ODlhLTBkZGItNDZkNS05MDRhLTRmNTQ4MThhZTlhZQ==",
      client_id = "72addc1c-74c0-4bea-aefc-7b55b7d526bd",
      client_secret = "7bf5689a-0ddb-46d5-904a-4f54818ae9ae",
      chaveAcesso = "dab59cfe-219d-4be3-ab07-956564b0008d",
      access_token = "d32a7c34-1b02-434d-a407-8c88e661965d"
    };

    /*
    var op = await _ordemPagamentoService.EnviarOrdemPagamento(opReq, tokench);

    if (op.isFailure)
    {
        if (descargaSaldo.isFailure)
        {
            var erroResponse = new
            {
                Sucesso = false,
                Mensagem = descargaSaldo.erroMessage.message,
                StatusCode = descargaSaldo.erroMessage.statusCode
            };

            return new ObjectResult(erroResponse)
            {
                StatusCode = 400 // Bad Request
            };
        }
    }
    */

    // Chamada 5 - Sensibiliza saldo negativo 

    var cargaRequest = new ContaCargaPositivaRequest
    {
      agencia = accessToken.agencia,
      conta = Convert.ToInt32(accessToken.conta),
      valor = accessToken.valor,
      observacao = accessToken.observacao,
      numerodereferencia = accessToken.numerodereferencia
    };

    var cargaSaldo = await _contaCargaService.Carga(cargaRequest, token);
    if (cargaSaldo.isSuccess)
    {
      var erroResponse = new
      {
        Sucesso = false,
        Mensagem = descargaSaldo.erroMessage.message,
        StatusCode = descargaSaldo.erroMessage.statusCode
      };

      return new ObjectResult(erroResponse)
      {
        StatusCode = 400 // Bad Request
      };
    }

    return null;
  }
}
using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Response;
using RendimentoPay.Core.Services.Interface;

namespace RendimentoPay.Core.Services.Services;

public class ContaCargaServiceDecorator : IContaCargaService
{
  private readonly IContaCargaService _contaCargaService;

  public ContaCargaServiceDecorator(IContaCargaService contaCargaService)
  {
    _contaCargaService = contaCargaService;
  }

  public async Task<ContaCargaPositivaResponse> Carga(ContaCargaPositivaRequest dados, AccessToken accessToken, bool boLocal = true)
  {
    return await _contaCargaService.Carga(dados, accessToken, boLocal);
  }

  public async Task<ContaConsultaResponse> Consulta(AccessToken accessToken)
  {
    return await _contaCargaService.Consulta(accessToken);
  }

  public async Task<ContaCargaNegativaResponse> Descarga(ContaCargaNegativaRequest dados, AccessToken accessToken, bool boLocal = true)
  {
    return await _contaCargaService.Descarga(dados, accessToken, boLocal);
  }

  public async Task<ContaSaldoResponse> Saldo(AccessToken accessToken, bool boLocal = true)
  {
    return await _contaCargaService.Saldo(accessToken, boLocal);
  }

  public async Task<ContaValidaExistenciaResponse> ValidaExistencia(ContaValidaExistenciaRequest dados, AccessToken accessToken, bool boLocal = true)
  {
    return await _contaCargaService.ValidaExistencia(dados, accessToken, boLocal);
  }
}

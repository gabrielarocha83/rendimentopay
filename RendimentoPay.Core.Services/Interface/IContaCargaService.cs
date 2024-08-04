using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Response;

namespace RendimentoPay.Core.Services.Interface;

public interface IContaCargaService
{
  Task<ContaCargaPositivaResponse> Carga(ContaCargaPositivaRequest dados, AccessToken accessToken, Boolean boLocal = true);
  Task<ContaCargaNegativaResponse> Descarga(ContaCargaNegativaRequest dados, AccessToken accessToken, Boolean boLocal = true);
  Task<ContaSaldoResponse> Saldo(AccessToken accessToken, Boolean boLocal = true);
  Task<ContaConsultaResponse> Consulta(AccessToken accessToken);
  Task<ContaValidaExistenciaResponse> ValidaExistencia(ContaValidaExistenciaRequest dados, AccessToken accessToken, Boolean boLocal = true);
}
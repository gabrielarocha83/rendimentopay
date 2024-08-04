using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Response;

namespace RendimentoPay.Core.Services.Interface;

public interface IOrdemPagamentoService
{
  Task<OrdemPagamentoResponse> EnviarOrdemPagamento(OrdemPagamentoRequest request, AccessToken accessToken);
  string GetAccessTokenAsync(AccessToken requestData);
}

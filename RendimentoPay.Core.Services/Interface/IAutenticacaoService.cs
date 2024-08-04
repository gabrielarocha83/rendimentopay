using RendimentoPay.Core.Domain.Request;
using RendimentoPay.Core.Domain.Response;

namespace RendimentoPay.Core.Services.Interface
{
    public interface IAutenticacaoService
    {        
        Task<AutenticacaoResponse> Autenticar(AutenticacaoRequest request, Domain.Request.AccessToken token);
    }
}
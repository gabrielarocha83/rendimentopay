using RendimentoPay.Core.Domain.Response.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RendimentoPay.Core.Domain.Response
{
  public class Mensagem
  {
    public Erromessage erroMessage { get; set; }
    public List<object> alertas { get; set; }
    public bool isSuccess { get; set; }
    public bool isFailure { get; set; }
  }
}

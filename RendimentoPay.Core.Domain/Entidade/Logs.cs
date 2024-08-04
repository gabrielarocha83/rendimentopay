using System.ComponentModel.DataAnnotations;
namespace RendimentoPay.Core.Domain.Request.Dominio;

public class Log
{
  [Key]
  public int Id { get; set; }

  public string Message { get; set; }

  public string MessageTemplate { get; set; }

  [StringLength(128)]
  public string Level { get; set; }

  [Required]
  public DateTimeOffset TimeStamp { get; set; }

  public string Exception { get; set; }

  public string Properties { get; set; }

  public string LogEvent { get; set; }
}
using System.ComponentModel.DataAnnotations;

public class LogEndPoint
{
    [Key]
    public int ID { get; set; }

    [Required]
    [StringLength(255)]
    public string Entidade { get; set; }

    [Required]
    [StringLength(50)]
    public string Metodo { get; set; }

    [Required]
    [StringLength(50)]
    public string Sentido { get; set; }

    [Required]
    [StringLength(255)]
    public string EndPoint { get; set; }

    [Required]
    [StringLength(20)]
    public string ISPBParticipante { get; set; }

    [Required]
    [StringLength(50)]
    public string TransactionID { get; set; }

    [Required]
    [StringLength(50)]
    public string ChaveId { get; set; }

    [Required]
    public DateTime DataHora { get; set; } = DateTime.Now;
}
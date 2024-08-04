using System.ComponentModel.DataAnnotations;

namespace RendimentoPay.Core.Domain.Request
{
    public class AccessTokenDICT
    {
        public string Authorization { get; set; }

        [Required]
        public string client_id { get; set; }

        [Required]
        public string client_secret { get; set; }

        [Required]
        public string access_token { get; set; }
    }
   
}
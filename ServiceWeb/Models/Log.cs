using SharedInfo.Messages;
using System.ComponentModel.DataAnnotations;

namespace ServiceWeb.Models
{
    public class Log
    {
        [Required]
        [Display(Name = "Type")]
        public MessageTypeEnum Type { get; set; }
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
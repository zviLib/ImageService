using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceWeb.Models
{
    

    public class Handler
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Path")]
        public string Path { get; set; }
    }
}
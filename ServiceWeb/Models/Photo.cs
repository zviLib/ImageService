using System.ComponentModel.DataAnnotations;

namespace ServiceWeb.Models
{
    public class Photo
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "PicPath")]
        public string PicPath { get; set; }
        [Required]
        [Display(Name = "ThumbPath")]
        public string ThumbPath { get; set; }
        [Required]
        [Display(Name = "Label")]
        public string Label { get; set; }
        [Required]
        [Display(Name = "TakenMonth")]
        public string TakenMonth { get; set; }
        [Required]
        [Display(Name = "TakenYear")]
        public string TakenYear { get; set; }
    }
}
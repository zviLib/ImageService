using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ServiceWeb.Models
{
    public class Photo
    {
        [Required]
        [Display(Name = "FullPath")]
        public string Path { get; set; }
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
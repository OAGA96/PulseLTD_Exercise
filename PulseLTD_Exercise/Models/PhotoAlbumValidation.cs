using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PulseLTD_Exercise.Models
{
    [MetadataType(typeof(PhotoAlbumMetadata))]
    public partial class PhotoAlbum
    {
    }

    public class PhotoAlbumMetadata
    {
        [ScaffoldColumn(false)]
        public object ImageId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [MinLength(4, ErrorMessage = "{0} must be between 4 and 100 characters.")]
        [MaxLength(100, ErrorMessage = "{0} must be between 4 and 100 characters.")]
        [Display(Name = "Image Title")]
        public object ImageTitle { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [MinLength(1, ErrorMessage = "{0} cannot be left empty.")]
        [Display(Name = "Image Name")]
        public object ImageName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Image")]
        public object ImageText { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "DateTime")]
        [DataType(DataType.DateTime)]
        public object ImageDateTime { get; set; }
    }
}
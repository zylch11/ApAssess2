using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApAssess2.Models {

    public class Category 
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order for Category must be greater than 0")]
        public int DisplayOrder { get; set; }
    }

}
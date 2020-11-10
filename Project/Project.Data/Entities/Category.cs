using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Data.Entities {
    public class Category {

        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Category Name should be 50 or less chracters")]
        public string Name { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}

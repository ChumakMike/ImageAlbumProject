using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace Project.Data.Entities {
    public class Image {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Caption { get; set; }

        [Required]
        public string Link { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public string UserRefId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

    }
}

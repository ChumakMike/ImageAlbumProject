using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project.Data.Entities {
    public class Rating {
        [Key]
        public int RatingId { get; set; }

        [Required]
        [ForeignKey("ImageId")]
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

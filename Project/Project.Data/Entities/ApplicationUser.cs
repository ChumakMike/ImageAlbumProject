using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Data.Entities {
    public class ApplicationUser : IdentityUser {
        public string FullName { get; set; }
        public string Bio { get; set; }

        [MaxLength(20, ErrorMessage = "Status should be 20 or less chracters")]
        public string Status { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

    }
}

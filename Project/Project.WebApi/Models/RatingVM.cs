using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebApi.Models {
    public class RatingVM {
        public int RatingId { get; set; }
        public int ImageId { get; set; }
        public string UserId { get; set; }
    }
}

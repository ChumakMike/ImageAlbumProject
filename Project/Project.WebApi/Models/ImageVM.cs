using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebApi.Models {
    public class ImageVM {
        public int ImageId { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Link { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserRefId { get; set; }
        public int CategoryId { get; set; }

        public int Rating { get; set; }
    }
}

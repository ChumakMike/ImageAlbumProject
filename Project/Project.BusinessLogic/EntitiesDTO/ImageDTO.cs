using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BusinessLogic.EntitiesDTO {
    public class ImageDTO {
        public int ImageId { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Link { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserRefId { get; set; }
        public int CategoryId { get; set; }
    }
}

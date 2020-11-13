using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebApi.Models {
    public class LoginModel {

        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Project.BusinessLogic.EntitiesDTO {
    public class UserDTO {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Status { get; set; }
    }
}

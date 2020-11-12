using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebApi.Helpers {
    public class ApplicationSettingsHelper {
        public string JWT_Secret { get; set; }
        public string Client_Url { get; set; }
        public string Issuer_Url { get; set; }
    }
}

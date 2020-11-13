using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BusinessLogic.Exceptions {
    public class SuchEntityExistsException : Exception {
        public SuchEntityExistsException(string entityType) : base($"Such entity of type: { entityType } exists!") {

        }
    }
}

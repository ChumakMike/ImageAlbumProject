using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BusinessLogic.Exceptions {
    public class NoSuchEntityException : Exception {
        public NoSuchEntityException(string entityType) : base($"No entity of type: { entityType } found!") {

        }
    }
}

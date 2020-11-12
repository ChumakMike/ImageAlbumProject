using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BusinessLogic.Exceptions {
    public class UserInvalidOperationException : Exception {
        public UserInvalidOperationException(string message) : base($"Error occured during user-managment operations: {message}") {

        }
    }
}

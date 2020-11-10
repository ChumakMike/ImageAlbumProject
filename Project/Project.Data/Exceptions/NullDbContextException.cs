using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Exceptions {
    public class NullDbContextException : Exception {
        public NullDbContextException(string message) : base(message) {

        }
    }
}

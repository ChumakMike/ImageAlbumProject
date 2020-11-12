using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BusinessLogic.Identity {
    public class ApplicationRoleManager : RoleManager<IdentityRole> {
        public ApplicationRoleManager(
            IRoleStore<IdentityRole> roleStore, 
            IEnumerable<IRoleValidator<IdentityRole>> roleValidators, 
            ILookupNormalizer lookupNormalizer, IdentityErrorDescriber identityErrorDescriber, 
            ILogger<RoleManager<IdentityRole>> logger) 
            : base(roleStore, roleValidators, lookupNormalizer, identityErrorDescriber, logger) {

        }
    }
}

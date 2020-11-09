using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BusinessLogic.Identity {
    public class ApplicationUserManager : UserManager<ApplicationUser> {
        public ApplicationUserManager(IUserStore<ApplicationUser> userStore,
            IOptions<IdentityOptions> options, IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer lookupNormalizer, IdentityErrorDescriber identityErrorDescriber,
            IServiceProvider serviceProvider, ILogger<UserManager<ApplicationUser>> logger)
            : base(userStore, options, passwordHasher, userValidators,
                  passwordValidators, lookupNormalizer, identityErrorDescriber, serviceProvider, logger) { }

    }
}

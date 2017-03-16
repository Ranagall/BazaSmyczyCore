using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace BazaSmyczy.UnitTests.TestHelpers
{
    public class SubstituteHelper
    {
        public static UserManager<TUser> SubstituteUserManager<TUser>() where TUser : class //Todo: create adapter for UserManager<T> ?
        {
            var store = Substitute.For<IUserStore<TUser>>();
            var options = Substitute.For<IOptions<IdentityOptions>>();
            var passHasher = Substitute.For<IPasswordHasher<TUser>>();
            var userValidators = Substitute.For<IEnumerable<IUserValidator<TUser>>>();
            var passValidators = Substitute.For<IEnumerable<IPasswordValidator<TUser>>>();
            var normalizer = Substitute.For<ILookupNormalizer>();
            var errorDescriber = Substitute.For<IdentityErrorDescriber>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            var logger = Substitute.For<ILogger<UserManager<TUser>>>();

            var userManager = Substitute.For<UserManager<TUser>>(store, options, passHasher, userValidators, passValidators, normalizer, errorDescriber, serviceProvider, logger);

            return userManager;
        }

        public static SignInManager<TUser> SubstituteSignManager<TUser>() where TUser : class //Todo: create adapter for SignInManager<T> ?
        {
            var userManager = SubstituteUserManager<TUser>();
            var contextAccessor = Substitute.For<IHttpContextAccessor>();
            var claimsPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<TUser>>();
            var options = Substitute.For<IOptions<IdentityOptions>>();
            var logger = Substitute.For<ILogger<SignInManager<TUser>>>();

            var signInManager = Substitute.For<SignInManager<TUser>>(userManager, contextAccessor, claimsPrincipalFactory, options, logger);

            return signInManager;
        }
    }
}

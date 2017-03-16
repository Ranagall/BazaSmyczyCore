using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Services;
using BazaSmyczy.UnitTests.TestHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BazaSmyczy.UnitTests.Services
{
    [TestFixture]
    public class AccountServiceTest
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;
        private ILogger<AccountService> _logger;
        private AccountService _service;
        private const string Username = "username";
        private const string Password = "password";
        private const string Email = "email";

        [SetUp]
        public void Init()
        {
            _userManager = SubstituteHelper.SubstituteUserManager<ApplicationUser>();
            _signInManager = SubstituteHelper.SubstituteSignManager<ApplicationUser>();
            _emailSender = Substitute.For<IEmailSender>();
            _logger = Substitute.For<ILogger<AccountService>>();
            _service = new AccountService(_userManager, _signInManager, _emailSender, _logger);
        }

        [TestCase("username", null)]
        [TestCase(null, "pass")]
        public void LogInAsync_UsernameOrPasswordAreNull_ShouldThrowArgumentNullException(string username, string password)
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), () => _service.LogInAsync(username, password, false));
        }

        [Test]
        public async Task LogInAsync_EmailIsNotConfirmed_ShouldntLogIn()
        {
            var user = new ApplicationUser();

            _userManager.FindByNameAsync(Arg.Is(Username)).Returns(user);
            _userManager.IsEmailConfirmedAsync(Arg.Is(user)).Returns(false);

            var result = await _service.LogInAsync(Username, Password, false);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsError);
            Assert.AreEqual("You must have a confirmed email to log in.", result.Errors.FirstOrDefault());
            await _signInManager.DidNotReceiveWithAnyArgs().SignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<bool>());
            await _signInManager.DidNotReceiveWithAnyArgs().PasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
            await _signInManager.DidNotReceiveWithAnyArgs().PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
            await _signInManager.DidNotReceiveWithAnyArgs().CheckPasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<bool>());
        }

        [Test]
        public async Task LogInAsync_SuccessfulLogIn_ShouldLogIn()
        {
            var user = new ApplicationUser();

            _userManager.FindByNameAsync(Arg.Is(Username)).Returns(user);
            _userManager.IsEmailConfirmedAsync(Arg.Is(user)).Returns(true);
            _signInManager.PasswordSignInAsync(Arg.Is(Username), Arg.Is(Password), Arg.Any<bool>(), Arg.Is(false)).Returns(SignInResult.Success);

            var result = await _service.LogInAsync(Username, Password, false);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsError);
            Assert.IsEmpty(result.Errors);
        }

        [TestCase(null, "password", "email")]
        [TestCase("username", null, "email")]
        [TestCase("username", "password", null)]
        public void RegisterAsync_UsernameOrPasswordOrEmailAreNull_ShouldThrowArgumentNullException(string username, string password, string email)
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), () => _service.RegisterAsync(username, password, email));
        }

        [Test]
        public async Task RegisterAsync_EmailExists_ShouldntRegister()
        {
            var user = new ApplicationUser();

            _userManager.FindByEmailAsync(Arg.Is(Email)).Returns(user);

            var result = await _service.RegisterAsync(Username, Password, Email);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsError);
            Assert.AreEqual("Email is already used.", result.Errors.FirstOrDefault());
        }

        [Test]
        public async Task RegisterAsync_ShouldRegister()
        {
            ApplicationUser createdUser = null;
            ApplicationUser nullUser = null;

            _userManager.FindByEmailAsync(Arg.Is(Email)).Returns(nullUser);
            _userManager.CreateAsync(Arg.Do<ApplicationUser>(arg=> createdUser = arg), Arg.Is(Password)).Returns(IdentityResult.Success);

            var result = await _service.RegisterAsync(Username, Password, Email);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsError);
            Assert.AreEqual(Username, createdUser.UserName);
            Assert.AreEqual(Email, createdUser.Email);
            await _userManager.Received().AddToRoleAsync(Arg.Is(createdUser), Arg.Is(Roles.Member));
        }

        [TestCase(null, "callback")]
        [TestCase("recipient", null)]
        public void SendConfirmationEmailAsync_RecipientOrCallbackUrlAreNull_ShouldThrowArgumentNullException(string recipient, string callbackUrl)
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), () => _service.SendConfirmationEmailAsync(recipient, callbackUrl));
        }

        [Test]
        public async Task SendConfirmationEmailAsync_ShouldReceiveCall()
        {
            var recipient = "email@host.dot";
            var callback = "callback";

            await _service.SendConfirmationEmailAsync(recipient, callback);

            //Assert
            await _emailSender.Received().SendAccountConfirmationEmailAsync(Arg.Is(recipient), Arg.Is(callback));
        }

        [Test]
        public async Task ConfirmEmailAsync_UserDoenstExist_ShouldntConfirm()
        {
            var userId = "123";
            var code = "code";
            ApplicationUser nullUser = null;

            _userManager.FindByIdAsync(Arg.Is(userId)).Returns(nullUser);

            var result = await _service.ConfirmEmailAsync(userId, code);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsError);
            await _userManager.DidNotReceiveWithAnyArgs().ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
        }

        [Test]
        public async Task ConfirmEmailAsync_CodeIsInvalid_ShouldntConfirm()
        {
            var userId = "123";
            var code = "invalidCode";

            _userManager.ConfirmEmailAsync(Arg.Any<ApplicationUser>(),Arg.Is(code)).Returns(IdentityResult.Failed());

            var result = await _service.ConfirmEmailAsync(userId, code);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsError);
            await _userManager.Received().ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Is(code));
        }

        [TestCase(null, "code")]
        [TestCase("userId", null)]
        public void ConfirmEmail_UserIdOrCodeAreNull_ShouldThrowArgumentNullException(string userId, string code)
        {
            Assert.ThrowsAsync(typeof(ArgumentNullException), () => _service.ConfirmEmailAsync(userId, code));
        }

        [Test]
        public async Task ConfirmEmailAsync_ShouldConfirm()
        {
            var userId = "123";
            var code = "code";
            var user = new ApplicationUser { Id = userId };

            _userManager.FindByIdAsync(Arg.Is(userId)).Returns(user);
            _userManager.ConfirmEmailAsync(Arg.Is(user), Arg.Is(code)).Returns(IdentityResult.Success);

            var result = await _service.ConfirmEmailAsync(userId, code);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsError);
            await _userManager.Received().ConfirmEmailAsync(Arg.Is(user), Arg.Is(code));
        }

        [Test]
        public async Task LogoutAsync_ShouldReceiveCall()
        {
            await _service.LogoutAsync();

            //Assert
            await _signInManager.Received().SignOutAsync();
        }

        [Test]
        public async Task GenerateEmailConfirmationTokenAsync_ShouldGenerateToken()
        {
            var token = "token";
            var user = new ApplicationUser();

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Is(user)).Returns(token);

            var result = await _service.GenerateEmailConfirmationTokenAsync(user);

            Assert.IsNotNull(result);
            Assert.AreEqual(token, result);
            await _userManager.Received().GenerateEmailConfirmationTokenAsync(Arg.Is(user));
        }
    }
}

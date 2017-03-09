using BazaSmyczy.Core.Services;
using NUnit.Framework;

namespace BazaSmyczy.UnitTests.Services
{
    [TestFixture]
    public class NotificationComposerTest
    {
        private NotificationComposer _composer;

        [SetUp]
        public void Init()
        {
            _composer = new NotificationComposer();
        }

        [Test]
        public void ComposeNotificationSubject_TypeIsConfirmation_ShouldComposeConfirmationSubject()
        {
            var type = NotificationType.Confirmation;

            var result = _composer.ComposeNotificationSubject(type);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual("Confirm your account", result);
        }

        [Test]
        public void ComposeNotificationBody_TypeIsConfirmation_ShouldComposeConfirmationBody()
        {
            var type = NotificationType.Confirmation;
            var callback = "callback";
            var expectedResult = $"Please confirm your account by clicking this: <a href='{callback}'>link</a>";

            var result = _composer.ComposeNotificationBody(type, callback);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(expectedResult, result);
        }
    }
}

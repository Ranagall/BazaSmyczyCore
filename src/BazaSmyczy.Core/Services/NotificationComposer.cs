namespace BazaSmyczy.Core.Services
{
    public class NotificationComposer : INotificationComposer
    {
        public string ComposeNotificationBody(NotificationType notificationType, string callbackUrl)
        {
            switch (notificationType)
            {
                case NotificationType.Confirmation:
                    return $"Please confirm your account by clicking this: <a href='{callbackUrl}'>link</a>";
            }

            return null;
        }

        public string ComposeNotificationSubject(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Confirmation:
                    return "Confirm your account";
            }

            return "Notification";
        }
    }
}

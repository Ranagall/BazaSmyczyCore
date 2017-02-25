namespace BazaSmyczy.Core.Services
{
    public interface INotificationComposer
    {
        string ComposeNotificationBody(NotificationType notificationType, string callbackUrl);
        string ComposeNotificationSubject(NotificationType notificationType);
    }
}

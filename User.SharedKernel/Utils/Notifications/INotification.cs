namespace User.SharedKernel.Utils.Notifications
{
    public interface INotification
    {
        void Add(string message);
        string GetNotifications();
        T AddWithReturn<T>(string message);
        bool Any();
        bool Notify(bool condition, string value);
    }
}

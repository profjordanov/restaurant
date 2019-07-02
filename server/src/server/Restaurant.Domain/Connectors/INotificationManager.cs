using System.Threading.Tasks;

namespace Restaurant.Domain.Connectors
{
    public interface INotificationManager
    {
        Task SendNotificationAsync(string email, string subject, string message);

        Task SendNotificationAsync(string receiver, string subject, string templateFileName, string[] templateParams);
    }
}
using TMonitBackend.Models;

namespace TMonitBackend.Services{
    interface IMessage{
        Task<bool> deliver(User whom, string subject, string content);
    }
}
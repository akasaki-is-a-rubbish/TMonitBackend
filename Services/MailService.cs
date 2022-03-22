using System.Net;
using System.Net.Mail;
using TMonitBackend.Models;

namespace TMonitBackend.Services
{

    public class EmailService : IMessage
    {
        SmtpClient client;
        public EmailService(string host, int port, string fromAddr, string displayName) :
        this(host, port, new MailAddress(fromAddr, displayName, System.Text.Encoding.UTF8))
        { }
        public EmailService(string host, int port, MailAddress from)
        {
            this.client = new SmtpClient(host, port);
        }
        public async Task<bool> deliver(string toAddr, string subject, string content)
        {
            // return deliverMail(toAddr, subject, body);
            //todo
            return true;
        }
        public Task<bool> deliver(User whom, string subject, string content){
            //check if user do have an verified email addr
            if(whom.Email != null || !whom.EmailConfirmed)
                throw new Exception("User has not verified email address");
            return deliver(whom.Email!, subject, content);
        }

    }
}
using AlexAPI.Library.Mail;

namespace AlexAPI.Services
{
    public interface IMailService
    {
        void SendEmailNow(Mail mail);
        void SendEmailDelayMinutes(Mail mail, int delay);
        void SendEmailDelayHours(Mail mail, int delay);
        void SendEmailDelayDays(Mail mail, int delay);
        void SendEmailAtDateTime(Mail mail, DateTime dateTime);
    }
}

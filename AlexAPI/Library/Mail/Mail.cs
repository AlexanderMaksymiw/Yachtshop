namespace AlexAPI.Library.Mail
{
    public class Mail
    {
        public Mail() { }

        public Mail(string ToEmail, string Subject, string Body, List<string> Attachments)
        {
            this.ToEmail = ToEmail;
            this.Subject = Subject;
            this.Body = Body;
            this.Attachments = Attachments;
        }

        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<string> Attachments { get; set; }
    }
}

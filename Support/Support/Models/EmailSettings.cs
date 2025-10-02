namespace Support.Models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string RecipientEmail { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderDisplayName { get; set; }
        public string CaptionName { get; set; }
        public string CaptionEmail { get; set; }
        public string CaptionSubject { get; set; }
        public string CaptionMessage { get; set; }
    }
}

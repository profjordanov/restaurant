namespace Restaurant.Domain.Settings
{
    public class MailSettings
    {
        public string MailServerAddress { get; set; }

        public string MailTemplateDirectoryPath { get; set; }

        public string MailSender { get; set; }

        public string MailServerUserName { get; set; }

        public string MailServerPassword { get; set; }

        public int MailServerPort { get; set; }

        public bool MailServerSecurityEnabled { get; set; }
    }
}
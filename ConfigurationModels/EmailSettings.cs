namespace BlogMVC.ConfigurationModels
{
    public class EmailSettings
    {
        public string? SmtpHost { get; set; }
        public int SmtpTlsPort { get; set; }
        public string? SmtpUserName { get; set; }
        public string? SmtpPassword { get; set; }
    }
}

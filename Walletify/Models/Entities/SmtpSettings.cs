namespace Walletify.Models.Entities
{
    public class SmtpSettings
    {
        public string Host { get; set; } = "live.smtp.mailtrap.io";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "api"; 
        public string Password { get; set; } = "07213ee07d247785f358e21c6721b03d"; 
        public bool EnableSsl { get; set; } = true;
    }
}

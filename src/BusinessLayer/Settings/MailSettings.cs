namespace BusinessLayer.Settings
{
    public class MailSettings
    {
        public string Mail { get; set; } = "thecuriousreaders123@gmail.com";
        public string DisplayName { get; set; } = "The Curious Readers";
        public string Password { get; set; } = "Thecurious123!";
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
    }
}

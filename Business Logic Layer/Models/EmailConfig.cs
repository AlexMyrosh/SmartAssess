namespace Business_Logic_Layer.Models
{
    public class EmailConfig
    {
        public string SMTPServer { get; set; }

        public int SMTPPort { get; set; }

        public string FromEmail { get; set; }

        public string FromPassword { get; set; }
    }
}

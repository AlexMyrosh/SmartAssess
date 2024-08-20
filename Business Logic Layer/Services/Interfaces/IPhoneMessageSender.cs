using Azure.Communication.Sms;

namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IPhoneMessageSender
    {
        public Task<SmsSendResult> SendSmsAsync(string phoneNumberTo, string message);

        public string GenerateVerificationCode();
    }
}

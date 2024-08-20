using Azure.Communication.Sms;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Business_Logic_Layer.Services.Implementations
{
    public class PhoneMessageSender : IPhoneMessageSender
    {
        private readonly CommunicationServiceConfig _emailConfig;

        public PhoneMessageSender(IOptions<CommunicationServiceConfig> communicationServiceConfig)
        {
            _emailConfig = communicationServiceConfig.Value;
        }

        public string GenerateVerificationCode()
        {
            Random random = new();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<SmsSendResult> SendSmsAsync(string phoneNumberTo, string message)
        {
            var smsClient = new SmsClient(_emailConfig.ConnectionString);
            var result = await smsClient.SendAsync(
                from: _emailConfig.PhoneNumberFrom,
                to: phoneNumberTo,
                message: message
            );

            return result;
        }
    }
}

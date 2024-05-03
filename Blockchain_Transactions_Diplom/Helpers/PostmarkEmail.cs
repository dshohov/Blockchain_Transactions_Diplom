using Blockchain_Transactions_Diplom.Helpers;
using Blockchain_Transactions_Diplom.Interfaces;
using Microsoft.Extensions.Options;
using PostmarkDotNet.Model;
using PostmarkDotNet;


namespace Blockchain_Transactions_Diplom.Services
{
    public class PostmarkEmail : IPostmarkEmail
    {

        public AuthMessageSenderOptions Options { get; set; }//API
        public PostmarkEmail(IOptions<AuthMessageSenderOptions> options)
        {
            Options = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.ApiKey))
            {
                throw new Exception("Null PostmarkKey");
            }
            await Execute(Options.ApiKey, subject, message, toEmail);
        }

        private async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new PostmarkClient(apiKey);
            var msg = new PostmarkMessage()
            {
                To = toEmail,
                From = "d.shokhov@student.csn.khai.edu",
                TrackOpens = true,
                Subject = subject,
                TextBody = message,
                HtmlBody = message,
                Tag = "Reset Password",               
                Headers = new HeaderCollection
                {

                }
            };
            var sendResult = await client.SendMessageAsync(msg);
            
            //_logger.LogInformation(sendResult.Status == PostmarkStatus.Success? $"Email to {toEmail} queued successfully!": $"Failure Email to {toEmail}");


        }
    }
}

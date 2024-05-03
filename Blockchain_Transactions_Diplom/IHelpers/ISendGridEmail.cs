namespace Blockchain_Transactions_Diplom.Interfaces
{
    public interface ISendGridEmail
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}

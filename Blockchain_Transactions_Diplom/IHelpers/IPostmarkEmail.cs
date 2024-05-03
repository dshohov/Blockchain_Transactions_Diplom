namespace Blockchain_Transactions_Diplom.Interfaces
{
    public interface IPostmarkEmail
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}

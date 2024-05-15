namespace Blockchain_Transactions_Diplom.IHelpers
{
    public interface ILiqPayInvoice
    {
        public Task SendInvoiceAsync(string toEmail, int countCoins, string orderId);
    }
}

namespace Blockchain_Transactions_Diplom.ViewModels
{
    public class TransactionCreateViewModel
    {
        public string FromPublicKey { get; set; }
        public string FromPrivateKey { get; set; }
        public string ToPublicKey { get; set; }
        public ulong Amount { get; set; }
    }
}

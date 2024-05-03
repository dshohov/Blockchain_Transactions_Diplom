using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

record Transaction(string From, string To, long Amount);
record TransactionBlock(Transaction Data, string Sign) : ISignedBlock<Transaction>
{

    public string PublicKey => Data.From;

}
 class CoinApp
{
    private readonly IEncryptor _encryptor;
    private readonly TypedBlockchain<TransactionBlock> _blockchain;
    public CoinApp()
    {
        _encryptor = new RSAEncryptor();

        _blockchain = new TypedBlockchain<TransactionBlock>(
            new Blockchain(new CRC32Hash()),
            new SignedRule<TransactionBlock,Transaction>(_encryptor),
            new AmountRule()
            );
    }
    public long GetBalanceUser(KeyPair user)
    {
        return _blockchain.GetBalance(user);
    }
    public IEnumerable<TypedBlock<TransactionBlock>> GetTransactions(KeyPair keyPair)
    {
        return _blockchain.GetTransactions(keyPair);
    }
    private void AddTransaction(TransactionBlock  block)
    {
        
        _blockchain.AddBlock(block); 
    }
    public void PerformTransaction(KeyPair from, string toPublicKey, long amount)
    {
        var transaction = new Transaction(from.Publickey, toPublicKey, amount);
        var transactionString = JsonSerializer.Serialize(transaction);
        var sign = _encryptor.Sign(from.PrivateKey, transactionString);
        var transactionBlock = new TransactionBlock(transaction,sign);
        AddTransaction(transactionBlock);
    }

   
}
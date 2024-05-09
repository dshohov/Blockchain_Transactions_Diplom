using System.Text.Json;

interface ISignedBlock<TData>
{
    TData Data { get; }
    string PublicKey { get; }
    string Sign { get; }
}
class SignedRule<TBlock, TData> : IRule<TBlock> where TBlock : ISignedBlock<TData>
{
    private readonly IEncryptor _encryptor;
    public SignedRule(IEncryptor encryptor)
    {
        _encryptor = encryptor;
    }
    public void Execute(IEnumerable<TypedBlock<TBlock>> _, TypedBlock<TBlock> newBlock)
    {
        var dataToSign = JsonSerializer.Serialize(newBlock.Data.Data);
        if (!_encryptor.VerifySign(newBlock.Data.PublicKey, dataToSign, newBlock.Data.Sign))
            throw new ApplicationException("Block is signed incorrectly");
       

    }

    public void ExecuteSuperAdmin(IEnumerable<TypedBlock<TBlock>> _, TypedBlock<TBlock> newBlock)
    {
        var dataToSign = JsonSerializer.Serialize(newBlock.Data.Data);
        if (!_encryptor.VerifySign(newBlock.Data.PublicKey, dataToSign, newBlock.Data.Sign))
            throw new ApplicationException("Block is signed incorrectly");

    }
}
    

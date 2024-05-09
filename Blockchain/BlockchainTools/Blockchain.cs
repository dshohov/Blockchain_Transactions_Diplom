
using System.Collections;
using System.Runtime.CompilerServices;

class Blockchain : IEnumerable<Block>
{
    private readonly IHashFunction _hashFunction;
    private readonly BlockchainBuilder _blockchainBuilder;
    private readonly List<Block> _bloks = new();

    public Blockchain(IHashFunction hashFunction)
    {
        _blockchainBuilder = new BlockchainBuilder(hashFunction, null);
        _hashFunction = hashFunction;
    }

    public Block BuildBlock(string data)
    {
        
        var block = _blockchainBuilder.AddBlock(data);
        return block;
    }

    public bool AddBlock(Block block)
    {
        var tail = _bloks.LastOrDefault();
        if (block.ParentHash == tail?.Hash)
        {
            var expectedHash = _hashFunction.GetHash(block.ParentHash + block.Data);
            if (expectedHash == block.Hash)
            {
                _bloks.Add(block);
                return true;
            }
            else
            {
                //throw new ApplicationException("Block Hash is invalid");
                return false;
            }

        }
        else
            //throw new ApplicationException($"{block.Hash} is incorrect. Becouse of parent hash. It should be {tail.Hash} but recived {block.ParentHash}");
            return false;
    }

    public IEnumerator<Block> GetEnumerator()
    {
        return _bloks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

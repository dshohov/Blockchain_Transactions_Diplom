using System.Text.Json;

record TypedBlock<T>(string Hash, string ParebtHash, string RawData, T Data)
{
    public static TypedBlock<T> FromLowLevel(Block block)
    {
        return new TypedBlock<T>(block.Hash,block.ParentHash,block.Data, JsonSerializer.Deserialize<T>(block.Data));
    }
}

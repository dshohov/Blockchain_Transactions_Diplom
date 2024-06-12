using System.Text.Json;

record TypedBlock<T>(string Hash, string ParebtHash, string RawData, T Data)
{
    public static TypedBlock<T> FromLowLevel(Block block)
    {
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
        return new TypedBlock<T>(block.Hash,block.ParentHash,block.Data, JsonSerializer.Deserialize<T>(block.Data));
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
    }
}

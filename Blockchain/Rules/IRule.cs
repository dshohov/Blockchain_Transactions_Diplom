interface IRule<T>
{
    void Execute(IEnumerable<TypedBlock<T>> previousBlocks,TypedBlock<T> nextBlock);
}

interface IRule<T>
{
    void Execute(IEnumerable<TypedBlock<T>> previousBlocks,TypedBlock<T> nextBlock);
    void ExecuteSuperAdmin(IEnumerable<TypedBlock<T>> previousBlocks, TypedBlock<T> nextBlock);
}

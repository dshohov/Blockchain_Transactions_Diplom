class AmountRule : IRule<TransactionBlock>
{
    
    public void Execute(IEnumerable<TypedBlock<TransactionBlock>> previousBlocks, TypedBlock<TransactionBlock> newBlock)
    {
        long balance = 50;
        var currentUser = newBlock.Data.Data.From;
        foreach (var block in previousBlocks)
        {
            if (block.Data.Data.From == currentUser)
                balance -= block.Data.Data.Amount;
            else if (block.Data.Data.To == currentUser)
                balance += block.Data.Data.Amount;
        }
        
        if(newBlock.Data.Data.To == newBlock.Data.Data.From && newBlock.Data.Data.Amount == 0)
        {
            Console.WriteLine("Balance: " + balance);
        }
          
        
        if (balance < newBlock.Data.Data.Amount)
            throw new ApplicationException("User has not enoght funds. Balance is " + balance);
    }
   
}

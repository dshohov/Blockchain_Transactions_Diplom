﻿using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

class TypedBlockchain<T> : IEnumerable<TypedBlock<T>>
{
    private readonly Blockchain _blockchain;
    private readonly IRule<T>[] _rules;
    public TypedBlockchain(Blockchain blockchain, params IRule<T>[] rules)
    {
        _blockchain = blockchain;
        _rules = rules;
    }



    public void AddBlock(T data)
    {
        var raw = JsonSerializer.Serialize(data);
        var lowBlock = _blockchain.BuildBlock(raw);
        var block = TypedBlock<T>.FromLowLevel(lowBlock);

        foreach (var rule in _rules)
        {
            rule.Execute(this,block);
        }        
        _blockchain.AddBlock(new Block(block.ParebtHash, block.RawData, block.Hash));
    }

    public long GetBalance(KeyPair user)
    {
        var previousBlocks = this.Cast<TypedBlock<TransactionBlock>>();
        long balance = 50;
        var currentUser = user.Publickey;
        foreach (var block in previousBlocks)
        {
            if (block.Data.Data.From == currentUser)
                balance -= block.Data.Data.Amount;
            else if (block.Data.Data.To == currentUser)
                balance += block.Data.Data.Amount;
        }
        return balance;     
    }
    public IEnumerable<TypedBlock<TransactionBlock>> GetTransactions(KeyPair user)
    {
        var previousBlocks = this.Cast<TypedBlock<TransactionBlock>>();
        long balance = 50;
        var currentUser = user.Publickey;

        foreach (var block in previousBlocks)
        {
            if (block.Data.Data.From == currentUser)
                balance -= block.Data.Data.Amount;
            else if (block.Data.Data.To == currentUser)
                balance += block.Data.Data.Amount;

            yield return block;
        }
    }



    public IEnumerator<TypedBlock<T>> GetEnumerator()
    {
        return _blockchain.Select(x => TypedBlock<T>.FromLowLevel(x)) 
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

   
}


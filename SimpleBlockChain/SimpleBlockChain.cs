namespace SimpleBlockChain
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class SimpleBlockChain
    {
        private int transactionCountPerBlock;

        private BlockingCollection<Block> blocks;

        private ConcurrentQueue<Transaction> transactions;


        public SimpleBlockChain(int maxBlocks, int transactionCountPerBlock)
        {
            this.transactionCountPerBlock = transactionCountPerBlock;
            this.blocks = new BlockingCollection<Block>(maxBlocks);
            this.transactions = new ConcurrentQueue<Transaction>();

            Task.Factory.StartNew(this.Assemble, TaskCreationOptions.LongRunning);
        }

        public void Add(string sender, string receiver, double amount)
        {
            this.transactions.Enqueue(new Transaction() { Sender = sender, Receiver = receiver, Amount = amount });
        }

        public bool IsValid()
        {
            for (int i = 1; i < this.blocks.Count; i++)
            {
                if (this.blocks.ElementAt(i).CurHash != this.blocks.ElementAt(i).Hash())
                {
                    Console.WriteLine(this.blocks.ElementAt(i).Hash());
                    Console.WriteLine(this.blocks.ElementAt(i).CurHash);
                    Console.WriteLine(this.blocks.ElementAt(i).Hash());
                    return false;
                }

                if (this.blocks.ElementAt(i).PrevHash != this.blocks.ElementAt(i -1).Hash())
                {
                    Console.WriteLine(this.blocks.ElementAt(i).PrevHash);
                    Console.WriteLine(this.blocks.ElementAt(i - 1).Hash());
                    return false;
                }

                
            }
            return true;
        }


        public void Assemble()
        {
            // Frist block
            var block = new Block()
                            {
                                Index = 0,
                                TimeStamp = DateTime.UtcNow,
                                PrevHash = "0",
                                Transactions = new BlockingCollection<Transaction>(this.transactionCountPerBlock)
                            };

            while (true)
            {
                try
                {
                    Transaction transaction;
                    if (this.transactions.TryDequeue(out transaction))
                    {
                        var addSuccess = block.Transactions.TryAdd(transaction);
                        if (!addSuccess || block.Transactions.Count == this.transactionCountPerBlock)
                        {
                            block.CurHash = block.Hash();
                            var prevHash = block.CurHash;
                            var prevIndex = block.Index;
                            blocks.Add(block);
                            block = new Block()
                                        {
                                            Index = ++prevIndex,
                                            TimeStamp = DateTime.UtcNow,
                                            PrevHash = prevHash,
                                            Transactions = new BlockingCollection<Transaction>(this.transactionCountPerBlock)
                                        };
                            if (!addSuccess)
                            {
                                block.Transactions.Add(transaction);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception)
                {
                    // Ignore, no exception expected here.
                }
            }
        }

        public override string ToString()
        {
            var json = JsonConvert.SerializeObject(this.blocks);
            return json;
        }
    }

    
}

namespace SimpleBlockChain
{
    using System;
    using System.Collections.Concurrent;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    class Block
    {
        public long Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PrevHash { get; set; }
        public string CurHash { get; set; }
        public BlockingCollection<Transaction> Transactions { get; set; }

        public string Hash()
        {
            SHA256 sha256 = SHA256.Create();
            var bytes = Encoding.ASCII.GetBytes(this.ToString());
            return BitConverter.ToString(sha256.ComputeHash(bytes));
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Index);
            stringBuilder.Append(TimeStamp);
            stringBuilder.Append(PrevHash);
            foreach (var transaction in Transactions)
            {
                stringBuilder.Append(transaction);
            }
            return stringBuilder.ToString();
        }
    }
}

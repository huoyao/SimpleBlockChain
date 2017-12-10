namespace SimpleBlockChain
{
    using System.Text;

    public class Transaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public double Amount { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Sender);
            stringBuilder.Append(Receiver);
            stringBuilder.Append(Amount);
            return stringBuilder.ToString();
        }
    }
}

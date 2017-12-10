using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleBlockVChain.Test
{
    using System.Threading;

    using Newtonsoft.Json.Linq;

    using SimpleBlockChain;

    [TestClass]
    public class SimpleBlockChainTest
    {
        [TestMethod]
        public void TestBlockChain()
        {
            var blockChain = new SimpleBlockChain(100, 1);

            blockChain.Add("tom", "jim", 1);
            blockChain.Add("tom", "jim", 2);
            blockChain.Add("bob", "alice", 3);

            Thread.Sleep(1000);
            Console.WriteLine(blockChain.ToString());
            Assert.IsTrue(blockChain.IsValid());
        }
    }
}

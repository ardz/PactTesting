using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsumerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string foo = "hello";

            Console.Write(foo);

            var bar = new Guid();

            var s = bar.ToString();

            var charEnumerator = s.GetEnumerator();

            charEnumerator.Dispose();
        }
    }
}

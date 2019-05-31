using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NecControllerTest
{

    [TestClass]
    public class UtilsSuite
    {

        private int VALID_PORT = 482;
        private int INVALID_PORT = -3;

        private string VALID_HOST = "192.503.492.402";
        private string INVALID_HOST = "192.503.49.";

        private string PINGABLE_HOST = "127.0.0.1";
        private string UNREACHABLE_HOST = "128.0.0.1";

        [TestMethod]
        public void CheckChcecksumCounterTest()
        {
            byte[] request = { 2, 0, 0, 3, 5, 1, 3, 0 };
            Assert.AreEqual(Utils.Utils.CountChecksum(request), 14);
        }

        [TestMethod]
        public void ValidateValidPortTest()
        {
            Assert.IsTrue(Utils.Utils.IsPortVaild(VALID_PORT));
        }

        [TestMethod]
        public void ValidateInvalidPortTest()
        {
            Assert.IsFalse(Utils.Utils.IsPortVaild(INVALID_PORT));
        }

        [TestMethod]
        public void ValidateValidHostTest()
        {
            Assert.IsTrue(Utils.Utils.IsAddressValid(VALID_HOST));
        }

        [TestMethod]
        public void ValidateInvalidHostTest()
        {
            Assert.IsFalse(Utils.Utils.IsAddressValid(INVALID_HOST));
        }

        [TestMethod]
        public void PingValidHostTest()
        {
            Assert.IsTrue(Utils.Utils.Ping(PINGABLE_HOST));
        }

        [TestMethod]
        public void PingInvalidHostTest()
        {
            Assert.IsFalse(Utils.Utils.Ping(UNREACHABLE_HOST));
        }
    }
}

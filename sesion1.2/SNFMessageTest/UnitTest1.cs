using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class SNFMessageTests
{
    [TestMethod]
    public void TestEncodeDataMessage()
    {
        var message = new SNFMessage { SequenceNumber = 1, Data = 42 };
        var encoded = message.Encode();
        Assert.AreEqual(2, encoded.Length);
        Assert.AreEqual(1, encoded[0]);
        Assert.AreEqual(42, encoded[1]);
    }

    [TestMethod]
    public void TestEncodeAckMessage()
    {
        var message = new SNFMessage { SequenceNumber = 1, IsAck = true };
        var encoded = message.Encode();
        Assert.AreEqual(2, encoded.Length);
        Assert.AreEqual(1, encoded[0]);
        Assert.AreEqual(0xFF, encoded[1]);
    }

    [TestMethod]
    public void TestDecodeDataMessage()
    {
        var decoded = SNFMessage.Decode(new byte[] { 1, 42 });
        Assert.AreEqual(1, decoded.SequenceNumber);
        Assert.AreEqual(42, decoded.Data);
        Assert.IsFalse(decoded.IsAck);
    }

    [TestMethod]
    public void TestDecodeAckMessage()
    {
        var decoded = SNFMessage.Decode(new byte[] { 1, 0xFF });
        Assert.AreEqual(1, decoded.SequenceNumber);
        Assert.IsNull(decoded.Data);
        Assert.IsTrue(decoded.IsAck);
    }
}


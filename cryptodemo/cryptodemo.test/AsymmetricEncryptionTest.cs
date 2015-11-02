using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Cryptodemo;

namespace Cryptodemo.Test
{
    [TestClass]
    public class AsymmetricEncryptionTest
    {
        [TestMethod]
        public void KeyPairDecrypt()
        {
            KeyPair kp = new KeyPair();
            string publicKey = kp.PublicKey;
            StringReader sr = new StringReader(publicKey);
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            RSAParameters pubKey = (RSAParameters)xs.Deserialize(sr);

            string textToEncrypt = "The limits of my language means the limits of my world.";
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);
            string encryptedText = Convert.ToBase64String(csp.Encrypt(Encoding.Unicode.GetBytes(textToEncrypt), false));

            string decryptedText = kp.DecryptBase64String(encryptedText);
            Assert.AreEqual(textToEncrypt, decryptedText);
        }

        [TestMethod]
        public void ByteStringEncryptor()
        {
            KeyPair kp = new KeyPair();
            string publicKey = kp.PublicKey;

            string textToEncrypt = "Colorless green ideas sleep furiously.";
            BytestringEncryptor bse = new BytestringEncryptor(publicKey);
            string encryptedText = bse.EncryptString(textToEncrypt);
            string decryptedText = kp.DecryptBase64String(encryptedText);
            Assert.AreEqual(textToEncrypt, decryptedText);
        }

        [TestMethod]
        public void HaveAConversation()
        {
            Actor alice = new Actor("Alice");
            Actor bob = new Actor("Bob");

            Tuple<string, string> conversation = bob.SaySomethingTo(alice);
            Assert.IsTrue(conversation.Item2.Contains(conversation.Item1));

            Debug.WriteLine("Bob said: " + conversation.Item1);
            Debug.WriteLine("Alice responded: " + conversation.Item2);
        }

    }
}

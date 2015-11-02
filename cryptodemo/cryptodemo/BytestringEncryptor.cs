using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;


namespace Cryptodemo
{
    public class BytestringEncryptor
    {
        private RSACryptoServiceProvider _csp;
        private string _publicKeyToEncryptWith;

        public BytestringEncryptor(string publicKeyToEncryptWith)
        {
            _publicKeyToEncryptWith = publicKeyToEncryptWith;
            _csp = null;
        }

        public string EncryptString(string textToEncrypt)
        {
            byte[] bytesToEncyrpt = Encoding.Unicode.GetBytes(textToEncrypt);
            byte[] encryptedBytes = EncryptBytes(bytesToEncyrpt);
            return Convert.ToBase64String(encryptedBytes);
        }

        public byte[] EncryptBytes(byte[] bytesToEncrypt)
        {
            if (_csp == null)
                InitCSP();

            return _csp.Encrypt(bytesToEncrypt, false);
        }

        private void InitCSP()
        {
            StringReader sr = new StringReader(_publicKeyToEncryptWith);
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            RSAParameters pubKey = (RSAParameters)xs.Deserialize(sr);

            _csp = new RSACryptoServiceProvider();
            _csp.ImportParameters(pubKey);
        }
    }
}

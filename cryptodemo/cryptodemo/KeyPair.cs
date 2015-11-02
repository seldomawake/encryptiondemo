using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace Cryptodemo
{
    public class KeyPair
    {
        public static readonly int BITS_IN_KEY_PAIR = 2048;
        private RSACryptoServiceProvider _cryptKeeper;

        private RSAParameters _publicKey;
        private string _publicKeyStringCache;

        //RSACryptoServiceProviders can also export private keys, but we don't need those shared for this example.
        //That looks like: RSAParameters _privateKey; _privateKey = _cyrptKeeper.ExportParameters(true);

        public string PublicKey
        {
            get
            {
                if(_publicKeyStringCache == null)
                {
                    StringWriter sw = new StringWriter();
                    XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
                    xs.Serialize(sw, _publicKey);
                    _publicKeyStringCache = sw.ToString();
                }
                return _publicKeyStringCache;
            }
        }

        public KeyPair()
        {
            _publicKeyStringCache = null;
            _cryptKeeper = new RSACryptoServiceProvider(BITS_IN_KEY_PAIR);
            _publicKey = _cryptKeeper.ExportParameters(false);
        }

        //Here's why we only use base-64 strings to represent byte data: 
        //http://haacked.com/archive/2012/01/30/hazards-of-converting-binary-data-to-a-string.aspx/
        public string DecryptBase64String(string encryptedString)
        {
            byte[] decrypted = _cryptKeeper.Decrypt(Convert.FromBase64String(encryptedString), false);
            string decryptedString = Encoding.Unicode.GetString(decrypted);
            return decryptedString;
        }
    }
}

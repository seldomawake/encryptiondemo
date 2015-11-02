using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptodemo
{
    public class Actor
    {
        private static List<string> sharedWisdom = new List<string>()
        {
            "I doubt, therefore I might be.",
            "Resist the temptation of hitting on an answer to the question how we can define such-and-such a notion, an answer which supplies a smooth and elegant definition which entirely ignores the purpose which we originially wanted the notion for.",
            "Is it solipsistic in here, or is it just me?",
            "What better way to control a man than by giving him the vote and telling him he’s free?",
            "We do not see things as they are. We see things as we are.",
            "Freedom is what you do with what's been done to you.",
            "A witty saying proves nothing.",
            "It is only as an aesthetic phenomenon that existence and the world are eternally justified.",
            "All existing things are born for no reason, continue through weakness and die by accident",
            "It is meaningless that we are born; it is meaningless that we die.",
            "Convictions are more dangerous enemies of truths than lies.",
            "Wisest is he who knows he does not know.",
            "Whereof one cannot speak, thereof one must be silent.",
        };

        private List<KeyPair> _keyChain;
        private string _name;

        public string Name { get { return _name; } }
        
        public Actor(string name)
        {
            _name = name;
            _keyChain = new List<KeyPair>();
            AddKeyToKeychain();
        }

        private void AddKeyToKeychain()
        {
            _keyChain.Add(new KeyPair());
        }

        public string RequestPublicKey()
        {
            if (_keyChain == null || _keyChain.Count() <= 0)
                AddKeyToKeychain();

            return _keyChain.First().PublicKey;
        }

        private KeyPair WhichKey(string publicKey)
        {
            return _keyChain.Where(x => x.PublicKey == publicKey).FirstOrDefault();
        }

        private string DecryptMessage(string message, string publicKey)
        {
            KeyPair key = WhichKey(publicKey);
            if (key == null)
                return null;

            return key.DecryptBase64String(message);
        }

        public string SayItBackToMe(string message, string publicKey)
        {
            return String.Format("I heard, '{0}'", DecryptMessage(message, publicKey));
        }

        public Tuple<string, string> SaySomethingTo(Actor a)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            string whatISaid = sharedWisdom[r.Next(0, sharedWisdom.Count())];

            string theirPublicKey = a.RequestPublicKey();
            string howIWillSayIt = new BytestringEncryptor(theirPublicKey).EncryptString(whatISaid);
            string whatTheyHeard = a.SayItBackToMe(howIWillSayIt, theirPublicKey);

            return new Tuple<string, string>(whatISaid, whatTheyHeard);
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace Task3.RockPaperScissors
{
    public class Generation
    {
        private byte[] _key;
        public HMAC HMAC{get;set;}
        public byte[] Key {get {return _key;} set
        {
            byte[] key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key, 0, key.Length);
           _key = key;

        }}
     
        private HMAC GetHMAC(byte[] key)
            => new HMACSHA256(key);
        private  byte[] CreateTheKey()
        {
            byte[] key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key, 0, key.Length);
            return key;
        }
         public byte[] GenerateHMACHash(string move) 
        {
            if (_key is null)
                _key = CreateTheKey();
            if (HMAC is null)
                HMAC = GetHMAC(_key);
                
            var moveAsBytes = Encoding.ASCII.GetBytes(move);
            var resultHMAC = HMAC.ComputeHash(moveAsBytes);
            System.Console.WriteLine("HMAC: {0}", HMAC.Hash.BytesAsString());
            return resultHMAC;
        }
       
        
    }
}
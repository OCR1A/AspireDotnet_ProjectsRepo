using System.Runtime.Serialization;
using HashidsNet;

namespace HashIdManager.Services
{

    public class HashIdService
    {

        private readonly Hashids _hashids;
        public HashIdService()
        {
            _hashids = new Hashids("super-ultra-secret-salt", 8);
        }

        public string Encode(int id) => _hashids.Encode(id);

        public int? Decode(string encodedId)
        {
            var result = _hashids.Decode(encodedId);
            return result.Length > 0 ? result[0] : null;
        }


    }

}
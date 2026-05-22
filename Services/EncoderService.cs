using BC = BCrypt.Net.BCrypt;

namespace Jornadas_Metalurgia_2026.Services
{
    public interface IEncoderServices
    {
        string Encode(string value);
        bool Verify(string value, string hash);
    }
    public class EncoderService : IEncoderServices
    {
        public string Encode(string value)
        {
            var salt = BC.GenerateSalt(13);
            string encoded = BC.HashPassword(value, salt);
            return encoded;
        }

        public bool Verify(string value, string hash)
        {
            bool matched = BC.Verify(value, hash);
            return matched;
        }
    }
}

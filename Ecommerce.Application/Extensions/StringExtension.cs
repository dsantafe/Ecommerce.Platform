using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.Application.Extensions
{
    public static class StringExtension
    {
        public static Guid ToGuid(this string code)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(code);
            byte[] hashBytes = MD5.HashData(inputBytes);

            return new Guid(hashBytes);
        }
    }
}

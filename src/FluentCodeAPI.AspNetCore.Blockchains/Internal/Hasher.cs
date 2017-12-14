using System;
using System.Security.Cryptography;
using System.Text;

namespace FluentCodeAPI.AspNetCore.Blockchains.Internal
{
    /// <summary>
    /// Represents a <see cref="Hasher"/> tool used for simple cryptography.
    /// </summary>
    public class Hasher
    {
        /// <summary>
        /// Compute the hash value for the specified <see cref="String"/> with SHA-256.
        /// </summary>
        /// <param name="value">The value to compute</param>
        /// <returns>The hashed value as a <see cref="String"/> in hexadecimal</returns>
        /// <exception cref="ArgumentNullException">Thrown if the argument is null or empty.</exception>
        public static string ComputeSHA256(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var result = string.Empty;

            using (var sha = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                var datas = sha.ComputeHash(buffer);

                for (var i = 0; i < datas.Length; i++)
                {
                    result += datas[i].ToString("X2");
                }
            }

            return result;
        }
    }
}
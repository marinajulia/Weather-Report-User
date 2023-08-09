using System.Security.Cryptography;
using System.Text;

namespace User.Domain.Common.Cryptography
{
    public class PasswordService
    {
        private const string EncryptionKey = "teste"; //colocar no appsettings?

        public static string Encrypt(string password)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
                byte[] iv = new byte[aesAlg.BlockSize / 8];

                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public static string Decrypt(string encryptedPassword)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
                byte[] iv = new byte[aesAlg.BlockSize / 8];

                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}

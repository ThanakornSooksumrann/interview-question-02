using System.Security.Cryptography;
using System.Text;

namespace Example.API.Common.Utilities;
public class EncryptUtil
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(EncryptUtil));

    public static string DecryptAES128(string encryptText, string aesKey)
    {
        byte[] keyAndIvBytes = Encoding.UTF8.GetBytes(aesKey);
        byte[] encryptedData = Convert.FromBase64String(encryptText);

        string? plaintext = null;
        // Create Aes    
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            // Create a decryptor    
            ICryptoTransform decryptor = aes.CreateDecryptor(keyAndIvBytes, keyAndIvBytes);
            // Create the streams used for decryption.    
            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                // Create crypto stream    
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    // Read crypto stream    
                    using (StreamReader reader = new StreamReader(cs))
                        plaintext = reader.ReadToEnd();
                }
            }
        }
        return plaintext;
    }

    public static string EncryptAES128(string inputText, string aesKey)
    {
        byte[] keyAndIvBytes = Encoding.UTF8.GetBytes(aesKey);
        byte[] plainText = Encoding.UTF8.GetBytes(inputText);

        byte[] encrypted;
        // Create a new AesManaged.    
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            // Create encryptor    
            ICryptoTransform encryptor = aes.CreateEncryptor(keyAndIvBytes, keyAndIvBytes);
            // Create MemoryStream    
            using (MemoryStream ms = new MemoryStream())
            {
                // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                // to encrypt    
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    // Create StreamWriter and write data to a stream    
                    // using (StreamWriter sw = new StreamWriter(cs))
                    //     sw.Write(plainText);
                    cs.Write(plainText, 0, plainText.Length);
                    cs.FlushFinalBlock();
                    encrypted = ms.ToArray();
                }
            }
        }
        // Return encrypted data    
        return Convert.ToBase64String(encrypted);
    }
    
    public static string ComputeSha256Hash(string rawData)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(rawData);
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
    
    public static bool VerifySha256Hash(string rawData, string hash)
    {
        string computedHash = ComputeSha256Hash(rawData);
        return StringComparer.OrdinalIgnoreCase.Compare(computedHash, hash) == 0;
    }
    
    public static (string rawToken, string tokenHash) GenerateToken(int lengthBytes = 32)
    {
        byte[] tokenData = new byte[lengthBytes];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenData);
        }
        
        string rawToken = Convert.ToBase64String(tokenData)
            .TrimEnd('=') 
            .Replace('+', '-')
            .Replace('/', '_');
        
        string tokenHash = ComputeSha256Hash(rawToken);

        return (rawToken, tokenHash);
    }
}

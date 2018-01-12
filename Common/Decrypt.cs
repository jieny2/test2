using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Common
{
    public class Decrypt
    {
        /// <summary>
        /// 用自定义的密钥进行DES解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string cipherText, string key)
        {
            DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();
            int len = cipherText.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(cipherText.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            desCSP.Key = ASCIIEncoding.ASCII.GetBytes(Encrypt.MD5Encrypt(key).Substring(0, 8));
            desCSP.IV = ASCIIEncoding.ASCII.GetBytes(Encrypt.MD5Encrypt(key).Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desCSP.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}

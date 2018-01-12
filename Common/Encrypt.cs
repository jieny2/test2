using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;

namespace Common
{
    public class Encrypt
    {
        public static string key = Decrypt.DESDecrypt(ConfigurationManager.AppSettings["D"], "xyb"); // 加密密钥

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string MD5Encrypt(string plainText)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);
            var res = new MD5CryptoServiceProvider().ComputeHash(inputByteArray);
            string cipherText = Tools.ByteArrayToHexString(res, true);

            return cipherText;
        }

        /// <summary>
        /// MD5加密（获取文件的MD5）
        /// </summary>
        /// <param name="plainText">流对象</param>
        /// <param name="isUpperCase">是否大写</param>
        /// <returns>密文</returns>
        public static string MD5Encrypt(Stream plainText, bool isUpperCase)
        {
            var res = new MD5CryptoServiceProvider().ComputeHash(plainText);

            return Tools.ByteArrayToHexString(res, isUpperCase);
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文（字节数组形式）</returns>
        public static byte[] SHA1Encrypt(string plainText)
        {
            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] hashBytes = ue.GetBytes(plainText);
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] cipherTextByteArray = sha1.ComputeHash(hashBytes);

            return cipherTextByteArray;
        }

        /// <summary>
        /// 用自定义的密钥进行DES加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string plainText, string key)
        {
            DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);
            desCSP.Key = ASCIIEncoding.ASCII.GetBytes(Encrypt.MD5Encrypt(key).Substring(0, 8));
            desCSP.IV = ASCIIEncoding.ASCII.GetBytes(Encrypt.MD5Encrypt(key).Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desCSP.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder cipherText = new StringBuilder();
            foreach (byte item in ms.ToArray())
            {
                cipherText.AppendFormat("{0:X2}", item);
            }

            return cipherText.ToString();
        }

        /// <summary>
        /// 用配置里的密钥进行DES加密
        /// </summary>
        /// <param name="plainText">明文 </param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string plainText)
        {
            return DESEncrypt(plainText, key);
        }
    }
}

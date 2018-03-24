using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web.Security;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public sealed class EncryptHelper
    {
        /// <summary>
        /// DES加解密密钥
        /// </summary>
        private static string _DesKey = "weplat!@";

        #region DES加解密
        /// <summary>
        /// 进行DES加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="key">密钥，且必须为8位</param>
        /// <returns>以Base64格式返回的加密字符串</returns>
        public static string DesEncrypt(string str, string key = null)
        {
            if (String.IsNullOrEmpty(str) == true)
                return null;

            if (key == null) key = _DesKey;

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string result = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return result;
            }
        }

        /// <summary>
        /// 进行DES解密
        /// </summary>
        /// <param name="str">要解密的以Base64</param>
        /// <param name="key">密钥，且必须为8位</param>
        /// <returns>已解密的字符串</returns>
        public static string DesDecrypt(string str, string key = null)
        {
            if (String.IsNullOrEmpty(str) == true)
                return null;

            if (key == null) key = _DesKey;

            byte[] inputByteArray = Convert.FromBase64String(str);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string result = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return result;
            }
        }
        #endregion

        #region Sha1加密
        /// <summary>
        /// Sha1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Sha1(string str)
        {
            if (String.IsNullOrEmpty(str) == true)
                return null;

            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "Sha1");
        }
        #endregion

        #region Md5加密
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            if (String.IsNullOrEmpty(str) == true)
                return null;

            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "Md5");
        }
        #endregion
    }
}

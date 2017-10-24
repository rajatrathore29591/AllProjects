using System;
using System.Security.Cryptography;
using System.Text;
namespace BorgCivil.Utils
{
    public class SecurityHelper
    {
        #region Public Properties

        private static string SECURITY_KEY = "qAyEBatre3rUp9EfUfaK";

        #endregion

        #region Public Methods

        /// <summary>
        /// Encrypt a string using dual encryption method. Returns a encrypted text.
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns>Returns encrypted string.</returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            return Encrypt(toEncrypt, useHashing, SECURITY_KEY);
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Returns a encrypted text with custom security(key).
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns>Returns encrypted string.</returns>
        public static string Encrypt(string toEncrypt, bool useHashing, string securityKey)
        {
            #region
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                // Get the key from config file
                string key = securityKey;
                if (useHashing)
                {
                    using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
                    {
                        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    }
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                byte[] resultArray = null;
                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tdes.CreateEncryptor();
                    resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                }
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                throw;
            }
            #endregion
        }

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string.
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns>Returns decrypted text.</returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            return Decrypt(cipherString, useHashing, SECURITY_KEY);
        }

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string with custom security(key).
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns>Returns decrypted text.</returns>
        public static string Decrypt(string cipherString, bool useHashing, string securityKey)
        {
            #region
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                //Get your key from config file to open the lock!
                string key = securityKey;

                if (useHashing)
                {
                    using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
                    {
                        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    }
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                byte[] resultArray = null;
                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = tdes.CreateDecryptor();
                    resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                }

                if (resultArray.Length > 0)
                    return UTF8Encoding.UTF8.GetString(resultArray);
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
            #endregion
        }
        #endregion
    }
}
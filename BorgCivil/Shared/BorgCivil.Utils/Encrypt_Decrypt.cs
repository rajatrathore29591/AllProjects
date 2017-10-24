using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BorgCivil.Utils
{
  public static class Encrypt_Decrypt
    {
        const string DESKey = "AQWSEDRF";

        const string DESIV = "HGFEDCBA";

        public static string DESDecrypt(string stringToDecrypt)
        {
            //Decrypt the content

            byte[] key = null;
            byte[] IV = null;

            byte[] inputByteArray = null;

            try
            {
                key = Convert2ByteArray(DESKey);

                IV = Convert2ByteArray(DESIV);

                int len = stringToDecrypt.Length;
                inputByteArray = Convert.FromBase64String(stringToDecrypt);


                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                Encoding encoding__1 = Encoding.UTF8;
                return encoding__1.GetString(ms.ToArray());


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        public static string DESEncrypt(string stringToEncrypt)
        {
            // Encrypt the content

            byte[] key = null;
            byte[] IV = null;

            byte[] inputByteArray = null;

            try
            {
                key = Convert2ByteArray(DESKey);

                IV = Convert2ByteArray(DESIV);

                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        private static byte[] Convert2ByteArray(string strInput)
        {

            int intCounter = 0;
            char[] arrChar = null;
            arrChar = strInput.ToCharArray();

            byte[] arrByte = new byte[arrChar.Length];

            for (intCounter = 0; intCounter <= arrByte.Length - 1; intCounter++)
            {
                arrByte[intCounter] = Convert.ToByte(arrChar[intCounter]);
            }

            return arrByte;

        }

        public static bool IsBase64(this string base64String)
        {

            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }
    }
}
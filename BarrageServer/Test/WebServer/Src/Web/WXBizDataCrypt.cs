using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebServer
{
    public class WXBizDataCrypt
    {
        public string appId;
        public string sessionKey;
        public WXBizDataCrypt(string appId, string sessionKey)
        {
            this.appId = appId;
            this.sessionKey = sessionKey;

        }
        public string decrypt(string encryptedData, string iv)
        {
            //sessionkey 
            byte[] sessionKeyByte = Convert.FromBase64String(sessionKey);
            string sessionKeyString = Encoding.Default.GetString(sessionKeyByte);
            //数据
            byte[] encryptedDataByte = Convert.FromBase64String(encryptedData);
            string encryptedDataString = Encoding.Default.GetString(encryptedDataByte);
            //IV
            byte[] ivByte = Convert.FromBase64String(iv);
            string ivString = Encoding.Default.GetString(ivByte);



            // AES加密
            // byte[] encrypted = EncryptStringToBytes_Aes(string, sessionKeyByte, ivByte);

            // Aes解密
            return DecryptStringFromBytes_Aes(encryptedDataByte, sessionKeyByte, ivByte);

        }
         byte[] endecrypt(string Data, string iv)
        {
            //sessionkey 
            byte[] sessionKeyByte = Convert.FromBase64String(sessionKey);
            string sessionKeyString = Encoding.Default.GetString(sessionKeyByte);

            //IV
            byte[] ivByte = Convert.FromBase64String(iv);
            string ivString = Encoding.Default.GetString(ivByte);
            // AES加密
            return EncryptStringToBytes_Aes(Data, sessionKeyByte, ivByte);



        }
        //加密
         byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }
        //解密
        string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting 
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}

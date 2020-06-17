using System;
using System.Security.Cryptography;
using System.Text;

namespace EasyAssetManagerCore.Model.CommonModel
{
    public class Encription
    {
        private const string EncryptionKey = "MAKV2SPBNI99212BugY$k#";
        DESCryptoServiceProvider cryp = new DESCryptoServiceProvider();
        byte[] bytes = ASCIIEncoding.ASCII.GetBytes("SyfulIsl");
        //public static string Encrypt(string clearText)
        //{

        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Dispose();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}


        //public static string Decrypt(string cipherText)
        //{

        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Dispose();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}

        public string Encrypt(string originalString)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(originalString);
            byte[] resultArray = cryp.CreateEncryptor(bytes, bytes).TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public  string Decrypt(string cipherString)
        {
            byte[] toDecryptArray = Convert.FromBase64String(cipherString);
            byte[] resultArray = cryp.CreateDecryptor(bytes, bytes).TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}

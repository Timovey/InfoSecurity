using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace InfoSecurity1.BusinessLogic.Helpers
{
    public static class FileHelper
    {
        private static readonly string UserStorageName = "..\\Users.txt";
        private static readonly byte[] SavedKey = new byte[16] {64,37,178,248,69,9,33,92,248,159,77,122,189,212,18,33};
        private static readonly byte[] SavedIV = new byte[16] {100,75,140,209,34,82,195,149,175,76,143,18,65,160,91,167};
        private static readonly string SavedKeyText = "abcdefgh";
        private static readonly string SavedIVText = "hgfedcba";

        public static string Encode(string text)
        {
            try
            {
                string ourText = text;
                string Return = null;
                string _key = "abcdefgh";
                string privatekey = "hgfedcba";
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);
                byte[] inputtextbyteArray = Encoding.UTF8.GetBytes(ourText);
                using (DESCryptoServiceProvider dsp = new DESCryptoServiceProvider())
                {

                    dsp.BlockSize = 64;
                    dsp.KeySize = 64;
                    dsp.Mode = CipherMode.CFB;
                    dsp.FeedbackSize = 8;
                    dsp.Padding = PaddingMode.None;

                    var memstr = new FileStream(UserStorageName, FileMode.OpenOrCreate);
                    var crystr = new CryptoStream(memstr, dsp.CreateEncryptor(_keybyte, privatekeyByte), CryptoStreamMode.Write);
                    crystr.Write(inputtextbyteArray, 0, inputtextbyteArray.Length);
                    crystr.FlushFinalBlock();
                    memstr.Close();
                    return "";
                }
                return Return;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }
        public static string Decode()
        {
            try
            {
                string x = null;
                string _key = "abcdefgh";
                string privatekey = "hgfedcba";
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);
                using (DESCryptoServiceProvider dEsp = new DESCryptoServiceProvider())
                {
                    dEsp.BlockSize = 64;
                    dEsp.KeySize = 64;
                    dEsp.Mode = CipherMode.CFB;
                    dEsp.FeedbackSize = 8;
                    dEsp.Padding = PaddingMode.None;

                    var memstr = new FileStream(UserStorageName, FileMode.Open);
                    var crystr = new CryptoStream(memstr, dEsp.CreateDecryptor(_keybyte, privatekeyByte), CryptoStreamMode.Read);
                    string b;
                    int y;
                    byte[] uy = new byte[memstr.Length];
                    int i = 0;
                    while ((y = crystr.ReadByte()) != -1)
                    {
                        uy[i] = (byte)y;
   
                        i++;
                    }
                    byte[] res = new byte[i];

                    string hx = Encoding.UTF8.GetString(uy);
                    for (int m = 0; m < i; m++) 
                    {
                        res[m] = uy[m];
                    }
                    memstr.Close();
                    x = Encoding.UTF8.GetString(res);

                }
                return x;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //public static string Encode(string text)
        //{
        //    try
        //    {
        //        string ourText = text;
        //        string Return = null;
        //        string _key = "abcdefgh";
        //        string privatekey = "hgfedcba";
        //        byte[] privatekeyByte = { };
        //        privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        //        byte[] _keybyte = { };
        //        _keybyte = Encoding.UTF8.GetBytes(_key);
        //        byte[] inputtextbyteArray = Encoding.UTF8.GetBytes(ourText);
        //        using (DESCryptoServiceProvider dsp = new DESCryptoServiceProvider())
        //        {

        //            var memstr = new FileStream(UserStorageName, FileMode.OpenOrCreate);
        //            var crystr = new CryptoStream(memstr, dsp.CreateEncryptor(_keybyte, privatekeyByte), CryptoStreamMode.Write);
        //            crystr.Write(inputtextbyteArray, 0, inputtextbyteArray.Length);
        //            crystr.FlushFinalBlock();
        //            memstr.Close();
        //            return "";
        //        }
        //        return Return;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return "";
        //}
        //public static string Decode()
        //{
        //    try
        //    {
        //        string x = null;
        //        string _key = "abcdefgh";
        //        string privatekey = "hgfedcba";
        //        byte[] privatekeyByte = { };
        //        privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        //        byte[] _keybyte = { };
        //        _keybyte = Encoding.UTF8.GetBytes(_key);
        //        using (DESCryptoServiceProvider dEsp = new DESCryptoServiceProvider())
        //        {
        //            var memstr = new FileStream(UserStorageName, FileMode.Open);
        //            var crystr = new CryptoStream(memstr, dEsp.CreateDecryptor(_keybyte, privatekeyByte), CryptoStreamMode.Read);
        //            string b;
        //            int y;
        //            byte[] uy = new byte[memstr.Length];
        //            int i = 0;
        //            while ((y = crystr.ReadByte()) != -1)
        //            {
        //                uy[i] = (byte)y;

        //                i++;
        //            }
        //            byte[] res = new byte[i];

        //            for (int m = 0; m < i; m++)
        //            {
        //                res[m] = uy[m];
        //            }
        //            memstr.Close();
        //            x = Encoding.UTF8.GetString(res);

        //        }
        //        return x;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        public static string Write2(string Plain)
        {
            try
            {
                string ourText = "bIwl1Y/220KPke+MzXxKErqBT5jkOEbDJnVEPxCzs7I=";
                string x = null;
                string _key = "abcdefgh";
                string privatekey = "hgfedcba";
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);
                byte[] inputtextbyteArray = new byte[ourText.Replace(" ", "+").Length];
                //This technique reverses base64 encoding when it is received over the Internet.
                inputtextbyteArray = Convert.FromBase64String(ourText.Replace(" ", "+"));
                using (DESCryptoServiceProvider dEsp = new DESCryptoServiceProvider())
                {
                    var memstr = new MemoryStream();
                    var crystr = new CryptoStream(memstr, dEsp.CreateDecryptor(_keybyte, privatekeyByte), CryptoStreamMode.Write);
                    crystr.Write(inputtextbyteArray, 0, inputtextbyteArray.Length);
                    crystr.FlushFinalBlock();
                    return Encoding.UTF8.GetString(memstr.ToArray());
                }
                return x;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Read()
        {
            

            //
            // Now decrypt the cipher back to plaintext
            //

            using (RijndaelManaged Aes128 = new RijndaelManaged())
            {
                GetSettingAes(Aes128);
                using (var decryptor = Aes128.CreateDecryptor())
                //using (var msEncrypt = new MemoryStream(cipherBytes))
                using (var msEncrypt = new FileStream(UserStorageName, FileMode.Open))
                using (var csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                //using (var br = new BinaryReader(csEncrypt, Encoding.UTF8))
                {
                    //csEncrypt.FlushFinalBlock();
                    //plainBytes = br.ReadBytes(cipherBytes.Length);
                    //byte[] line;
                    //while ((line = sr.ReadLine()) != null)
                    //{
                    //    //plainBytes.Append(Encoding.UTF8.GetBytes(line));
                    //    Console.WriteLine(Encoding.UTF8.GetString(line));
                    //}

                    //byte v;
                    ////byte[] plainBytes = br.ReadBytes(plainBytes.Length);
                    //var readFile = br.ReadToEnd();
                    //byte[] plainBytes = Encoding.ASCII.GetBytes(readFile);

                    //Console.WriteLine(readFile);
                    //Console.WriteLine(plainBytes);
                    //Console.WriteLine(Encoding.ASCII.GetString(plainBytes));


                    int y = 0;
                    byte[] uy = new byte[msEncrypt.Length];
                    //while(y < msEncrypt.Length)
                    //{
                    //    byte v = br.ReadByte();
                    //    uy[y] = v;
                    //    y++;
                    //}

                    int i = 0;
                    while ((y = csEncrypt.ReadByte()) != -1)
                    {
                        uy[i] = (byte)y;
                        i++;
                    }
                    Console.WriteLine(uy);
                    Console.WriteLine(Encoding.UTF8.GetString(uy));

                }
            }
            return "";
        }
        public static bool Write(string Plain)
        {
            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(Plain);

                using (RijndaelManaged Aes128 = new RijndaelManaged())
                {
                    GetSettingAes(Aes128);

                    using (var encryptor = Aes128.CreateEncryptor())
                    using (var msEncrypt = new FileStream(UserStorageName, FileMode.OpenOrCreate))
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    //using (var bw = new BinaryWriter(csEncrypt, Encoding.UTF8))
                    {
                        foreach(var p in plainBytes)
                        {
                            csEncrypt.WriteByte(p);

                        }
                        msEncrypt.Close();
                        //bw.Write(plainBytes);
                        //bw.Close();
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        //настраиваем Rijndael
        private static void GetSettingAes(RijndaelManaged Aes128)
        {
            //
            // Specify a blocksize of 128, and a key size of 128, which make this
            // instance of RijndaelManaged an instance of AES 128.
            //
            Aes128.BlockSize = 128;
            Aes128.KeySize = 128;
            //
            // Specify CFB8 mode
            //
            Aes128.Mode = CipherMode.CFB;
            Aes128.FeedbackSize = 8;
            Aes128.Padding = PaddingMode.None;

            //
            //Specify vector and secret Key
            Aes128.Key = SavedKey;
            Aes128.IV = SavedIV;
            //Aes128.GenerateKey();
            //Aes128.GenerateIV();

            return;
        }
    }                              
}

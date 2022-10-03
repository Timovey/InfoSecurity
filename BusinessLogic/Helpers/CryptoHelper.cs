using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Security.Cryptography;

namespace InfoSecurity1.BusinessLogic.Helpers
{
    public class CryptoHelper
    {
        //для кофига ключ и вектор инициализации в строковом представлении
        private static readonly string Key = "abcdefgh";
        private static readonly string IV = "hgfedcba";

        //временный файл и зашифрованный
        private static readonly string UserStorageName = "..\\Users.txt";
        private static readonly string UserStorageNameCrypto = "..\\UsersCrypto.txt";
        private List<User> users;
        private DESCryptoServiceProvider dESCryptoServiceProvider;
        public CryptoHelper()
        {
            //конфиг шифр-класса
            dESCryptoServiceProvider = new DESCryptoServiceProvider();
            dESCryptoServiceProvider.Mode = CipherMode.CFB;
            dESCryptoServiceProvider.Padding = PaddingMode.PKCS7;

            //проверка на наличия зашифрованного файла
            if (!File.Exists(UserStorageNameCrypto))
            {
                users = new List<User>() { new User()
                {
                    Login = "ADMIN",
                    Blocked = false,
                    Password = "",
                    RestrictionPassword = false
                } };
                //записываем во временный файл админа без пароля
                using (var sw = new StreamWriter(UserStorageName))
                {
                    sw.WriteLine(users.SafeSerialize(null));
                    sw.Close();
                }

            }
            else
            {
                
                //считываем данные из зашифрованного файла во временный, используя ключ и вектор инициализации
                byte[] bytes = File.ReadAllBytes(UserStorageNameCrypto);
                byte[] eBytes = dESCryptoServiceProvider.CreateDecryptor
                    (Encoding.UTF8.GetBytes(Key), Encoding.UTF8.GetBytes(IV)).
                        TransformFinalBlock(bytes, 0, bytes.Length);
                File.WriteAllBytes(UserStorageName, eBytes);
                string usersText = File.ReadAllText(UserStorageName);
                users = usersText.SafeDeserialize<List<User>>(null);
                //если вдруг чего не так пошло - создаем админа без пароля
                if(users == null || users.Count == 0)
                {
                    users = new List<User>() { new User()
                    {
                        Login = "ADMIN",
                        Blocked = false,
                        Password = "",
                        RestrictionPassword = false
                    } };
                }
            }
        }
        public void EncryptUsers()
        {
            // считываем данные из временного файла в зашифрованный, используя ключ и вектор инициализации
            byte[] bytes = File.ReadAllBytes(UserStorageName);
            byte[] eBytes = dESCryptoServiceProvider.
                CreateEncryptor(Encoding.UTF8.GetBytes(Key),
                    Encoding.UTF8.GetBytes(IV)).
                        TransformFinalBlock(bytes, 0, bytes.Length);
            File.WriteAllBytes(UserStorageNameCrypto, eBytes);
            //удаляем временный файл
            if(File.Exists(UserStorageName))
            {
                File.Delete(UserStorageName);
            }
        }
        //сохраняем юзеров во временный файл
        private void SaveUsers()
        {
            try
            {
                using (var sw = new StreamWriter(UserStorageName, false))
                {
                    sw.WriteLine(users.SafeSerialize(null));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<User> GetUsers()
        {
            return users;
        }
        public void AddUser(User newUser)
        {
            try
            {
                users.Add(newUser);
                SaveUsers();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void EditUser(User editUser)
        {
            try
            {
                //находим и изменяем все поля
                foreach(var user in users)
                {    
                    if(user.Login.Equals(editUser.Login))
                    {
                        user.Blocked = editUser.Blocked;
                        user.Password = editUser.Password;  
                        user.RestrictionPassword = editUser.RestrictionPassword;

                    }
                }
                SaveUsers();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

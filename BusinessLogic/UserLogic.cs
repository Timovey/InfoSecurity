using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using InfoSecurity1.BusinessLogic.Helpers;
using Unity;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Security.Cryptography;

namespace InfoSecurity1.BusinessLogic
{
    public class UserLogic
    {
        //зависимость IoC - контейнера
        [Dependency]
        public IUnityContainer Container { get; set; }
        private List<User> users = new List<User>();
        private User Admin;
        private CryptoHelper helper;
        private SHA256CryptoServiceProvider hashProvider;
        /// <summary>
        /// соль для хеширования
        /// </summary>
        byte[] Salt = Encoding.UTF8.GetBytes("abvvfghq");
        /// <summary>
        /// строка - регулярное выражение для ограничения на пароль
        /// </summary>
        private string regex = @"[A-Za-zА-Яа-я*+/-]";
        public UserLogic(CryptoHelper _helper)
        {
            helper = _helper;
            hashProvider = new SHA256CryptoServiceProvider();
            //получаем всех юзеров в конструкторе
            LoadData();
        }
        public bool IsAdmin(User user)
        {
            if(user == null) return false;

            return user.Login.Equals(Admin.Login) && HashPassword(user.Password).Equals(Admin.Password);
        }
        public bool IsPasswordEmpty(string login)
        {
            foreach(var u in users)
            {
                if (login.Equals(u.Login) && u.Password.Equals(""))
                {
                    return true;
                }
                else if (login.Equals(u.Login) && !u.Password.Equals(""))
                {
                    return false;
                }
            }
            return false;
        }
        public bool IsExistUser(string login)
        {
            foreach (var u in users)
            {
                if (u.Login.Equals(login))
                {
                    return true;
                }
            }
            return false;
        }
        private User GetUserByLogin(string login)
        {
            foreach (var u in users)
            {
                if (u.Login.Equals(login))
                {
                    return u;
                }
            }
            return null;
        }
        public User ChangePassword(User user)
        {
            User u = GetUserByLogin(user.Login);

            if (!u.RestrictionPassword)
            {
                EditUser(user);
                return GetUserByLogin(user.Login);
            }
            else
            {
                if (!Regex.IsMatch(user.Password, regex))
                {
                    return null;                }
                else
                {
                    EditUser(user);
                    return GetUserByLogin(user.Login);
                }
            }
            return null;
        }
        // метод для сравнивания паролей
        public User MatchPassword(User user)
        {
            User u = GetUserByLogin(user.Login);
            if(u == null)
            {
                throw new Exception("Нет пользователя с таким логином");
            }
            if(u.Blocked)
            {
                throw new Exception("Ваши функции заблокированы");
            }
            else
            {
                //если пароля не было и нет ограничений - редактируем пользователя
                if (string.IsNullOrEmpty(u.Password) && !u.RestrictionPassword)
                {
                    user.RestrictionPassword = u.RestrictionPassword;
                    user.Blocked = u.Blocked;
                    EditUser(user);
                    return user;
                }
                //если пароля не было и есть ограничения
                else if (string.IsNullOrEmpty(u.Password) && u.RestrictionPassword)
                {
                    if (!Regex.IsMatch(user.Password, regex))
                    {
                        throw new RegexExeption("Пароль должен состоять из символов латиницы, кириллицы и знаков операций");
                    }
                    else {
                        user.RestrictionPassword = u.RestrictionPassword;
                        user.Blocked = u.Blocked;
                        EditUser(user);
                        return user;
                    }
                }
                else
                {
                    //проверяем хешированные пароли
                    return u.Password.Equals(HashPassword(user.Password)) ? user : null; 
                }  
                
            }
            return null;
        }
        //получение всех пользователей без админа
        public List<User> GetUsers()
        {
            return users.Where(x => !x.Login.Equals("ADMIN")).ToList();
        }
        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new Exception("Нет пользователя для добавления");
            }
            User existUser = GetUserByLogin(user.Login);
            if(existUser != null)
            {
                throw new Exception("Пользователь с таким именем уже существует");
            }
            helper.AddUser(user);
            LoadData();
        }
        public void EditUser(User user, bool IsChahgePassword = true)
        {
            if(user == null)
            {
                throw new Exception("Нет пользователя для редактирования");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                helper.EditUser(user);
            }
            else if (!IsChahgePassword)
            {
                helper.EditUser(user);
            }
            else
            {
                helper.EditUser(HashPassword(user));
            }
            LoadData();
        }
        private void LoadData()
        {
            users = helper.GetUsers();

            foreach(var u in users)
            {
                if(u.Login == "ADMIN")
                {
                    Admin = u;
                }
            }
        }
        //методы для хеширования паролей
        private User HashPassword(User user)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(user.Password);

            byte[] saltedInput = new byte[Salt.Length + passwordBytes.Length];
            Salt.CopyTo(saltedInput, 0);
            passwordBytes.CopyTo(saltedInput, Salt.Length);

            byte[] hashedBytes = hashProvider.ComputeHash(saltedInput);

            user.Password = BitConverter.ToString(hashedBytes);

            return user;
        }
        //получаем байтовое представление пароля
        //получаем "соль" и совмещаем массивы байтов
        //возвращаем строковое представление
        private string HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] saltedInput = new byte[Salt.Length + passwordBytes.Length];
            Salt.CopyTo(saltedInput, 0);
            passwordBytes.CopyTo(saltedInput, Salt.Length);

            byte[] hashedBytes = hashProvider.ComputeHash(saltedInput);

            return BitConverter.ToString(hashedBytes);
        }
        //при уничтожении класса зашифрует всех пользователей
        ~UserLogic()
        {
            helper.EncryptUsers();
        }
        
    }
}

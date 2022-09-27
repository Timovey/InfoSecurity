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
        /// <summary>
        /// строка - регулярное выражение для ограничения на пароль
        /// </summary>
        private string regex = @"[A-Za-zА-Яа-я*+/-]";
        public UserLogic(CryptoHelper _helper)
        {
            helper = _helper;
            //получаем всех юзеров в конструкторе
            LoadData();
        }
        public bool IsAdmin(User user)
        {
            if(user == null) return false;

            return user.Login.Equals(Admin.Login) && user.Password.Equals(Admin.Password);
        }
        public bool IsPasswordEmpty(string login)
        {
            bool isEmpty = false;
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
            return isEmpty;
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

            if(!user.RestrictionPassword)
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
                    helper.EditUser(user);
                    return GetUserByLogin(user.Login);
                }
            }
            return null;
        }
        // метод для сравнивания паролей
        public User MatchPassword(User user)
        {
            if(user.Blocked)
            {
                throw new Exception("Ваши функции заблокированы");
            }
            else
            {
                User u = GetUserByLogin(user.Login);
                //если пароля не было и нет ограничений - редактируем пользователя
                if (string.IsNullOrEmpty(u.Password) && !u.RestrictionPassword)
                {
                    user.RestrictionPassword = u.RestrictionPassword;
                    user.Blocked = u.Blocked;
                    helper.EditUser(user);
                    return user;
                }
                //если пароля не было и есть ограничения
                else if (string.IsNullOrEmpty(u.Password) && u.RestrictionPassword)
                {
                    if (!Regex.IsMatch(user.Password, regex))
                    {
                        throw new Exception("Пароль должен состоять из символов латиницы, кириллицы и знаков операций");
                    }
                    else {
                        user.RestrictionPassword = u.RestrictionPassword;
                        user.Blocked = u.Blocked;
                        helper.EditUser(user);
                        return user;
                    }
                }
                else
                {
                    return u.Password.Equals(user.Password) ? user : null; 
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
        public void EditUser(User user)
        {
            if(user == null)
            {
                throw new Exception("Нет пользователя для редактирования");
            }
            helper.EditUser(user);
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
        //при уничтожении класса зашифрует всех пользователей
        ~UserLogic()
        {
            helper.EncryptUsers();
        }
        
    }
}

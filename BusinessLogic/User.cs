﻿using System.ComponentModel;

namespace InfoSecurity1.BusinessLogic
{
    public class User
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DisplayName("Имя")]
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DisplayName("Пароль")]
        public string Password { get; set; }
        /// <summary>
        /// Ограничения на пароль
        /// </summary>
        [DisplayName("Ограничения на пароль")]
        public bool RestrictionPassword { get; set; } = false;
        /// <summary>
        /// Блокировка пользователя
        /// </summary>
        [DisplayName("Блокировка пользователя")]
        public bool Blocked{ get; set; } = false;

    }
}

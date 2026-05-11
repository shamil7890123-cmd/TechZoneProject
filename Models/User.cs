using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TechZoneProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Display(Name = "Роль")]
        // Устанавливаем "User" как значение по умолчанию, чтобы не было пустых строк
        public string Role { get; set; } = "User";

        // Можно добавить дату регистрации, если захотите оживить колонку в таблице
        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TechZoneProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название товара")]
        [StringLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите цену")]
        [Range(0.01, 1000000, ErrorMessage = "Цена должна быть в диапазоне от 0.01 до 1 000 000")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Выберите категорию")]
        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Display(Name = "Ссылка на изображение")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Добавьте описание товара")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        // --- НОВОЕ ПОЛЕ: КОНТРОЛЬ КОЛИЧЕСТВА ---
        [Required(ErrorMessage = "Укажите количество на складе")]
        [Range(0, 10000, ErrorMessage = "Количество не может быть меньше 0")]
        [Display(Name = "Остаток на складе")]
        public int Stock { get; set; }

        [Display(Name = "Процессор")]
        public string Processor { get; set; }

        [Display(Name = "Видеокарта")]
        public string VideoCard { get; set; }

        [Display(Name = "Оперативная память")]
        public string RAM { get; set; }

        [Display(Name = "Накопитель")]
        public string Storage { get; set; }
    }
}
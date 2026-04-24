using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MvcOdev.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap adı boş bırakılamaz.")]
        [Display(Name = "Kitap Adı")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yazar adı boş bırakılamaz.")]
        [Display(Name = "Yazar")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat alanı boş bırakılamaz.")]
        [Range(0, 10000, ErrorMessage = "Fiyat 0 ile 10000 arasında olmalıdır.")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stok alanı boş bırakılamaz.")]
        [Range(0, 1000, ErrorMessage = "Stok 0 ile 1000 arasında olmalıdır.")]
        [Display(Name = "Stok")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Lütfen bir kategori seçiniz.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace MvcOdev.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap adı boş geçilemez")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Yazar adı boş geçilemez")]
        [StringLength(50)]
        public string Author { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalı")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok 0 veya daha büyük olmalı")]
        public int Stock { get; set; }
    }
}
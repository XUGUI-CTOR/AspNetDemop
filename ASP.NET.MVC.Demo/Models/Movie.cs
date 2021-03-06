using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ASP.NET.MVC.Demo.Models
{
    
    public class Movie
    {
        public int ID { get; set; }
        [Display(Name = "电影标题"),StringLength(60,MinimumLength = 3)]
        public string Title { get; set; }
        [Display(Name = "发行日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "类型")]
        [RegularExpression("^[A-Z]+[a-zA-Z'\\s]*$")]
        [Required]
        public string Genre { get; set; }
        [Display(Name = "票价")]
        [Range(1,100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Display(Name = "评级")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
    }

    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
    }
}
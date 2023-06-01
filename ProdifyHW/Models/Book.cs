using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProdifyHW.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; } = "";

        //[Required]
        //[Column(TypeName ="nvarchar(30)")]
        public int Year { get; set; }

        //[Required]
        //[Column(TypeName ="nvarchar(30)")]
        public string Author { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(30)")]
        public Boolean IsAvailable { get; set; } = false;

        //[Column(TypeName ="nvarchar(30)")]
        [ForeignKey("Client")]
        public int ClientID { get; set; }


    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProdifyHW.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; } = "";

        public int Year { get; set; }
   
        public string Author { get; set; }

        public Boolean IsAvailable { get; set; } = false;

        [ForeignKey("Client")]
        public int ClientID { get; set; }
    }
}

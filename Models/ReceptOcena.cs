using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models {

    [Table("RecOc")]
    public class ReceptOcena {

        [Key]
        public int ID { get; set; }

        [Required]
        public Recept Recept { get; set; }

        [Required]
        public Korisnik Korisnik { get; set; }

        [Required]
        public int Ocena { get; set; }

    }
}
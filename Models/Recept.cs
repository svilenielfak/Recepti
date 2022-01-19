using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models {

    [Table("Recept")]
    public class Recept {

        [Key]
        public int ID { get; set; }

        [Required]
        public string Naziv { get; set; }

        [Required]
        public Korisnik Korisnik { get; set; }

        public double Ocena { get; set; }

        public int BrojOcena { get; set; }

        [Required]
        public Kuvar Kuvar { get; set; }
    }
}
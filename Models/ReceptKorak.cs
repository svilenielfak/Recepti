using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models {

    [Table("RecKor")]
    public class ReceptKorak {

        [Key]
        public int ID { get; set; }

        [Required]
        public Recept Recept { get; set; }

        [Required]
        public int BrKorak { get; set; }

        [Required]
        public string Opis { get; set; }

    }
}
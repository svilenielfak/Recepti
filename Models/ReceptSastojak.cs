using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models {
    [Table("RecSas")]
    public class ReceptSastojak {

        [Key]
        public int ID { get; set; }

        [Required]
        public Recept Recept { get; set; }

        [Required]
        public Sastojak Sastojak { get; set; }

        public string Mera { get; set; }
    }
}
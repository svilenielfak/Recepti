using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models {

    [Table("Kuvar")]
    public class Kuvar {

        [Key]
        public int ID { get; set; }

        [Required]
        public string Naziv { get; set; }
    }
}
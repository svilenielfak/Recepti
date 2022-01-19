
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models {

    [Table("Sastojak")]

    public class Sastojak {

        [Key]
        public int ID { get; set; }

        [Required]
        public string Naziv { get; set; }


        public Kuvar Kuvar { get; set; }

        public static implicit operator Sastojak(ReceptSastojak v) {
            throw new NotImplementedException();
        }
    }
}
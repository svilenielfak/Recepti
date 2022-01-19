using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models {

    [Table("Korisnik")]
    public class Korisnik {

        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ime { get; set; }


        [Required]
        [MaxLength(60)]
        public string Prezime { get; set; }

        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Proverite e-mail!")]
        public string Email { get; set; }


        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Sifra mora imati barem jedno veliko slovo, jedno malo slovo, jedan broj, jedan specijalni znak i mora biti najmanje 8 karaktera ")]
        public string Password { get; set; }

    }
}
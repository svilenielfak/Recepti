using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers {


    [ApiController]
    [Route("[controller]")]
    public class ReceptOcenaController : ControllerBase {
        public KuvarContext Context { get; set; }

        public ReceptOcenaController(KuvarContext context) {
            Context = context;
        }

        [Route("Oceni/{idRecept}/{idKorisnik}/{ocena}")]
        [HttpPost]
        public async Task<ActionResult> Oceni(int idRecept, int idKorisnik, int ocena) {
            try {
                if (ocena < 1 || ocena > 5)
                    return BadRequest("Ocena mora biti izmedju 1 i 5!");

                Recept recept = await Context.Recepti.Where(r => r.ID == idRecept).FirstOrDefaultAsync();

                if (recept == null)
                    return BadRequest("Recept nije pronadjen!");

                Korisnik korisnik = await Context.Korisnici.FindAsync(idKorisnik);

                if (korisnik == null)
                    return BadRequest("Korisnik nije pronadjen!");

                var tmp = await Context.Ocene
                    .Where(o =>
                        o.Recept.ID == idRecept &&
                        o.Korisnik.ID == idKorisnik)
                    .FirstOrDefaultAsync();

                if (tmp != null)
                    return BadRequest("Vec ste ocenili ovaj recept!");

                recept.BrojOcena++;
                recept.Ocena = (recept.Ocena + ocena) / recept.BrojOcena;

                Context.Recepti.Update(recept);

                Context.Ocene.Add(new ReceptOcena {
                    Recept = recept,
                    Korisnik = korisnik,
                    Ocena = ocena
                });

                await Context.SaveChangesAsync();
                return Ok("Recept ocenjen!");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Update/{idRecept}/{idKorisnik}/{ocena}")]
        [HttpPut]
        public async Task<ActionResult> PromeniOcenu(int idRecept, int idKorisnik, int ocena) {
            try {
                if (ocena < 1 || ocena > 5)
                    return BadRequest("Ocena mora biti izmedju 1 i 5!");



                ReceptOcena ocenaObj = await Context.Ocene
                    .Where(o =>
                         o.Recept.ID == idRecept &&
                         o.Korisnik.ID == idKorisnik)
                         .Include(o => o.Recept)
                    .FirstOrDefaultAsync();

                Recept recept= ocenaObj.Recept; 

                if (ocenaObj == null)
                    return BadRequest("Doslo je do greske!");


                recept.Ocena = (recept.Ocena * recept.BrojOcena - ocenaObj.Ocena + ocena) / recept.BrojOcena;

                ocenaObj.Ocena = ocena;

                Context.Recepti.Update(recept);

                Context.Ocene.Update(ocenaObj);

                await Context.SaveChangesAsync();

                return Ok("Ocena promenjena!");

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Delete/{idRecept}/{idKorisnik}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiOcenu(int idRecept, int idKorisnik) {
            try {

                Recept recept = await Context.Recepti.Where(r => r.ID == idRecept).FirstOrDefaultAsync();

                if (recept == null)
                    return BadRequest("Recept nije pronadjen!");


                ReceptOcena ocenaObj = await Context.Ocene
                    .Where(o =>
                         o.Recept.ID == idRecept &&
                         o.Korisnik.ID == idKorisnik)
                    .FirstOrDefaultAsync();

                if (ocenaObj == null)
                    return BadRequest("Doslo je do greske!");


                recept.BrojOcena--;
                recept.Ocena = (recept.Ocena * recept.BrojOcena - ocenaObj.Ocena) / recept.BrojOcena;


                Context.Recepti.Update(recept);

                Context.Ocene.Remove(ocenaObj);

                await Context.SaveChangesAsync();

                return Ok("Ocena obrisana!");

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        [Route("Get/{idRecept}")]
        [HttpGet]
        public async Task<ActionResult> PromeniOcenu(int idRecept) {
            try {
                var ocene = await Context.Ocene.Where(o => o.Recept.ID == idRecept).ToListAsync();

                return Ok(ocene);

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        [Route("DidOcenio/{idRecept}/{idKor}")]
        [HttpGet]
        public async Task<ActionResult> DidOcenio(int idRecept, int idKor) {
            try {
                var ocene = await Context.Ocene.Where(o => o.Recept.ID == idRecept && o.Korisnik.ID == idKor).FirstOrDefaultAsync();

                return Ok(ocene);

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

    }
}

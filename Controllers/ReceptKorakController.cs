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
    public class ReceptKorakController : ControllerBase {
        public KuvarContext Context { get; set; }

        public ReceptKorakController(KuvarContext context) {
            Context = context;
        }

/*         [Route("Preuzmi/{idRec}/{brKor}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiKorak(int idRec, int brKor) {
            try {

                var korak = await Context.Koraci.Where(k => k.BrKorak == brKor && k.Recept.ID == idRec).FirstOrDefaultAsync();
                if (korak == null) {
                    return BadRequest("Korak nije pronadjen!");
                }

                return Ok(korak);

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        } */

        [Route("PreuzmiSve/{idRec}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSveKorake(int idRec) {
            try {

                var koraci = await Context.Koraci.Where(k => k.Recept.ID == idRec).OrderBy(k => k.BrKorak).ToListAsync();
                if (koraci == null) {
                    return BadRequest("Za ovaj recept nema koraka!");
                }

                return Ok(koraci);

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }



        [Route("Dodaj/{idRecept}/{brKorak}/{opis}")]
        [HttpPost]
        public async Task<ActionResult> Dodaj(int idRecept, int brKorak, string opis) {

            if (string.IsNullOrWhiteSpace(opis))
                return BadRequest("Korak mora biti opisan!");


            try {
                Recept recept = await Context.Recepti.FindAsync(idRecept);
                if (recept == null)
                    return BadRequest("Recept ne postoji!");

                var tmp = await Context.Koraci
                    .Where(k =>
                       k.BrKorak == brKorak &&
                       k.Recept.ID == idRecept)
                    .FirstOrDefaultAsync();

                if (tmp != null)
                    return BadRequest($"Korak broj {brKorak} je vec definisan za ovaj recept");

                Context.Koraci.Add(new ReceptKorak {
                    Opis = opis,
                    Recept = recept,
                    BrKorak = brKorak
                });
                await Context.SaveChangesAsync();

                return Ok("Korak dodat!");

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Update/{idKorak}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int idKorak, string opis) {

            if (string.IsNullOrWhiteSpace(opis))
                return BadRequest("Korak mora biti opisan!");
            try {
                var korak = await Context.Koraci.Where(rs => rs.ID == idKorak).FirstOrDefaultAsync();

                if (korak == null)
                    return BadRequest("Doslo je do greske!");

                korak.Opis = opis;

                Context.Koraci.Update(korak);
                await Context.SaveChangesAsync();

                return Ok("Korak promenjen!");

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Ukloni/{idKorak}")]
        [HttpDelete]
        public async Task<ActionResult> Obrisi(int idKorak) {
            try {
                var s = await Context.Koraci.FindAsync(idKorak);

                Context.Koraci.Remove(s);

                await Context.SaveChangesAsync();

                return Ok("Korak uklonjen!");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

    }
}
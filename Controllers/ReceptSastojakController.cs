using System;
using System.Collections.Generic;
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
    public class ReceptSastojakController : ControllerBase {
        public KuvarContext Context { get; set; }

        public ReceptSastojakController(KuvarContext context) {
            Context = context;
        }


        [Route("Dodaj/{idRecept}/{idSastojak}/{mera}")]
        [HttpPost]
        public async Task<ActionResult> Dodaj(int idRecept, int idSastojak, string mera) {

            if (string.IsNullOrWhiteSpace(mera))
                return BadRequest("Mera mora biti zadata!");


            try {
                Recept recept = await Context.Recepti.FindAsync(idRecept);
                if (recept == null)
                    return BadRequest("Recept ne postoji!");
                Sastojak sastojak = await Context.Sastojci.FindAsync(idSastojak);
                if (sastojak == null)
                    return BadRequest("Sastojak ne postoji!");


                var tmp = await Context.ReceptSastojak
                    .Where(q =>
                       q.Sastojak.ID == idSastojak &&
                       q.Recept.ID == idRecept)
                    .FirstOrDefaultAsync();

                if (tmp != null)
                    return BadRequest("Za ovaj recept je vec unet taj sastojak!");



                Context.ReceptSastojak.Add(new ReceptSastojak {
                    Mera = mera,
                    Recept = recept,
                    Sastojak = sastojak

                });
                await Context.SaveChangesAsync();

                return Ok("Sastojak dodat u recept");

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        [Route("Sastojci/{idRecept}")]
        [HttpGet]
        public async Task<ActionResult> GetSastojciZaRecept(int idRecept) {
            try {
                var sastojci = await Context.ReceptSastojak
                    .Where(rs => rs.Recept.ID == idRecept)
                    .Include(rs => rs.Sastojak)
                    .ToListAsync();

                return Ok(sastojci);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        [Route("Update/{idRs}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int idRs, int idSastojak, string mera) {
            try {
                ReceptSastojak receptSastojak = await Context.ReceptSastojak.Where(rs => rs.ID == idRs).FirstOrDefaultAsync();

                if (receptSastojak == null)
                    return BadRequest("Doslo je do greske!");

                if (idSastojak > 0) {
                    Sastojak s = await Context.Sastojci.FindAsync(idSastojak);
                    if (s == null)
                        return BadRequest("Sastojak ne postoji!");
                    receptSastojak.Sastojak = s;
                }
                if (string.IsNullOrWhiteSpace(mera))
                    receptSastojak.Mera = mera;

                Context.ReceptSastojak.Update(receptSastojak);
                await Context.SaveChangesAsync();

                return Ok("Sastojak za recept promenjen");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("PoSastojcima/{idKuvar}")]
        [HttpPut]
        public async Task<ActionResult> PretragaPoSastojcima(int idKuvar, [FromBody] int[] sasIds) {
            try {
                var recepti = await Context.ReceptSastojak.Where(rs => sasIds.Contains(rs.Sastojak.ID) && rs.Recept.Kuvar.ID == idKuvar)
                        .Include(rs => rs.Recept)
                            .ThenInclude(r => r.Korisnik)

                        .Select(r => new Recept {
                            ID = r.Recept.ID,
                            Naziv = r.Recept.Naziv,
                            Ocena = r.Recept.Ocena,
                            BrojOcena = r.Recept.BrojOcena,
                            Korisnik = new Korisnik {
                                Ime = r.Recept.Korisnik.Ime,
                                Prezime = r.Recept.Korisnik.Prezime,
                            }
                        }).Distinct()
                    .ToListAsync();
                /*            var receptiIds = new List<Recept>();
                           foreach (var r in recepti) {
                               receptiIds.Add(((Recept)r).Recept);
                           } */

                return Ok(recepti);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Ukloni/{idRs}")]
        [HttpDelete]
        public async Task<ActionResult> Obrisi(int idRs) {
            try {
                var s = await Context.ReceptSastojak.FindAsync(idRs);

                Context.ReceptSastojak.Remove(s);

                await Context.SaveChangesAsync();

                return Ok("Sastojak uklonjen iz recepta!");
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
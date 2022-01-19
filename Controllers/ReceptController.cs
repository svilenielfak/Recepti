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
    public class ReceptController : ControllerBase {
        public KuvarContext Context { get; set; }

        public ReceptController(KuvarContext context) {
            Context = context;
        }


        public class SastojakHelper {
            public int id { get; set; }
            public string naziv { get; set; }
            public string mera { get; set; }
            public bool deleted { get; set; }
        }

        public class KorakHelper {
            public int id { get; set; }

            public int brKorak { get; set; }
            public string opis { get; set; }
        }


        public class ReceptHelper {
            public string naziv { get; set; }
            public int kuvarId { get; set; }
            public int korisnikID { get; set; }

            public List<SastojakHelper> sastojci { get; set; }

            public List<KorakHelper> koraci { get; set; }
        }

        [Route("Dodaj")]
        [HttpPost]
        public async Task<ActionResult> Dodaj([FromBody] ReceptHelper helper) {
            try {
                var tmp = await Context.Recepti
                    .Where(r =>
                       r.Naziv == helper.naziv &&
                       r.Korisnik.ID == helper.korisnikID)
                    .FirstOrDefaultAsync();

                if (tmp != null)
                    return BadRequest("Vec imate recept sa istim imenom!");


                var korisnik = await Context.Korisnici.Where(k => k.ID == helper.korisnikID).FirstOrDefaultAsync();

                if (korisnik == null)
                    return BadRequest("Korisnik nije pronadjen!");

                var kuvar = await Context.Kuvari.Where(k => k.ID == helper.kuvarId).FirstOrDefaultAsync();

                Recept recept = new Recept {
                    Naziv = helper.naziv,
                    Korisnik = korisnik,
                    Ocena = 0,
                    BrojOcena = 0,
                    Kuvar = kuvar
                };
                Context.Recepti.Add(recept);


                foreach (var sastojakh in helper.sastojci) {
                    var s = await Context.Sastojci.Where(s => s.Naziv.ToLower().Equals(sastojakh.naziv.ToLower())).FirstOrDefaultAsync();

                    Sastojak sastojak;
                    if (s == null) {

                        sastojak = new Sastojak {
                            Naziv = sastojakh.naziv,
                            Kuvar = kuvar
                        };

                        Context.Sastojci.Add(sastojak);
                    } else {
                        sastojak = s;
                        sastojak.Kuvar = null;
                    }

                    Context.ReceptSastojak.Add(new ReceptSastojak {
                        Mera = sastojakh.mera,
                        Recept = recept,
                        Sastojak = sastojak
                    });
                }

                foreach (var kor in helper.koraci) {
                    Context.Koraci.Add(new ReceptKorak {
                        Recept = recept,
                        BrKorak = kor.brKorak,
                        Opis = kor.opis
                    });
                }



                await Context.SaveChangesAsync();
                return Ok("Recept je dodat" + helper.naziv);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<ActionResult> Izmeni([FromBody] ReceptHelper helper) {
            try {
                var recept = await Context.Recepti
                    .Where(r =>
                       r.Naziv == helper.naziv &&
                       r.Korisnik.ID == helper.korisnikID)
                    .FirstOrDefaultAsync();

                recept.Naziv = helper.naziv;


                var korisnik = await Context.Korisnici.Where(k => k.ID == helper.korisnikID).FirstOrDefaultAsync();

                if (korisnik == null)
                    return BadRequest("Korisnik nije pronadjen!");

                var kuvar = await Context.Kuvari.Where(k => k.ID == helper.kuvarId).FirstOrDefaultAsync();

                Context.Recepti.Update(recept);

                /**/
                var recsas = await Context.ReceptSastojak.Where(rs => rs.Recept.ID == recept.ID).ToListAsync();
                foreach (var rs in recsas)
                    Context.ReceptSastojak.Remove(rs);
                var reckor = await Context.Koraci.Where(rk => rk.Recept.ID == recept.ID).ToListAsync();
                foreach (var rk in reckor)
                    Context.Koraci.Remove(rk);
                /**/

                foreach (var sastojakh in helper.sastojci) {
                    var s = await Context.Sastojci.Where(s => s.Naziv.ToLower().Equals(sastojakh.naziv.ToLower())).FirstOrDefaultAsync();

                    Sastojak sastojak;
                    if (s == null) {

                        sastojak = new Sastojak {
                            Naziv = sastojakh.naziv,
                            Kuvar = kuvar
                        };

                        Context.Sastojci.Add(sastojak);
                    } else {
                        sastojak = s;
                        sastojak.Kuvar = null;
                    }

                    Context.ReceptSastojak.Add(new ReceptSastojak {
                        Mera = sastojakh.mera,
                        Recept = recept,
                        Sastojak = sastojak
                    });

                }

                foreach (var kor in helper.koraci) {
                    Context.Koraci.Add(new ReceptKorak {
                        Recept = recept,
                        BrKorak = kor.brKorak,
                        Opis = kor.opis
                    });
                }

                await Context.SaveChangesAsync();
                return Ok("Recept je izmenjen" + helper.naziv);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Recept/{idRecept}")]
        [HttpGet]
        public async Task<ActionResult> GetRecept(int idRecept) {
            try {
                var recept = await Context.Recepti.Where(r => r.ID == idRecept).FirstOrDefaultAsync();
                return Ok(recept);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        [Route("ReceptiKuvara/{idKuvar}/{naziv}")]
        [HttpGet]
        public async Task<ActionResult> GetRecepteKuvara(int idKuvar, string naziv) {
            try {
                var recepti = await Context.Recepti.Where(r => r.Kuvar.ID == idKuvar && r.Naziv.ToLower().Contains(naziv.ToLower()))
                    .Include(r => r.Kuvar)
                    .Include(r => r.Korisnik)
                    .Select(r => new Recept {
                        ID = r.ID,
                        Naziv = r.Naziv,
                        Ocena = r.Ocena,
                        BrojOcena = r.BrojOcena,
                        Korisnik = new Korisnik {
                            Ime = r.Korisnik.Ime,
                            Prezime = r.Korisnik.Prezime
                        }
                    })
                .ToListAsync();
                return Ok(recepti);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        [Route("ReceptiKorisnika/{idKor}/{idKuvar}")]
        [HttpGet]
        public async Task<ActionResult> GetRecepteKorisnika(int idKor, int idKuvar) {
            try {
                var recepti = await Context.Recepti
                    .Where(r =>
                            r.Korisnik.ID == idKor &&
                            r.Kuvar.ID == idKuvar)

                        .Include(r => r.Korisnik)
                    .ToListAsync();
                return Ok(recepti);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        [Route("Delete/{idRecept}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiRecept(int idRecept) {
            try {
                var recept = await Context.Recepti.FindAsync(idRecept);

                if (recept == null)
                    return BadRequest("Recept nije pronadjen.");

                var satojci = await Context.ReceptSastojak.Where(rs => rs.Recept.ID == idRecept).ToListAsync();
                foreach (var s in satojci)
                    Context.ReceptSastojak.Remove(s);

                var koraci = await Context.Koraci.Where(k => k.Recept.ID == idRecept).ToListAsync();
                foreach (var k in koraci)
                    Context.Koraci.Remove(k);

                var ocene = await Context.Ocene.Where(o => o.Recept.ID == idRecept).ToListAsync();
                foreach (var o in ocene)
                    Context.Ocene.Remove(o);

                Context.Recepti.Remove(recept);

                await Context.SaveChangesAsync();

                return Ok("Recept obrisan!");

            } catch (Exception e) {
                return BadRequest(e.Message);
            }

        }
    }
}

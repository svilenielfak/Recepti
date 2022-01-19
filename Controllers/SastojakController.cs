using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers {


    [ApiController]
    [Route("[controller]")]
    public class SastojakController : ControllerBase {

        public KuvarContext Context { get; set; }

        public SastojakController(KuvarContext context) {

            Context = context;
        }


        [Route("All/{kuvarID}")]
        [HttpGet]
        public async Task<ActionResult> SviSastojci(int kuvarID) {
            try {
                var sastojci = await Context.Sastojci.Where(s => s.Kuvar.ID == kuvarID || s.Kuvar == null).ToListAsync();

                return Ok(sastojci);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("Nadji/{ime}")]
        [HttpGet]
        public async Task<ActionResult> Nadji(string ime) {

            try {
                var sastojak = await Context.Sastojci.Where(s => s.Naziv.ToLower().Equals(ime)).FirstOrDefaultAsync();

                if (sastojak == null)
                    return BadRequest("Sastojak sa tim nazivom nije pronadjen!");

                return Ok(sastojak);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        [Route("Dodaj")]
        [HttpPost]
        public async Task<ActionResult> Dodaj([FromBody] Sastojak sastojak) {

            try {
                var s = await Context.Sastojci.Where(s => s.Naziv.ToLower().Equals(sastojak.Naziv.ToLower())).FirstOrDefaultAsync();

                if (s != null)
                    return BadRequest("Sastojak sa tim nazivom vec postoji!");
                var kuvar = await Context.Kuvari
                    .Where(r => r.ID == sastojak.Kuvar.ID)
                .FirstOrDefaultAsync();
                sastojak.Kuvar = kuvar;
                Context.Sastojci.Add(sastojak);
                await Context.SaveChangesAsync();
                return Ok(sastojak);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }


        }

    }
}
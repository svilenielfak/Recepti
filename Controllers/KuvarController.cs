using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers {


    [ApiController]
    [Route("[controller]")]
    public class KuvarController : ControllerBase {

        public KuvarContext Context { get; set; }

        public KuvarController(KuvarContext context) {

            Context = context;
        }

        [Route("Kuvari")]
        [HttpGet]
        public async Task<ActionResult> Preuzmi() {
            var kuvari = await Context.Kuvari.ToListAsync();
            return Ok(kuvari);
        }

        /*   [Route("AddKuvar")]
          [HttpPost]
          public async Task<ActionResult> DodajStudenta([FromBody] Kuvar kuvar) {
              try {
                  Context.Kuvari.Add(kuvar);
                  await Context.SaveChangesAsync();
                  return Ok("Kuvar je dodat!");
              } catch (System.Exception e) {
                  return BadRequest(e.Message);
              }

          }
   */

    }
}
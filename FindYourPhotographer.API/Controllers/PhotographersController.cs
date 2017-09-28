

namespace FindYourPhotographer.API.Controllers
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using FindYourPhotographer.Domain;
    [Authorize]
    public class PhotographersController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Photographers
        public IQueryable<Photographer> GetPhotographers()
        {
            return db.Photographers;
        }

        // GET: api/Photographers/5
        [ResponseType(typeof(Photographer))]
        public async Task<IHttpActionResult> GetPhotographer(int id)
        {
            Photographer photographer = await db.Photographers.FindAsync(id);
            if (photographer == null)
            {
                return NotFound();
            }

            return Ok(photographer);
        }

        // PUT: api/Photographers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhotographer(int id, Photographer photographer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photographer.PhotographerId)
            {
                return BadRequest();
            }

            db.Entry(photographer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotographerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Photographers
        [ResponseType(typeof(Photographer))]
        public async Task<IHttpActionResult> PostPhotographer(Photographer photographer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Photographers.Add(photographer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = photographer.PhotographerId }, photographer);
        }

        // DELETE: api/Photographers/5
        [ResponseType(typeof(Photographer))]
        public async Task<IHttpActionResult> DeletePhotographer(int id)
        {
            Photographer photographer = await db.Photographers.FindAsync(id);
            if (photographer == null)
            {
                return NotFound();
            }

            db.Photographers.Remove(photographer);
            await db.SaveChangesAsync();

            return Ok(photographer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotographerExists(int id)
        {
            return db.Photographers.Count(e => e.PhotographerId == id) > 0;
        }
    }
}
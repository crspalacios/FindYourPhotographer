

namespace FindYourPhotographer.Backend.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using FindYourPhotographer.Backend.Models;
    using FindYourPhotographer.Domain;
    using FindYourPhotographer.Backend.Helpers;
    using System;

    [Authorize(Users = "alopez@crealodigital.com")]
    public class PhotographersController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Photographers
        public async Task<ActionResult> Index()
        {
            var photographers = db.Photographers.Include(p => p.Category);
            return View(await photographers.ToListAsync());
        }

        // GET: Photographers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var  photographer = await db.Photographers.FindAsync(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // GET: Photographers/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Descripcion");
            return View();
        }

        // POST: Photographers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PhotographerView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var photographer = ToPhotographer(view);
                photographer.Image = pic;
                db.Photographers.Add(photographer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Descripcion", view.CategoryId);
            return View(view);
        }

        private Photographer ToPhotographer(PhotographerView view)
        {
            return new Photographer
            {
                Category = view.Category,
                CategoryId = view.CategoryId,
                Description = view.Description,
                Image = view.Image,
                IsActivte = view.IsActivte,
                LastEvent = view.LastEvent,
                PhotographerId = view.PhotographerId,
                Price = view.Price,
                Remarks = view.Remarks,
                Stock = view.Stock,
            };
        }

        // GET: Photographers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photographer = await db.Photographers.FindAsync(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Descripcion", photographer.CategoryId);
            var view = ToView(photographer);
            return View(view);
        }

        private PhotographerView ToView(Photographer photographer)
        {
            return new PhotographerView
            {
                Category = photographer.Category,
                CategoryId = photographer.CategoryId,
                Description = photographer.Description,
                Image = photographer.Image,
                IsActivte = photographer.IsActivte,
                LastEvent = photographer.LastEvent,
                PhotographerId = photographer.PhotographerId,
                Price = photographer.Price,
                Remarks = photographer.Remarks,
                Stock = photographer.Stock,
            };
        }

        // POST: Photographers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( PhotographerView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Image;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var photographer = ToPhotographer(view);
                photographer.Image = pic;

                db.Entry(photographer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Descripcion", view.CategoryId);
            return View(view);
        }

        // GET: Photographers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photographer photographer = await db.Photographers.FindAsync(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // POST: Photographers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Photographer photographer = await db.Photographers.FindAsync(id);
            db.Photographers.Remove(photographer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
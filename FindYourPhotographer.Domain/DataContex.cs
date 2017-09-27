
using System.Data.Entity;

namespace FindYourPhotographer.Domain
{
    public class DataContext : DbContext
    {
        public  DataContext() : base("DefaultConnection")
         {

        }
        public System.Data.Entity.DbSet<FindYourPhotographer.Domain.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<FindYourPhotographer.Domain.Photographer> Photographers { get; set; }
    }
}

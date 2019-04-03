using Microsoft.EntityFrameworkCore;

namespace ZadatakNeki.Models
{
    public class ToDoContext : DbContext
    {
        
        public ToDoContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Osoba> Osobe { get; set; }
        public virtual DbSet<Uredjaj> Uredjaji { get; set; }
        public virtual DbSet<Kancelarija> Kancelarije { get; set; }
        public virtual DbSet<OsobaUredjaj> OsobaUredjaj { get; set; }
    }
}

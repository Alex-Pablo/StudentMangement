using Microsoft.EntityFrameworkCore;

namespace StudentMangement.Models
{
    public class Db: DbContext
    {
        public Db(DbContextOptions<Db> options ): base (options) { }

        public DbSet<Student> Students { get; set; }

    }

}

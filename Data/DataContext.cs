using dotnetEFAndJWT.classes;
using Microsoft.EntityFrameworkCore;

namespace dotnetEFAndJWT.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class DataContext: DbContext
{
    public DbSet<tblDuLieu> tblDuLieus { get; set; }
    public DbSet<SoLan> soLans {get; set;}



    public DataContext()
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=dulieu.db;");
    }
}   
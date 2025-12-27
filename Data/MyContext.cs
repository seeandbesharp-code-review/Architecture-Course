using ChineseRaffleApi.Models;
using Microsoft.EntityFrameworkCore;


namespace ChineseRaffleApi.Data
{
    public class MyContext: DbContext
    {
        //private const string ConnectionString = "Server=Srv2\\pupils;DataBase=PhotoPoint;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True;";
        //private const string ConnectionString = "Server=localhost;Database=YourDatabaseName;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True";
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Donor> Donors => Set<Donor>();
        public DbSet<Gift> Gifts => Set<Gift>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Basket> Baskets => Set<Basket>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gift>()
                .HasIndex(g => g.Title)
                .IsUnique();
                     
            modelBuilder.Entity<User>()
              .HasIndex(u => u.UserName)
              .IsUnique();
        }




        //public static MyContext CreateContext()
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
        //    optionsBuilder.UseSqlServer(ConnectionString);
        //    return new MyContext(optionsBuilder.Options);
        //}

    }
}

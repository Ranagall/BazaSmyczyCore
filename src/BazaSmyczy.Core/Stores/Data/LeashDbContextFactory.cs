//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;

//namespace BazaSmyczy.Core.Stores.Data
//{
//    public class LeashDbContextFactory : IDbContextFactory<LeashDbContext>
//    {
//        public LeashDbContext Create(DbContextFactoryOptions options)
//        {
//            var builder = new DbContextOptionsBuilder<LeashDbContext>();
//            builder.UseNpgsql("User ID=sever;Password=ziom;Host=localhost;Port=5445;Database=LeashTestDb2;Pooling=true;");
//            return new LeashDbContext(builder.Options);
//        }
//    }
//}

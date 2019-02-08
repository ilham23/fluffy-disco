namespace Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private DbContext dbContext;

        public DbContext Init()
        {
            return dbContext ?? (dbContext = new DbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
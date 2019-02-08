using Model.Models;

namespace Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private DbContext dbContext;

        private IRepository<Product> _productRepository;
        private IRepository<ProductCategory> _productCategoryRepository;
        private IRepository<Error> _errorRepository;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public DbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }


        public IRepository<Product> ProductRepository {
            get { return _productRepository ?? (_productRepository = new Repository<Product>(dbFactory)); }
        }

        public IRepository<ProductCategory> ProductCategoryRepository
        {
            get { return _productCategoryRepository ?? (_productCategoryRepository = new Repository<ProductCategory>(dbFactory)); }
        }

        public IRepository<Error> ErrorRepository
        {
            get { return _errorRepository ?? (_errorRepository = new Repository<Error>(dbFactory)); }
        }
        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
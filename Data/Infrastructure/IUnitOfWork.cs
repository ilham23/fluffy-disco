using Model.Models;

namespace Data.Infrastructure
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }
        IRepository<ProductCategory> ProductCategoryRepository { get; }
        IRepository<Error> ErrorRepository { get; }
        void Commit();
    }
}
using Data.Infrastructure;
using Model.Models;

namespace Service
{
    public interface IErrorService
    {
        Error Create(Error error);

        void Save();
    }

    public class ErrorService : IErrorService
    {
        private IRepository<Error> _errorRepository;
        private IUnitOfWork _unitOfWork;

        public ErrorService(IUnitOfWork unitOfWork)
        {
            this._errorRepository = unitOfWork.ErrorRepository;
            this._unitOfWork = unitOfWork;
        }

        public Error Create(Error error)
        {
            return _errorRepository.Add(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
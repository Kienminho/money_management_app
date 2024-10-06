using Common.UnitOfWork.UnitOfWorkPattern;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMemoryCache _memoryCache;

        public BaseService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            this._unitOfWork = unitOfWork;
            this._memoryCache = memoryCache;
        }
    }
}

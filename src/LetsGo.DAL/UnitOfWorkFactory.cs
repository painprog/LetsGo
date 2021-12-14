using LetsGo.Core;
using LetsGo.DAL.Contracts;

namespace LetsGo.DAL
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IApplicationDbContextFactory _applicationDbContextFactory;

        public UnitOfWorkFactory(IApplicationDbContextFactory applicationDbContextFactory)
        {
            _applicationDbContextFactory = applicationDbContextFactory;
        }


        public IUnitOfWork MakeUnitOfWork()
        {
            return new UnitOfWork(_applicationDbContextFactory.Create());
        }
    }
}
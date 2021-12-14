namespace LetsGo.Core
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork MakeUnitOfWork();
    }
}
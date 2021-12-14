namespace LetsGo.DAL.Contracts
{
    public interface IApplicationDbContextFactory
    {
        ApplicationDbContext Create();
    }
}
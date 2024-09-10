namespace Koi.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public IKoiFishRepository KoiFishRepository { get; }
        public IKoiBreedRepository KoiBreedRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
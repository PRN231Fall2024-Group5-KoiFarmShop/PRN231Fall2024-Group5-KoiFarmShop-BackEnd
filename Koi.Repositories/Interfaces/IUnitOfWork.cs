namespace Koi.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        public IKoiFishRepository KoiFishRepository { get; }
        public IKoiBreedRepository KoiBreedRepository { get; }
        public IOrderRepository OrderRepository { get; }
        IWalletRepository WalletRepository { get; }

        Task<int> SaveChangeAsync();
    }
}
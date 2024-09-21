namespace Koi.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        public IKoiFishRepository KoiFishRepository { get; }
        public IKoiBreedRepository KoiBreedRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IKoiDiaryRepository KoiDiaryRepository { get; }
        IWalletRepository WalletRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        public IKoiCertificateRepository KoiCertificateRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
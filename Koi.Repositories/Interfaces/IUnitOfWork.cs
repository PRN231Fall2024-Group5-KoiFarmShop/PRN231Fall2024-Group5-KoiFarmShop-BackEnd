namespace Koi.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        public IKoiFishRepository KoiFishRepository { get; }
        public IKoiBreedRepository KoiBreedRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IKoiDiaryRepository KoiDiaryRepository { get; }
        public IWalletRepository WalletRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IKoiImageRepository KoiImageRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }

        public IKoiCertificateRepository KoiCertificateRepository { get; }
        public IDietRepository DietRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
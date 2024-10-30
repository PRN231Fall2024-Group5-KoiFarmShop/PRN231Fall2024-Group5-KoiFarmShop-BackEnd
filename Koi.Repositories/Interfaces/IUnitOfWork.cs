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

                public IBlogRepository BlogRepository { get; }
                public IConsignmentForNurtureRepository ConsignmentForNurtureRepository { get; }
                public INotificationRepository NotificationRepository { get; }
                public IWithdrawnRequestRepository WithdrawnRequestRepository { get; }

                public IRequestForSaleRepository RequestForSaleRepository { get; }

                Task<int> SaveChangeAsync();
        }
}
﻿using Koi.Repositories.Interfaces;

namespace Koi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KoiFarmShopDbContext _context;
        private readonly IUserRepository _userRepository;

        private readonly IKoiFishRepository _koiFishRepository;

        private readonly IKoiBreedRepository _koiBreedRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IKoiCertificateRepository _koiCertificateRepository;
        private readonly IKoiDiaryRepository _koiDiaryRepository;
        private readonly IKoiImageRepository _koiImageRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IDietRepository _dietRepository;
        public UnitOfWork(KoiFarmShopDbContext context,
            IKoiBreedRepository koiBreedRepository,
            IKoiFishRepository koiFishRepository,
            IUserRepository userRepository,
            IKoiCertificateRepository koiCertificateRepository,
            IOrderRepository orderRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            IKoiDiaryRepository koiDiaryRepository,
            IKoiImageRepository koiImageRepository,
            IOrderDetailRepository orderDetailRepository,
            IDietRepository dietRepository

        )
        {
            _koiFishRepository = koiFishRepository;
            _koiBreedRepository = koiBreedRepository;
            _context = context;
            _userRepository = userRepository;
            _koiCertificateRepository = koiCertificateRepository;
            _orderRepository = orderRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _koiDiaryRepository = koiDiaryRepository;
            _koiImageRepository = koiImageRepository;
            _orderDetailRepository = orderDetailRepository;
            _dietRepository = dietRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IDietRepository DietRepository => _dietRepository;
        public IKoiFishRepository KoiFishRepository => _koiFishRepository;
        public IKoiBreedRepository KoiBreedRepository => _koiBreedRepository;
        public IKoiDiaryRepository KoiDiaryRepository => _koiDiaryRepository;
        public IKoiCertificateRepository KoiCertificateRepository => _koiCertificateRepository;

        public IOrderRepository OrderRepository => _orderRepository;
        public IWalletRepository WalletRepository => _walletRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;
        public IKoiImageRepository KoiImageRepository => _koiImageRepository;
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;
        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
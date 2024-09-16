using Koi.Repositories.Interfaces;

namespace Koi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KoiFarmShopDbContext _context;
        private readonly IUserRepository _userRepository;

        private readonly IKoiFishRepository _koiFishRepository;

        private readonly IKoiBreedRepository _koiBreedRepository;
        private readonly IOrderRepository _orderRepository;

        public UnitOfWork(KoiFarmShopDbContext context,
            IKoiBreedRepository koiBreedRepository,
            IKoiFishRepository koiFishRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository

        )
        {
            _koiFishRepository = koiFishRepository;
            _koiBreedRepository = koiBreedRepository;
            _context = context;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public IUserRepository UserRepository => _userRepository;

        public IKoiFishRepository KoiFishRepository => _koiFishRepository;
        public IKoiBreedRepository KoiBreedRepository => _koiBreedRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
using HostME.Core.UnitOfWork.Repository;
using HostME.Data.Models;

namespace HostME.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HostMeContext _context;
        private IGenericRepository<Hostel>? _hostels;
        private IGenericRepository<Room>? _rooms;
        private IGenericRepository<HostelManager>? _hostelmanagers;
        private IGenericRepository<Booking>? _bookings;

        public UnitOfWork(HostMeContext context)
        {
            _context = context;
        }

        public IGenericRepository<Booking> BookingsRepository => _bookings ??= new GenericRepository<Booking>(_context);

        public IGenericRepository<HostelManager> HostelManagerRepository => _hostelmanagers ??= new GenericRepository<HostelManager>(_context);

        public IGenericRepository<Room> RoomRepository => _rooms ??= new GenericRepository<Room>(_context);

        public IGenericRepository<Hostel> HostelRepository => _hostels ??= new GenericRepository<Hostel>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

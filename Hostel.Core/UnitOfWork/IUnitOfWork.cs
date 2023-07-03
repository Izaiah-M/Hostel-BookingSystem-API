using HostME.Core.UnitOfWork.Repository;
using HostME.Data.Models;

namespace HostME.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Booking> BookingsRepository { get; }

        IGenericRepository<HostelManager> HostelManagerRepository { get; }

        IGenericRepository<Room> RoomRepository { get; }

        IGenericRepository<Hostel> HostelRepository { get; }

        Task Save();
    }
}

using HostME.Core.UnitOfWork.Repository;
using HostME.Data.Models;

namespace HostME.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Booking> BookingsRepository { get; }

        IGenericRepository<HostelManager> HostelManagerRepository { get; }

        IGenericRepository<HostelResident> HostelResidentRepository { get; }

        IGenericRepository<Room> RoomRepository { get; }

        IGenericRepository<Hostel> HostelRepository { get; }

        IGenericRepository<ApiUser> UserRepository { get; }

        Task Save();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}

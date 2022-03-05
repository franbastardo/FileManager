using FileManager.Models;
using System;
using System.Threading.Tasks;

namespace FileManager.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Credentials> Credentials { get; }
        Task Save();
    }
}

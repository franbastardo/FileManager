using FileManager.Data;
using FileManager.IRepository;
using FileManager.Models;
using System;
using System.Threading.Tasks;

namespace FileManager.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FileManagerContext _context;
        private IGenericRepository<User> _users;
        private IGenericRepository<Credentials> _credentials;
        public UnitOfWork( FileManagerContext context)
        {
            _context = context;
        }
        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);

        public IGenericRepository<Credentials> Credentials => _credentials ??= new GenericRepository<Credentials>(_context);

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

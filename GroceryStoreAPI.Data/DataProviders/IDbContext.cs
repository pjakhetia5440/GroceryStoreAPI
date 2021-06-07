using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Data
{
    public interface IDbContext : IDisposable
    {
        #region Methods
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync();
        int SaveChanges();
        #endregion Methods
    }
}

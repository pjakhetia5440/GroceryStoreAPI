using Microsoft.EntityFrameworkCore;
using Serilog;
using GroceryStoreAPI.Data.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public interface IDBServices
    {
        Task<IList<Entities.Customer>> GetAllCustomerFromDB();
        Task<Entities.Customer> GetCustomerByIdFromDB(int Id);
        Task<bool> AddCustomer(Entities.Customer req);
        Task<bool> DeleteCustomer(int id);
        Task<bool> UpdateCustomerByIdFromDB(Entities.Customer tr, DomainModels.Customer sr);
    }
    public class DBServices : IDBServices
    {
        private readonly ILogger _logger;
        private readonly GroceryStoreAPIDBContext _groceryStoreAPIDBContext;
        public DBServices(ILogger logger, GroceryStoreAPIDBContext groceryStoreAPIDBContext)
        {
            _logger = logger;
            _groceryStoreAPIDBContext = groceryStoreAPIDBContext;
        }

        public async Task<IList<Entities.Customer>> GetAllCustomerFromDB()
        {
            try
            {
                return await _groceryStoreAPIDBContext.Customer
                 .OrderByDescending(i => i.ID)
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("ReadRequest - Error Occurred {0}", ex.Message);
                return null;
            }
        }
        public async Task<Entities.Customer> GetCustomerByIdFromDB(int Id)
        {
            try
            {
                return await _groceryStoreAPIDBContext.Customer
                  .Where(i => i.ID.Equals(Id))
                  .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("GetCustomerByIdFromDB - Error Occurred {0}", ex.Message);
                return null;
            }
        }
        public async Task<bool> AddCustomer(Entities.Customer req)
        {
            try
            {
                if (req is null)
                    return (false);
                _groceryStoreAPIDBContext.Add(req);
                await _groceryStoreAPIDBContext.SaveChangesAsync();
                return (true);
            }
            catch (Exception ex)
            {
                _logger.Error("AddCustomer - Error Occurred {0}", ex.Message);
                return false;
            }
        }
        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                var result = await GetCustomerByIdFromDB(id);
                if (result != null)
                {
                    _groceryStoreAPIDBContext.Remove(result);
                    await _groceryStoreAPIDBContext.SaveChangesAsync();
                    return (true);
                }
                return (false);
            }
            catch (Exception ex)
            {
                _logger.Error("DeleteCustomer - Error Occurred {0}", ex.Message);
                return (false);
            }
        }
        public async Task<bool> UpdateCustomerByIdFromDB(Entities.Customer tr, DomainModels.Customer sr)
        {
            tr.Name = sr.Name;
            _groceryStoreAPIDBContext.Update(tr);
            await _groceryStoreAPIDBContext.SaveChangesAsync();
            return (true);
        }

    }
}

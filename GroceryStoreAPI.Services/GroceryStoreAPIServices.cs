using GroceryStoreAPI.DomainModels;
using GroceryStoreAPI.DomainModels.DomainEntityMapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public interface IGroceryStoreAPIServices
    {
        Task<DomainModels.Customer> GetCustomerById(int Id);
        Task<IList<DomainModels.Customer>> GetAllCustomers();
        Task<GeneralOutput> AddCustomer(GeneralInput srequest);
        Task<bool> DeleteCustomer(int id);
        Task<GeneralOutput> UpdateCustomer(Customer servRequest);
    }
    public class GroceryStoreAPIServices : IGroceryStoreAPIServices
    {
        private readonly ILogger _logger;
        private readonly IDBServices _dbServices;
        public GroceryStoreAPIServices(ILogger logger, IDBServices dbServices)
        {
            _logger = logger;
            _dbServices = dbServices;
        }
        public async Task<DomainModels.Customer> GetCustomerById(int Id)
        {
            try
            {
                var result = await _dbServices.GetCustomerByIdFromDB(Id);

                if (result is not null)
                {
                    return result.ToDomainModel();
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("GetCustomerById - Error Occurred {0}", ex.Message);
                throw;
            }
        }
        public async Task<IList<DomainModels.Customer>> GetAllCustomers()
        {
            try
            {
                var result = await _dbServices.GetAllCustomerFromDB();

                if (result is not null && result.Count > 0)
                {
                    return result.Select(c => c.ToDomainModel()).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("GetAllCustomers - Error Occurred {0}", ex.Message);
                throw;
            }
        }
        public async Task<GeneralOutput> AddCustomer(GeneralInput srequest)
        {
            var outPut = new GeneralOutput
            {
                Status = HttpStatusCode.BadRequest,
                ErrorMessage = ""
            };
            var TReq = new Entities.Customer
            {
                Name = srequest.Name
            };
            var result = await _dbServices.AddCustomer(TReq);
            if (result)
            {
                outPut.Status = HttpStatusCode.OK;               
                return outPut;
            }
            else
            {
                return outPut;
            }
        }
        public async Task<bool> DeleteCustomer(int id)
        {
            var result = await _dbServices.DeleteCustomer(id);
            return result;
        }
        public async Task<GeneralOutput> UpdateCustomer(Customer servRequest)
        {
            var outPut = new GeneralOutput();

            var result = await _dbServices.GetCustomerByIdFromDB(servRequest.ID);
            if (result is null)
            {
                outPut.Status = HttpStatusCode.NotFound;
                outPut.ErrorMessage = "customer not found to update";
                return outPut;
            }
            if(string.IsNullOrWhiteSpace(servRequest.Name))
            {
                outPut.Status = HttpStatusCode.BadRequest;
                outPut.ErrorMessage = "invalid name";
                return outPut;
            }

            var updateResult = await _dbServices.UpdateCustomerByIdFromDB(result, servRequest);
            if (updateResult)
            {
                outPut.Status = HttpStatusCode.OK;
                outPut.ErrorMessage = "customer info updated";
            }

            return outPut;
        }
    }
}

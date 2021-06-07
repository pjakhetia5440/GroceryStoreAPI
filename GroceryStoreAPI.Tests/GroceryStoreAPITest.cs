using FluentAssertions;
using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Net;
using Xunit;

namespace GroceryStoreAPI.Tests
{
    public class GroceryStoreAPITest: SqlLiteProvider
    {
        private readonly IDBServices _dbServices;
        private readonly IGroceryStoreAPIServices _groceryStoreAPIServices;
        private readonly GroceryStoreAPIController _groceryStoreAPIController;
        public GroceryStoreAPITest()
        {
            var logger = Log.Logger;
            _dbServices = new DBServices(logger, DbContext);
            _groceryStoreAPIServices = new GroceryStoreAPIServices(logger, _dbServices);
            _groceryStoreAPIController = new GroceryStoreAPIController(logger, _groceryStoreAPIServices);
        }

        [Fact]
        public async void MockDBSetupConnectionTest()
        {
            //Arrange Act Assert
            var connection = await DbContext.Database.CanConnectAsync();
            connection.Should().BeTrue();
        }

        [Fact]
        public async void GetActionTest()
        {
            //Arrange Act Assert
            var getRecord = await _groceryStoreAPIController.Get(1);
            getRecord.Should().BeOfType<OkObjectResult>();

            var getRecordById = await _groceryStoreAPIController.Get(2);
            getRecordById.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void PostActionPassTest()
        {
            //Arrange
            var newCustomerRequest = new DomainModels.GeneralInput
            {
                Name = "Test Prasoon"
            };
            //Act
            var createRecord = await _groceryStoreAPIController.Post(newCustomerRequest);

            //Assert
            createRecord.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async void PostActionFailTest()
        {
            //Arrange
            var newCustomerRequest = new DomainModels.GeneralInput();           

            //Act
            var createRecord = await _groceryStoreAPIController.Post(newCustomerRequest);

            //Assert
            createRecord.Should().BeOfType<BadRequestObjectResult>("because object is empty");
        }

        [Fact]
        public async void DeleteActionPassTest()
        {
            //Arrange
            int id = 1;
            //Act
            var deleteRecord = await _groceryStoreAPIController.Delete(id);
            //Assert
            deleteRecord.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async void DeleteActionFailTest()
        {
            //Arrange
            int id = 0;
            //Act
            var deleteRecord = await _groceryStoreAPIController.Delete(id);
            //Assert
            deleteRecord.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void GetCustomerTest()
        {
            var getRecords = await _groceryStoreAPIController.Get();
            getRecords.Should().BeOfType<OkObjectResult>();
        }


        [Fact]
        public async void PutActionPassTest()
        {
            //Arrange
            var newCustRequest = new DomainModels.Customer
            {
                ID = 1,
               Name = "Test Customer Updated"
            };

            //Act
            var updatedRecord = await _groceryStoreAPIController.Put(newCustRequest);
            //Assert
            updatedRecord.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void PutActionFailTest()
        {
            //Arrange
            var newCustRequest = new DomainModels.Customer
            {
                ID = 100,
                Name = "Test Customer Updated"
            };
            //Act
            var updatedRecord = await _groceryStoreAPIController.Put(newCustRequest);
            //Assert
            updatedRecord.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void PutActionFail2Test()
        {
            //Arrange
            var newCustRequest = new DomainModels.Customer
            {
                ID = 100,
                Name = "Update customer"
            };

            //Act
            var updatedRecord = await _groceryStoreAPIController.Put(newCustRequest);
            //Assert
            updatedRecord.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void PutActionFail3Test()
        {
            //Arrange
            var newCustRequest = new DomainModels.Customer
            {
                ID = 1,
                Name = ""
            };

            //Act
            var updatedRecord = await _groceryStoreAPIController.Put(newCustRequest);
            //Assert
            updatedRecord.Should().BeOfType<BadRequestObjectResult>();
        }


    }
}

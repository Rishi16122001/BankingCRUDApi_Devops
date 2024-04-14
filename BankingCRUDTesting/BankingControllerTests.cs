using BankingCRUDApi_Devops.Controllers;
using BankingCRUDApi_Devops.Data;
using BankingCRUDApi_Devops.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankingCRUDTesting
{
    public class BankingControllerTests
    {
        [Fact]
        public async Task GetAllCustomerDetails_ReturnsListOfCustomers()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<BankingDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseOne")
                .Options;

            using (var context = new BankingDataContext(dbContextOptions))
            {

                var controller = new BankingController(context);
                var customers = new List<CustomerModel>
                {
                    new CustomerModel { CustomerName = "Customer 1", CustomerCity = "City 1", AccountNumber = 123, CustomerEmail = "Email 1" },
                    new CustomerModel { CustomerName = "Customer 2", CustomerCity = "City 2", AccountNumber = 1234, CustomerEmail = "Email 2" }
                };
                await context.CustomerDetails.AddRangeAsync(customers);
                await context.SaveChangesAsync();

                // Act
                var result = await controller.GetAllCustomerDetails();

                // Assert

                if (result.Result == null)
                {
                    // Log or print a message to investigate why result.Result is null
                    Console.WriteLine("Result is null");
                }
                else
                {
                    var okResult = Assert.IsType<OkObjectResult>(result.Result);
                    var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<CustomerModel>>(okResult.Value);
                    Assert.Equal(2, returnedCustomers.Count());
                }
            }
        }

        [Fact]
        public async Task GetCustomerDetailsWithId_ReturnsCustomer_IfExists()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<BankingDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBase2")
                .Options;
            using (var context = new BankingDataContext(dbContextOptions))
            {
                var controller = new BankingController(context);
                var customer = new CustomerModel { Id = 1, CustomerName = "Customer 1", CustomerCity = "City 1", AccountNumber = 123, CustomerEmail = "Email 1" };
                await context.CustomerDetails.AddAsync(customer);
                await context.SaveChangesAsync();

                // Act
                var result = await controller.GetCustomerDetailsWithId(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedCustomer = Assert.IsType<CustomerModel>(okResult.Value);
                Assert.Equal(customer.CustomerName, returnedCustomer.CustomerName);
            }
        }

        [Fact]
        public async Task AddCustomer_ReturnsUpdatedCustomerList_WhenCustomerAdded()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<BankingDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new BankingDataContext(dbContextOptions))
            {
                var controller = new BankingController(context);
                var customer = new CustomerModel { CustomerName = "Customer 1", CustomerCity = "City 1", AccountNumber = 123, CustomerEmail = "Email 1" };

                // Act
                var result = await controller.AddCustomer(customer);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var updatedCustomers = Assert.IsAssignableFrom<IEnumerable<CustomerModel>>(okResult.Value);
                Assert.Contains(customer, updatedCustomers);
                // Add more assertions as needed
            }
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<BankingDataContext>()
                .UseInMemoryDatabase(databaseName: "TestingDatabase")
                .Options;
            using (var context = new BankingDataContext(dbContextOptions))
            {
                var controller = new BankingController(context);
                var customer = new CustomerModel { Id = 1, CustomerName = "Customer 1", CustomerCity = "City 1", AccountNumber = 123, CustomerEmail = "Email 1" };
                await context.CustomerDetails.AddAsync(customer);
                await context.SaveChangesAsync();

                // Act
                var result = await controller.DeleteCustomer(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var deletedCustomer = Assert.IsType<CustomerModel>(okResult.Value);
                Assert.Equal(customer, deletedCustomer);
                // Add more assertions as needed
            }
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsUpdatedCustomer_WhenCustomerExists()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<BankingDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseFive")
                .Options;

            using (var context = new BankingDataContext(dbContextOptions))
            {
                var controller = new BankingController(context);
                var existingCustomer = new CustomerModel { Id = 1, CustomerName = "Customer 1", CustomerCity = "City 1", AccountNumber = 123, CustomerEmail = "Email 1" };
                await context.CustomerDetails.AddAsync(existingCustomer);
                await context.SaveChangesAsync();

                var updatedCustomer = new CustomerModel { Id = 1, CustomerName = "Updated Customer 1", CustomerCity = "Updated City 1", AccountNumber = 123456, CustomerEmail = "Updated Email 1" };

                // Act
                var result = await controller.UpdateCustomer(updatedCustomer);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedCustomers = Assert.IsAssignableFrom<List<CustomerModel>>(okResult.Value);

                // Ensure that there is only one updated customer returned
                Assert.Single(returnedCustomers);

                // Verify properties of the returned customer
                var returnedCustomer = returnedCustomers.Single();
                Assert.Equal(updatedCustomer.Id, returnedCustomer.Id);
                Assert.Equal(updatedCustomer.CustomerName, returnedCustomer.CustomerName);
                Assert.Equal(updatedCustomer.CustomerCity, returnedCustomer.CustomerCity);
                Assert.Equal(updatedCustomer.AccountNumber, returnedCustomer.AccountNumber);
                Assert.Equal(updatedCustomer.CustomerEmail, returnedCustomer.CustomerEmail);
            }
        }

    }
}
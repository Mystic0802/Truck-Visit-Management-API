using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Truck_Visit_Management_API.Controllers;
using Truck_Visit_Management_API.Controllers.Models;
using Truck_Visit_Management_API.Data;
using Truck_Visit_Management_API.Data.Models;

namespace TruckVisitTests
{
    public class TruckVisitControllerTests
    {
        private readonly IDbPlaceholder _mockDb;
        private readonly TruckVisitController _controller;

        public TruckVisitControllerTests()
        {
            _mockDb = Substitute.For<IDbPlaceholder>();
            _controller = new TruckVisitController(_mockDb);
        }

        [Fact]
        public void GetAllTruckVisits_ReturnsOk_WithListOfTruckVisits()
        {
            // Arrange

            var activities = new List<VisitActivity>
            {
                new(VisitActivity.ActivityType.Delivery, "ABCD1234"),
                new(VisitActivity.ActivityType.Delivery, "WXYZ6789")
            };

            var truckVisits = new List<TruckVisit>
            {
                new(TruckVisit.TruckVisitStatus.PreRegistered, activities, "ABC123", new Driver("John", "Willard"), "Creator1"),
                new(TruckVisit.TruckVisitStatus.PreRegistered, activities, "DEF456", new Driver("Sarah", "Good"), "Creator 2"),
                new(TruckVisit.TruckVisitStatus.PreRegistered, activities, "GHI789", new Driver("John", "Proctor"), "Creator 3")
            };

            var returnable = truckVisits.AsQueryable();
            _mockDb.TruckVisits.GetAll().Returns(returnable);

            // Act
            var result = _controller.GetAllTruckVisits();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTruckVisits = Assert.IsAssignableFrom<IEnumerable<TruckVisit>>(okResult.Value);
            Assert.Equal(3, returnedTruckVisits.Count());
        }

        [Fact]
        public void CreateTruckVisit_ReturnsBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            var activities = new List<VisitActivityRequest>
            {
                new() {ActivityType = VisitActivity.ActivityType.Delivery, UnitNum = "ABCD1234" },
                new() {ActivityType = VisitActivity.ActivityType.Delivery, UnitNum= "WXYZ6789"}
            };

            var invalidRequest = new CreateTruckVisitRequest { Status = TruckVisit.TruckVisitStatus.Completed, Activities = activities, LicensePlate = "ABCD 123" }; // Invalid reason: Missing required params

            // Act
            var result = _controller.CreateTruckVisit(invalidRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Driver does not exist. Create a new Driver first.", badRequestResult.Value);
        }

        [Fact]
        public void CreateTruckVisit_ReturnsCreated_WhenRequestIsValid()
        {
            // Arrange
            var validRequest = new CreateTruckVisitRequest
            {
                LicensePlate = "ABC123",
                DriverId = 1,
                Activities = new List<VisitActivityRequest>
                {
                    new() { ActivityType = VisitActivity.ActivityType.Delivery, UnitNum = "Unit1234" }
                },
                Status = TruckVisit.TruckVisitStatus.PreRegistered,
                CreatedBy = "Admin"
            };

            var driver = new Driver("Giles", "Corey");
            _mockDb.Drivers.GetById(Arg.Any<Func<Driver, bool>>()).Returns(driver);

            // Act
            var result = _controller.CreateTruckVisit(validRequest);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);

            var createdTruckVisit = Assert.IsType<TruckVisit>(createdResult.Value);
            Assert.Equal("ABC123", createdTruckVisit.LicensePlate);
        }

        [Fact]
        public void UpdateTruckVisitStatus_ReturnsNotFound_WhenTruckVisitDoesNotExist()
        {
            // Arrange
            var updateRequest = new UpdateTruckVisitRequest { Status = TruckVisit.TruckVisitStatus.Completed, UpdatedBy = "Admin" };

            _mockDb.TruckVisits.GetById(Arg.Any<Func<TruckVisit, bool>>()).Returns((TruckVisit)null);

            // Act
            var result = _controller.UpdateTruckVisitStatus(1, updateRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Truck visit not found", notFoundResult.Value);
        }

        [Fact]
        public void UpdateTruckVisitStatus_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var activities = new List<VisitActivity> { new(VisitActivity.ActivityType.Delivery, "ABCD1234") };
            var truckVisit = new TruckVisit(TruckVisit.TruckVisitStatus.AtGate, activities, "ABC123", new Driver("John", "Willard"), "Creator1");
            var updateRequest = new UpdateTruckVisitRequest { Status = TruckVisit.TruckVisitStatus.Completed, UpdatedBy = "Admin" };

            _mockDb.TruckVisits.GetById(Arg.Any<Func<TruckVisit, bool>>()).Returns(truckVisit);

            // Act
            var result = _controller.UpdateTruckVisitStatus(truckVisit.Id, updateRequest);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(TruckVisit.TruckVisitStatus.Completed, truckVisit.Status);
            Assert.Equal("Admin", truckVisit.UpdatedBy);
        }
    }
}
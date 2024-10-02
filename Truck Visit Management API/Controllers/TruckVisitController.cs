using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Truck_Visit_Management_API.Controllers.Models;
using Truck_Visit_Management_API.Data;
using Truck_Visit_Management_API.Data.Models;

namespace Truck_Visit_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckVisitController : ControllerBase
    {
        private readonly IDbPlaceholder _db;

        public TruckVisitController(IDbPlaceholder db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<TruckVisit>> GetAllTruckVisits()
        {
            var truckVisits = _db.TruckVisits.GetAll().ToList();
            return Ok(truckVisits);
        }

        [Authorize]
        [HttpPost("create")]
        public ActionResult<TruckVisit> CreateTruckVisit([FromBody] CreateTruckVisitRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.LicensePlate) || !request.LicensePlate.IsAlphanumeric())
            {
                return BadRequest("Invalid request");
            }

            var truckDriver = _db.Drivers.GetById(d => d.Id == request.DriverId);
            if (truckDriver == null)
            {
                return BadRequest("Driver does not exist. Create a new Driver first.");
            }

            var activities = request.Activities
                .Where(a => VisitActivity.IsValidUnitNumber(a.UnitNum))
                .Select(a => new VisitActivity(a.ActivityType, a.UnitNum))
                .ToList();

            var newTruckVisit = new TruckVisit(request.Status, activities, request.LicensePlate, truckDriver, request.CreatedBy);
            _db.TruckVisits.Add(newTruckVisit);

            return StatusCode(201, newTruckVisit);
        }

        [Authorize]
        [HttpPost("driver/create")]
        public ActionResult<Driver> CreateDriver([FromBody] CreateDriverRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            var newDriver = new Driver(request.FirstName, request.LastName);
            _db.Drivers.Add(newDriver);

            return StatusCode(201, newDriver);
        }

        [Authorize]
        [HttpPut("{id}/status")]
        public ActionResult UpdateTruckVisitStatus(int id, [FromBody] UpdateTruckVisitRequest request)
        {
            var truckVisit = _db.TruckVisits.GetById(tv => tv.Id == id);
            if (truckVisit == null)
            {
                return NotFound("Truck visit not found");
            }

            truckVisit.Status = request.Status;
            truckVisit.UpdatedBy = request.UpdatedBy;
            truckVisit.UpdatedTime = DateTime.Now;

            return NoContent();
        }
    }
}
